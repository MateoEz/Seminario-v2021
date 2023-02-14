namespace AI.Core.StateMachine
{
    public class MyStateReader
    {
        private MyState _defaultState;
        private MyState _currentState;

        public MyStateReader(MyState defaultState)
        {
            _defaultState = defaultState;
        }

        public void Update()
        {
            if (_currentState == null)
            {
                _defaultState.Execute();
                return;
            }

            _currentState.Execute();
        }

        public void SetState(MyState nextState)
        {
            if (_currentState == null)
            {
                PulseState(nextState);
                return;
            }
            if (!_currentState.IsBlocked)
            {
                if (!_currentState.CanChangeState()) return;
            }
            if (_currentState != nextState)
            {
                if (_currentState.IsBlocked)
                    _currentState.OnForceQuit();
                _currentState.Sleep();
                PulseState(nextState);
            }
        }

        private void PulseState(MyState nextState)
        {
            _currentState = nextState;
            _currentState.Awake();
        }

        public void SetDefault()
        {
            SetState(_defaultState);
        }

        public MyState CurrentState()
        {
            return _currentState != null ? _currentState : _defaultState;
        }
    }
}