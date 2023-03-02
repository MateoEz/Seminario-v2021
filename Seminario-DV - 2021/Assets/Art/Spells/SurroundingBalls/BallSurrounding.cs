using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallSurrounding : MonoBehaviour
{
	[Header("Ball Ssurround Values")]
	[SerializeField] ParticleSystem attackParticles;
	[SerializeField] float rangeDetection;
	[SerializeField] float distanceFromOwner;
	[SerializeField] public float circleRotationSpeed;
	[SerializeField] float upDownMovementRadius;
	[SerializeField] float upDownSpeed;
	[SerializeField] public Transform owner;
	[SerializeField] LayerMask enemyAndPlayerLayerMask;
	[SerializeField] int enemysLayerNumber;
	[SerializeField] float attackSpeed;
	[SerializeField] Vector3 scaleOnAtack;
	[SerializeField] public float rotateSelfSpeed;
	[SerializeField] private int _powerValue;
	[SerializeField] private float _initialHeight;
	Vector3 rotateSelfDirection;
	Vector3 target;
	public float t;
	public float t2;

	enum SurroundingBallState { Idle, Attack, Attacking}
	SurroundingBallState currentState;
	[SerializeField] private Rigidbody _rigi;
	private Vector3 _backupTargetPosition;

	public void SetInitialCircleAngle(float angle)
	{
		t = Mathf.Deg2Rad * angle;
	}

	public void SetInitialUpDownAngle(float angle)
	{
		t2 = Mathf.Deg2Rad * angle;
	}

	private void Awake()
	{
		Invoke(nameof(DestroySpellAfterTime),Random.Range(4,5.5f));
		//Lo que se hacia en el awake anterior...
		/*
		 * Se corria una corrutina para auto destruir las bolas luego de un tiempo si no eran utilizadas.
		 * Se creaban los diccionarios vacios de que tipo de poder era el spell y las interacciones con los otros poderes.
		 */
		currentState = SurroundingBallState.Idle;
		rotateSelfDirection = new Vector3(UnityEngine.Random.Range(0, 9), UnityEngine.Random.Range(0, 9), UnityEngine.Random.Range(0, 9)).normalized;
	}

	private void Update()
	{
		//Lo que se hacia en el update anterior...
		/*
		 * Nada...
		 */
		switch (currentState)
		{
			case SurroundingBallState.Idle:
				SurroundingMovement();
				if (IsEnemyOnRange())
					currentState = SurroundingBallState.Attack;
				/*
					if (IsClearAttack())
						currentState = SurroundingBallState.Attack;
						*/
				break;
			case SurroundingBallState.Attack:
				Attack();
				break;
			default:
				break;
		}
	}

	protected void OnTriggerEnter(Collider other)
	{
		IDamageable entity = other.GetComponent<IDamageable>();
		if (entity == null) return;
		if (entity is PlayerView) return;
		entity.GetDamaged(_powerValue);
		DestroySpell();
	}

	private void SurroundingMovement()
	{
		t += Time.deltaTime;
		t2 += Time.deltaTime;
		float circleSin = Mathf.Sin(t * circleRotationSpeed);
		float circleCos = Mathf.Cos(t * circleRotationSpeed);
		float upDownSin = Mathf.Sin(t2 * upDownSpeed);
		transform.Rotate(rotateSelfDirection * rotateSelfSpeed);
		transform.position = owner.position +
		                     new Vector3(circleSin, 0, circleCos) * distanceFromOwner +
		                     Vector3.up * _initialHeight +
		                     Vector3.up * upDownSin * upDownMovementRadius;
	}

	private bool IsEnemyOnRange()
	{
		var colliders = Physics.OverlapSphere(this.transform.position, rangeDetection);
		foreach (var item in colliders)
		{
			var ballTarget = item.GetComponent<IBallSurroundingTarget>();
			if(ballTarget != null)//EnemyLayer
			{
				target = ballTarget.MiddleBodyPoint;
				return true;
			}
		}
		return false;
	}

	private bool IsClearAttack()
	{
		Vector3 dirToTargetIgnoringHeight = (target - transform.position).normalized;
		dirToTargetIgnoringHeight = new Vector3(dirToTargetIgnoringHeight.x, 0, dirToTargetIgnoringHeight.z).normalized;
		RaycastHit hit;
		if(Physics.Raycast(transform.position, dirToTargetIgnoringHeight, out hit, rangeDetection, enemyAndPlayerLayerMask))
		{
			if (hit.collider.gameObject.layer == enemysLayerNumber) return true;
		}
		return false;
	}

	private void Attack()
	{
		Vector3 targetDir;
		targetDir= (target - transform.position).normalized;
		transform.localScale = scaleOnAtack;
		transform.forward = targetDir;
		_rigi.AddForce(targetDir * attackSpeed, ForceMode.Impulse);
		currentState = SurroundingBallState.Attacking;
        AudioMaster.Instance.PlayClip("BallImpact", 0.3f, .5f);
        DestroyAfterAttack(1).Subscribe();
		//transform.position += targetDir * attackSpeed * Time.deltaTime;
	}

	private IObservable<Unit> DestroyAfterAttack(float time)
	{
		return Observable.Timer(TimeSpan.FromSeconds(time))
			.DoOnCompleted(DestroySpell)
			.AsUnitObservable();
	}

	private void DestroySpellAfterTime()
	{
		if (!IsEnemyOnRange())
		{
			attackParticles.loop = false;
			attackParticles.transform.parent = null;
			Destroy(gameObject);
		}
	}

	private void DestroySpell()
	{
		if (this)
		{
			attackParticles.loop = false;
			attackParticles.transform.parent = null;
			Destroy(gameObject);
		}

	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, rangeDetection);
	}
	

}

public interface IBallSurroundingTarget
{
	Vector3 MiddleBodyPoint { get; }
}
