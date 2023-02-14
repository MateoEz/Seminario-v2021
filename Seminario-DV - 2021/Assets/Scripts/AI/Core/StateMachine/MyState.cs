using System.Linq;

namespace AI.Core.StateMachine
{
    public abstract class MyState
    {
        protected EntityState _preConditions;
        private readonly int _priority;
        protected readonly MisionType _misionType;
        protected bool _isBlocked;

        public MyState(EntityState preConditions, int priority = 0)
        {
            _preConditions = preConditions;
            _priority = priority;
            _misionType = MisionType.None;
        }
        public MyState(EntityState preConditions, MisionType missionType, int priority = 0)
        {
            _preConditions = preConditions;
            _priority = priority;
            _misionType = missionType;
        }

        public MisionType MissionType => _misionType;
        public bool IsBlocked => _isBlocked;

        public int GetPrority()
        {
            return _priority;
        }

        public EntityState GetPreConditions()
        {
            return _preConditions;
        }
        
        public virtual void Awake() { }
        public virtual void Sleep() { } 
        public abstract void Execute();
        public virtual void LateExecute() { }
        public abstract bool CanChangeState();

        public void Block()
        {
            _isBlocked = true;
        }

        public void Unlock()
        {
            _isBlocked = false;
        }

        public virtual void OnForceQuit()
        {
            
        }
    }
}
