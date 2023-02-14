using System;
using System.Collections.Generic;
using System.Linq;
using AI.Core.StateMachine;
using AI.Enemies.FSM_States;
using Domain;
using Domain.Services;
using Player;
using UnityEngine;

public class SquadManager : MonoBehaviour
{
    [SerializeField] private List<BaseEnemyWithStateReader> _squadMembers;
    [SerializeField] private List<SquadMision> _squadMisions;
    private bool _forceUnlock;
    private AmbushService _ambushService;
    private List<IAmbusher> _ambushers;

    private void Awake()
    {
        _squadMisions = _squadMisions.OrderByDescending(mision => mision.priority).ToList();
        _ambushers = new List<IAmbusher>();
        SetMembers();
        SetAmbushers();
        _ambushService = new AmbushService();
    }

    private void SetMembers()
    {
        _squadMembers.ForEach(member =>
        {
            if (!member.IsDead && (member as ISquadMember) != null)
            {
                var squadMember = (ISquadMember) member;
                squadMember.Init(this);
            }
        });
    }

    private void Update()
    {
        _forceUnlock = false;
        _squadMembers = _squadMembers.Where(member => member != null &&  member.gameObject.activeSelf).ToList();
        // recorrer las misiones en orden de prioridad
        foreach (var misions in _squadMisions)
        {
            //Si solo queda 1, liberarle todos los estados. Excepto los que no puede hacer solo (ya esta contemplado en la condicion de arriba)
            if (_squadMembers.Count <= 1)
            {
                foreach (var member in _squadMembers)
                {
                    misions.misionTypes.ForEach(mision => member.UnlockMisionType(mision));
                }
            }

            //Si no llegan al minimo de la mision bloquear y seguir.
            if (_squadMembers.Count < misions.minimunMembersToMision)
            {
                foreach (var member in _squadMembers)
                {
                    misions.misionTypes.ForEach(mision => member.BlockMisionType(mision));
                }
                continue;
            }

            // ver si los squadsMembers cumplen.
            List<BaseEnemyWithStateReader> membersOnMision =
                _squadMembers.Where(member => misions.misionTypes.Contains(member.CurrentMision)).ToList();
            // si cumplen, bloquearles todos los estados que satisfagan dicha mision a los demás miembros del squad.
            if (membersOnMision.Count >= misions.membersAssigned && !_forceUnlock)
            {
                var membersForDuty = membersOnMision.Take(misions.membersAssigned).ToList();
                var membersToBlockMision = _squadMembers.Where(member => !membersForDuty.Contains(member)).ToList();
                foreach (var member in membersToBlockMision)
                {
                    misions.misionTypes.ForEach(misio => member.BlockMisionType(misio));
                }
            }
            // si no cumplen, desbloquearle los estados que cumplen con dicha mision.
            else
            {
                foreach (var member in _squadMembers)
                {
                    misions.misionTypes.ForEach(mision => member.UnlockMisionType(mision));
                }

                _forceUnlock = true;
            }
        }
    }

    public Vector3 GetAmbushPosition(IAmbusher ambusher)
    {
        ResetAmbushers();
        _ambushService.Init(_ambushers, PlayerState.Instance.Transform);
        return _ambushService.AmbushPositionOf(ambusher);
    }

    private void SetAmbushers()
    {
        _squadMembers.ForEach(member =>
        {
            if (!member.IsDead && (member as IAmbusher) != null)
            {
                var ambusher = (IAmbusher) member;
                _ambushers.Add(ambusher);
            }
        });
    }

    private void ResetAmbushers()
    {
        _ambushers.Clear();
        SetAmbushers();
    }

    public void NotifySquadMembers(string key, object value)
    {
        foreach (var member in _squadMembers)
        {
            member.SetWorldState(key, value);
        }
    }

    public List<BaseEnemyWithStateReader> GetActiveSquadMembers()
    {
        return _squadMembers.Where(member => member != null &&  member.gameObject.activeSelf).ToList();
    }
    
    public List<BaseEnemyWithStateReader> GetActiveSquadMembers<T>()
    {
        return _squadMembers.Where(member => member != null &&  member.gameObject.activeSelf && member is T && !member.IsDead).ToList();
    }
    public List<BaseEnemyWithStateReader> GetActiveSquadMembersOfType(EnemyType type)
    {
        return _squadMembers.Where(member => member != null &&  member.gameObject.activeSelf && member.GetType().Equals(type)).ToList();
    }
}