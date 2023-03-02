using System;
using System.Collections;
using System.Collections.Generic;
using AI.Enemies.Spells;
using DefaultNamespace;
using Domain;
using Domain.Services;
using Player;
using UnityEngine.SceneManagement;
using UniRx;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Weapons;
using Random = UnityEngine.Random;

public class PlayerView : MonoBehaviour, IEntityView, IDamageable, IKnockBackable, IStunable
{
    [SerializeField] private GameConfig _config;
    private const int BASICS_ATTACKS_COUNT = 2;
    public Rigidbody Rigidbody => _rigidbody;

    public float JumpForce => _jumpForce;

    public float Speed => _speed;

    public BoxCollider Collider { get; private set; }

    public IMeleeWeapon CurrentWeapon => GetComponentInChildren<IMeleeWeapon>();


    public Transform Transform
    {
        get { return transform; }
    }

    public ITarget CurrentTarget => _target;
    public float TargetingFieldOfViewDistance => _targetingFieldOfViewDistance;
    public float TargetingFieldOfViewAngle => _targetingFieldOfViewAngle;

    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject getDamageAnimation;
    [SerializeField] private GameObject diePanelAnimation;
    [SerializeField] private Lifebar healtBar;

    [SerializeField] private GameObject rootsFeedbackCanvas;

    private const string AHEAD_VELOCITY = "Velocity Z";
    private const string MOVING = "Moving";

    private ITarget _target;


    [Header("Movement Values")] [SerializeField]
    private float _speed;

    [SerializeField] private float _jumpForce;


    private PlayerPresenter _presenter;

    [Header("GetHP Feedback")] [SerializeField]
    private ParticleSystem HPParticle;

    [Header("Blink Feedback")] [SerializeField]
    private GameObject particleBlink;

    [Header("Dash feedback")] [SerializeField]
    private GameObject _dashObject;

    [SerializeField] private List<GameObject> _objectsToTurnDownOnDash;
    [SerializeField] private List<Collider> _collidersToTurnDownOnDash;
    [SerializeField] private PostProcessVolume normalPostPro;
    [SerializeField] private PostProcessVolume dashPostPro;
    [SerializeField] private DashPlayerFeedback _dashFeedback;
    [SerializeField] private float _minDashHeight;

    public float MinDashHeight => _minDashHeight;
    public Vector3 DashPosition => _dashObject.transform.position;

    [Header("Targeting Values")] [SerializeField]
    private float _targetingFieldOfViewDistance;

    [SerializeField] private float _targetingFieldOfViewAngle;
    [SerializeField] private int _maxHealt;
    [SerializeField] private int currentHealt;
    private bool _isKnocked;
    public bool isDead;
    private float _lastTimeKnocked;
    [SerializeField] private float _knockedCooldown;


    [Header("Energy Service")] [SerializeField]
    private EnergyView _energyView;

    [SerializeField] private float _blinkEnergy;
    [SerializeField] private Transform _middleBodyTransform;
    [SerializeField] private LayerMask _stuckLayer;

    [SerializeField] private PlayerOneHandSword playerOneHandSword;
    public float BlinkEnergy => _blinkEnergy;

    private float _initialMass;


    private AudioSource _footstepsAudioSource;


    private void Awake()
    {
        PlayerState.Clean();
        Collider = GetComponent<BoxCollider>();
        _animator.SetInteger("Weapon", 1);
        _animator.SetInteger("Action", 1);
        var sb = new SurroundingBallsSpell();
        sb.Init(_config, (go) => Instantiate(go, transform));
        _presenter = new PlayerPresenter(this, Camera.main, _energyView, _config, sb);
        PlayerState.Instance.Transform = transform;
        currentHealt = _maxHealt;
        healtBar.SetMaxHealt(_maxHealt);
        _initialMass = _rigidbody.mass;

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            ActivateAfterCinematic();
        }

