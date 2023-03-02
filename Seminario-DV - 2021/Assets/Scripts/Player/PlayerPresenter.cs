using System.Collections.Generic;
using System.Linq;
using Domain;
using Domain.Services;
using Player;
using UnityEngine;

public class PlayerPresenter
{
    private readonly float DASH_HEIGHT_OFFSET = 0.05f;
    
    private readonly PlayerView _view;
    private readonly Camera _camera;
    private readonly IEnergyView _energy;
    private readonly GameConfig _config;
    private readonly SurroundingBallsSpell _surroundingBalls;
    private readonly PlayerMeleeAttack _meleeAttack;
    private readonly Transform _playerTransform;
    
    private int _jumpFramesTimer;
    private bool _hasJump;
    private bool _isDashing;
    private int _groundLayer;
    private SpellCooldownManager _spellsCooldownManager;
    private CastSpell _spellCaster;

    private const float GROUNDED_RADIUS = 0.15f;

    
    public PlayerPresenter(PlayerView view, Camera camera, IEnergyView energy, GameConfig config, SurroundingBallsSpell surroundingBalls)
    {
        _view = view;
        _camera = camera;
        _energy = energy;
        _config = config;
        _meleeAttack = new PlayerMeleeAttack();
        _playerTransform = _view.transform;
        _groundLayer = 1 << 10;
        _surroundingBalls = surroundingBalls;
        var spellList = new List<ISpell>();
        spellList.Add(_surroundingBalls);
        _spellsCooldownManager = new SpellCooldownManager(spellList);
        _spellCaster = new CastSpell(_spellsCooldownManager, _energy);
    }

    public void UpdatePlayer()
    {
        if (PlayerState.Instance.IsKnocked) return;
        if (PlayerState.Instance.IsStunned) return;
        if (PlayerState.Instance.IsRecoveringFromKnock)
        {
            Move();
            return;
        }
        if (PlayerState.Instance.IsBlinking) return;
        ChangeTarget();
        Dash();
        SurroundingBalls();
        if (PlayerState.Instance.IsAttacking && _view.CurrentTarget != null && !_isDashing) return;
        Move();
        if (PlayerState.Instance.IsDashing) return;
        Jump();
        Attack();
    }

