using System;
using System.Collections.Generic;
using AI.Enemies.BaseScripts;
using UnityEngine;

namespace AI.Core.StateMachine
{
    public class StateMachineDaVinci {

        MyState _currentState;
        List<MyState> _states = new List<MyState>();
        private GoapEnemy _goapEnemy;
        private Action<Animator> _action;

        public StateMachineDaVinci(GoapEnemy owner)
        {
            _goapEnemy = owner;
            _action = (animator) => animator.SetBool("IsMoving", false);
        }

        public GoapEnemy GetOwner()
        {
            return _goapEnemy;
        }

        /// <summary>
        /// Llama al execute del estado actual.
        /// </summary>
        public void Update()
        {
            if (_currentState != null)
                _currentState.Execute();
        }

        /// <summary>
        /// Llama al LateExecute del estado actual.
        /// </summary>
        public void LateUpdate()
        {
            if (_currentState != null)
                _currentState.LateExecute();
        }

        /// <summary>
        /// Agrega un estado.
        /// </summary>
        /// <param name="s">El estado a agregar.</param>
        public void AddState(MyState state)
        {
            _states.Add(state);
            if (_currentState == null)
                _currentState = state;
        }

        /// <summary>
        /// Cambia de estado.
        /// </summary>
        public void SetState<T>() where T : MyState
        {
            for (int i = 0; i < _states.Count; i++)
            {
                if (_states[i].GetType() == typeof(T))
                {
                    _currentState.Sleep();
                    _currentState = _states[i];
                    _currentState.Awake();
                }
            }
        }

        public bool IsActualState<T>() where T : MyState
        {
            return _currentState.GetType() == typeof(T);
        }

        /// <summary>
        /// Busca el índice de un estado por su tipo.
        /// </summary>
        /// <param name="t">Tipo de estado.</param>
        /// <returns>Devuelve el índice.</returns>
        private int SearchState(Type t)
        {
            int ammountOfStates = _states.Count;
            for (int i = 0; i < ammountOfStates; i++)
                if (_states[i].GetType() == t)
                    return i;
            return -1;
        }

        public bool HasState<T>() where T : MyState
        {
            for (int i = 0; i < _states.Count; i++)
            {
                if (_states[i].GetType() == typeof(T))
                {
                    return true;
                }
            }

            return false;
        }

        public Action<Animator> GetGlobalAction()
        {
            return _action;
        }
        
        public void SetGlobalAction(Action<Animator> action)
        {
            _action = action;
        }
    }
}