        _footstepsAudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _initialSpeed = 7;
        _config.Instance.PlayerSpeed = _initialSpeed;
        if (ManagerKeys.instance != null)
        {
            ManagerKeys.instance.UpdateKeys();
        }
    }

    private void Update()
    {
        PlayerState.Instance.CanAffordBlink = false;
        if (_energyView.IsAffordable(_blinkEnergy))
            PlayerState.Instance.CanAffordBlink = true;
        if (!isDead)
            _presenter.UpdatePlayer();

        if (!PlayerState.Instance.IsStunned && rootsFeedbackCanvas) rootsFeedbackCanvas.SetActive(false);
    }

    private float _initialSpeed;
    public void DebuffSpeed()
    {
        _config.Instance.PlayerSpeed = 3;
    }
    public void ResetSpeed()
    {
        _config.Instance.PlayerSpeed = _initialSpeed;
    }

    public void OnEnterPortal()
    {
        gameObject.SetActive(false);
    }

    public void UpdateRotation()
    {
        var dir = new Vector3(Rigidbody.velocity.x, 0, Rigidbody.velocity.z).normalized;
        transform.forward = dir;
    }

    public void UpdateMovementAheadAnimation(float aheadAmount)
    {
        _animator.SetFloat(AHEAD_VELOCITY, aheadAmount * _speed);
    }

    public void SetInMovement(bool isMoving)
    {
        _animator.SetBool(MOVING, isMoving);
    }

    public void JumpAnimation()
    {
        _animator.SetTrigger("JumpTrigger");
        _animator.SetInteger("Jumping", 1);
        var random = Random.Range(0, 2);
        if (random == 1)
        {
            AudioMaster.Instance.PlayClip("SaltoPersonaje",0.2f);
        }
        else return;
    }

    public void SetFootsteps(bool isMoving)
    {
        if (_isKnocked) return;

        float random = Random.Range(0f, 0.16f);
        _footstepsAudioSource.volume = isMoving ? random : 0;
    }

    public void ActivateAfterCinematic()
    {
        _energyView.gameObject.SetActive(true);
        healtBar.gameObject.SetActive(true);
    }
    public void ShowBlinkingFeedback()
    {
    }

    public void HideBlinkingFeedback()
    {
    }

    public void GroundedAnimation()
    {
        _animator.SetInteger("Jumping", 0);
    }

    public void FallAnimation()
    {
        _animator.SetTrigger("JumpTrigger");
        _animator.SetInteger("Jumping", 2);
    }

    public void AttackAnimation()
    {
        _animator.SetBool("HasToAttack", true);
        //_animator.SetTrigger("AttackTrigger");
        _animator.SetInteger("Action", Random.Range(0, BASICS_ATTACKS_COUNT) + 1);
        
    }

    public void SetTarget(ITarget target)
    {
        _target = target;
    }

    public void GetDamaged(int damage)
    {
        currentHealt -= damage;
        healtBar.SetHealt(currentHealt);
        getDamageAnimation.SetActive(true);
        AudioMaster.Instance.PlayClip("PlayerGetHit");
        if (currentHealt <= 0)
        {
            Die();
        }
    }

    public void GetHp(int life)
    {
        currentHealt += life;
        if (currentHealt > _maxHealt) currentHealt = _maxHealt;
        healtBar.SetHealt(currentHealt);
        FeedbackGetLife();
    }

    public void FeedbackGetLife()
    {
        if (HPParticle == null) return;
        if (!HPParticle.isPlaying) HPParticle.Play();
        AudioMaster.Instance.PlayClip("HealtSound");
    }

    public void Die()
    {
        diePanelAnimation.SetActive(true);
        isDead = true;
        PlayerState.Instance.IsDead = true;
        _animator.SetBool("IsDead", true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        //StartCoroutine(PauseSecuencially(2));
    }

    private IEnumerator PauseSecuencially(float time)
    {
        while (Time.timeScale != 0)
        {
            Time.timeScale -= Time.deltaTime / time;
            yield return null;
        }

        yield return null; 
    }

    public void GetKnockedBack(Vector3 force)
    {
        var lookAtHit = new Vector3(force.x, 0, force.z).normalized * -1;
        transform.forward = lookAtHit;
        Rigidbody.AddForce(force);
        PlayerState.Instance.IsKnocked = true;
        _animator.SetBool("isStunned", false);
        _animator.SetTrigger("Knocked");
        StartCoroutine(GetUpAfterKnockedCooldown());
    }

    public Vector3 GetCurrentPosition()
    {
        return transform.position;
    }

    private IEnumerator GetUpAfterKnockedCooldown()
    {
        _lastTimeKnocked = Time.time;
        while (IsKnocked())
        {
            yield return null;
        }

        _animator.SetTrigger("KnockStandUp");
        PlayerState.Instance.IsKnocked = false;
        PlayerState.Instance.IsRecoveringFromKnock = true;
        StartCoroutine(WaitUntilAnimationsEnds("Levantarse",
            () => PlayerState.Instance.IsRecoveringFromKnock = false));
        getDamageAnimation.SetActive(false);
        yield return null;
    }

    private IEnumerator WaitUntilAnimationsEnds(string stateName, Action todo)
    {
        while (!_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
        {
            yield return null;
        }

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(0).length);
        todo();
        yield return null;
    }


    public bool IsKnocked()
    {
        return Time.time - _lastTimeKnocked < _knockedCooldown;
    }

    public IEnergyView GetEnergyService()
    {
        return _energyView;
    }

    public bool IsStucked()
    {
        var colliderOffset = (Collider.size * 0.5f).magnitude;
        var ray = new Ray(_middleBodyTransform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, colliderOffset * 3, _stuckLayer))
        {
            var dismissStuck = hit.collider.GetComponent<DismissStuckAndAvoidPlayerDash>();
            if (dismissStuck != null)
            {
                return false;
            }

            return true;
        }

        return false;
    }

    public void SetDashing(bool isDashing)
    {
        if (isDashing)
            EnterDash();
        else
            FinishDash();
    }
    
    
    private void EnterDash()
    {
        AudioMaster.Instance.PlayClip("DashStart",0.15f);
        foreach (var obj in _objectsToTurnDownOnDash)
        {
            obj.SetActive(false);
        }

        foreach (var col in _collidersToTurnDownOnDash)
        {
            col.enabled = false;
        }

        playerOneHandSword.DisableCollision();
        _rigidbody.useGravity = false;
        _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, 0, _rigidbody.velocity.z);
        _rigidbody.mass = 0;
        var contrains = (int) RigidbodyConstraints.FreezePositionY + (int) RigidbodyConstraints.FreezeRotation;
        _rigidbody.constraints = (RigidbodyConstraints) contrains;
        _dashObject.SetActive(true);
        _animator.SetBool("ContinueCombo", false);
        _animator.SetTrigger("Dash");
        dashPostPro.enabled = true;
        normalPostPro.enabled = false;
    }

    private void FinishDash()
    {
        AudioMaster.Instance.PlayClip("DashFinish",0.15f);
        foreach (var obj in _objectsToTurnDownOnDash)
        {
            obj.SetActive(true);
        }

        foreach (var col in _collidersToTurnDownOnDash)
        {
            col.enabled = true;
        }

        _rigidbody.useGravity = true;
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.mass = _initialMass;
        _dashFeedback.DisablePlayerFeedback();
        _dashObject.SetActive(false);
        dashPostPro.enabled = false;
        normalPostPro.enabled = true;
    }

    public DashPlayerFeedback DashPlayerFeedback => _dashFeedback;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(_dashObject.transform.position, Vector3.down * _minDashHeight + _dashObject.transform.position);
    }

    public void GetStunned(float timeStunned)
    {
        if (PlayerState.Instance.IsDead) return;
        PlayerState.Instance.IsStunned = true;
        _animator.SetBool("isStunned", true);
        StunnedFeedback();
        FinishStun(timeStunned).Subscribe();
    }


    private IObservable<Unit> FinishStun(float timeStunned)
    {
        return Observable.Timer(TimeSpan.FromSeconds(timeStunned))
            .DoOnCompleted(() => PlayerState.Instance.IsStunned = false)
            .DoOnCompleted(() => _animator.SetBool("isStunned", false))
            .AsUnitObservable();
    }

    private void StunnedFeedback()
    {
        rootsFeedbackCanvas.gameObject.SetActive(true);
    }
}