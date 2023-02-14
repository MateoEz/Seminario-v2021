using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractAbilities : MonoBehaviour
{
	[SerializeField] protected string habilitieName;
	[SerializeField] protected float powerValue;
	[SerializeField] protected bool isHealHabilitie;
	[SerializeField] protected float initialHeight;
	protected float lifetime;

	[SerializeField] protected float cooldown;
	protected float cooldownTimer;


	protected List<Powers.Power> myPowers;
	protected Dictionary<Powers.Power, Action> powersInteractions; 

	virtual protected void Awake()
	{
		lifetime = 60;
		myPowers = new List<Powers.Power>();
		powersInteractions = new Dictionary<Powers.Power, Action>();
		StartCoroutine(AutoDestruction());
	}

	virtual protected void Update()
	{

	}

	public Powers.Power[] GetPowers()
	{
		return myPowers.ToArray();
	}

	protected bool InteractWith(AbstractAbilities h)
	{
		foreach (Powers.Power power in h.GetPowers())
		{
			foreach (Powers.Power myPowerInteraction in powersInteractions.Keys)
			{
				if (myPowerInteraction.Equals(power))
				{
					powersInteractions[myPowerInteraction]();
					return true;
				}
			}
		}
		return false;
	}

	public string GetName()
	{
		return habilitieName;
	}

	public bool IsHabilitieName(string name)
	{
		return this.habilitieName == name;
	}

	public virtual float GetPowerValue()
	{
		return powerValue;
	}

	public virtual void SetPowerValue(float finalPowerValue)
	{
		powerValue = finalPowerValue;
	}

	public bool IsHealHabilitie()
	{
		return isHealHabilitie;
	}

	IEnumerator AutoDestruction()
	{
		yield return new WaitForSeconds(lifetime);
		Destroy(this.gameObject);
		yield return null;
	}

	protected virtual void DestroySpell()
	{
		Destroy(gameObject);
	}

	public abstract void SetInitiation(Vector3 castPos, Vector3 playerPos);
}