    private void SurroundingBalls()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {

            var colliders = Physics.OverlapSphere(_playerTransform.position, 100)
                .Where(x=>x.GetComponent<BallSurrounding>())
                .ToList();

            if (colliders.Count > 0) return;
            if (ManagerKeys.instance)
            {
                if (ManagerKeys.instance.playerHasPowerUp)
                {
                    _spellCaster.Invoke(_surroundingBalls);
                }
            }
        }
    }

    private void Dash()
    {
        if (_isDashing)
        {
            PlayerState.Instance.IsAttacking = false;
            var cost = _config.Instance.DashEnergyCostBySecond * Time.deltaTime;
            if (_energy.IsAffordable(cost))
            {
                _energy.UseEnergy(cost);
                RaycastHit hit;
                if (Physics.Raycast(_view.DashPosition, Vector3.down, out hit, _view.MinDashHeight, _groundLayer))
                {
                    if (hit.collider.GetComponent<IIgnoreDash>() == null)
                        _view.transform.position = hit.point;
                }
            }
            else
            {
                StopDash();
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (_energy.IsAffordable(_config.Instance.DashEnergyCost))
            {
                _energy.UseEnergy(_config.Instance.DashEnergyCost);
                _isDashing = true;
                PlayerState.Instance.IsDashing = true;
                PlayerState.Instance.IsAttacking = false;
                _view.SetDashing(_isDashing);
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) && _isDashing)
        {
            StopDash();
        }
    }

    private void StopDash()
    {
        _isDashing = false;
        PlayerState.Instance.IsDashing = false;
        _view.SetDashing(_isDashing);
    }

    private void ChangeTarget()
    {
        if (PlayerState.Instance.IsBlinking) return;
        
        if (_view.CurrentTarget != null)
        {
            _view.CurrentTarget.ShowFeedback();
        }
        
        var dirToCheck = _playerTransform.forward;
        if (PlayerState.Instance.IsAttacking && PlayerState.Instance.CanAffordBlink)
        {
            var v = Input.GetAxis("Vertical");
            var h = Input.GetAxis("Horizontal");
            if(h != 0f || v != 0f) 
                dirToCheck = MovementDirFromCameraDir(v, h);
        }

        var currentTarget = _view.CurrentTarget;
        var candidatesInDistance = GetTargetsInDistance().ToList();
        if (candidatesInDistance.Any())
        {
            var candidatesInSightAngle = GetTargetsInSightAngle(candidatesInDistance,dirToCheck).ToList();
            if (candidatesInSightAngle.Any())
            {
                var finalTarget = GetClosestTarget(candidatesInSightAngle);
                if (finalTarget != currentTarget)
                {
                    if (currentTarget != null)
                    {
                        currentTarget.HideFeedback();
                    }  
                }
                if (PlayerState.Instance.IsAttacking && PlayerState.Instance.CanAffordBlink && currentTarget == null)
                {
                    _view.AttackAnimation();
                }
                _view.SetTarget(finalTarget);
                return;
            }
            else
            {
                if(currentTarget != null) currentTarget.HideFeedback();
            }
        }
        else
        {
            if(currentTarget != null) currentTarget.HideFeedback();
        }
        if(!PlayerState.Instance.IsAttacking) _view.SetTarget(null);
    }

    private IEnumerable<Transform> GetTargetsInDistance()
    {
        return Physics.OverlapSphere(_playerTransform.position, _view.TargetingFieldOfViewDistance)
            //.Select(collider => collider.GetComponent<ITarget>())
            .Select(collider => collider.transform)
            .Where(target => target.gameObject.layer == 15);//Layer 15 tienen que estar todos los targets.
    }

    private IEnumerable<Transform> GetTargetsInSightAngle(IEnumerable<Transform> candidates, Vector3 fromDir)
    {
        return candidates.Where(candidate =>
        {
            var candidatePosition = candidate.position;
            candidatePosition = new Vector3(candidatePosition.x, _playerTransform.position.y, candidatePosition.z);
            var dirToCandidate = (candidatePosition - _playerTransform.position).normalized;
            var angleToCandidate = Vector3.Angle(fromDir, dirToCandidate);
            return angleToCandidate <= _view.TargetingFieldOfViewAngle;
        });
    }

    private ITarget GetClosestTarget(IEnumerable<Transform> candidatesInSightAngle)
    {
        return candidatesInSightAngle.OrderBy(candidate =>
                Vector3.Distance(candidate.position, _playerTransform.position))
            .First().GetComponent<ITarget>();
    }

    private void Move()
    {
        if (PlayerState.Instance.IsAttacking && _view.CurrentTarget != null) return;
        var v = Input.GetAxis("Vertical");
        var h = Input.GetAxis("Horizontal");
        var moveDir = MovementDirFromCameraDir(v, h);

        var speed = _isDashing ? _config.Instance.DashSpeed : _config.Instance.PlayerSpeed;
        
        _view.Rigidbody.velocity = new Vector3(moveDir.x, 0, moveDir.z) * speed +
                                   new Vector3(0, _view.Rigidbody.velocity.y, 0);

        _view.UpdateMovementAheadAnimation(moveDir.magnitude);
        _view.SetInMovement(false);
        _view.SetFootsteps(false);
        if (h == 0f && v == 0f) return;

        _view.SetInMovement(true);
        _view.UpdateRotation();
        if (_isDashing || !IsGrounded()) return;
        _view.SetFootsteps(true);
    }

    private Vector3 MovementDirFromCameraDir(float v, float h)
    {
        var cameraForwardDir = _camera.transform.forward;
        var mapCameraForwardDirToXZPlane = new Vector3(cameraForwardDir.x, 0, cameraForwardDir.z).normalized;

        var cameraRight = _camera.transform.right;
        var mapCameraRightToXZPlane = new Vector3(cameraRight.x, 0, cameraRight.z).normalized;

        var moveDir = (mapCameraForwardDirToXZPlane * v + mapCameraRightToXZPlane * h).normalized;
        return moveDir;
    }

    private void Jump()
    {
        bool isGrounded = IsGrounded();
        if (!PlayerState.Instance.IsGrounded && isGrounded)
        {
            _view.GroundedAnimation();
            _hasJump = false;
        }
        else if (PlayerState.Instance.IsGrounded && !isGrounded)
        {
            if(!_hasJump)
                _view.FallAnimation();
        }

        PlayerState.Instance.IsGrounded = isGrounded;

        if (PlayerState.Instance.IsAttacking) return;

        if (PlayerState.Instance.IsGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _hasJump = true;
            _view.Rigidbody.velocity = new Vector3(_view.Rigidbody.velocity.x, 0, _view.Rigidbody.velocity.z);
            _view.Rigidbody.AddForce(Vector3.up * _view.JumpForce, ForceMode.Impulse);
            _view.JumpAnimation();
        }
    }

    private void Attack()
    {
        if ((Input.GetButtonDown("Fire1") || Input.GetKeyDown(KeyCode.K)) && !PlayerState.Instance.IsAttacking && PlayerState.Instance.IsGrounded)
        {
            PlayerState.Instance.IsAttacking = true;
            _meleeAttack.Execute(_view, _view.CurrentTarget != null);
            
        }
    }

    public bool IsGrounded()
    {
        return Physics.OverlapBox(_playerTransform.position + _view.Collider.center,
                   _view.Collider.size * 0.48f + Vector3.up * GROUNDED_RADIUS, //El 0.45 es para evitar que sobrepase el borde del collider
                   _view.transform.rotation,
                   _groundLayer).Length > 0;
    }
}

public class SpellCooldownManager
{
    private readonly List<ISpell> _spellList;
    private Dictionary<ISpell, float> _spellByLastTimeUsed;

    public SpellCooldownManager(List<ISpell> spellList)
    {
        _spellByLastTimeUsed = new Dictionary<ISpell, float>();
        
        _spellList = spellList;
        
        foreach (var spell in _spellList)
        {
            _spellByLastTimeUsed.Add(spell, 0);
        }
    }

    public bool IsSpellOnCooldown(ISpell spell)
    {
        if (!_spellList.Contains(spell)) return false;
        return (Time.time - _spellByLastTimeUsed[spell]) >= spell.Cooldown;
    }

    public void UseSpell(ISpell spell)
    {
        if(!_spellList.Contains(spell)) return;
        _spellByLastTimeUsed[spell] = Time.time;
    }
}