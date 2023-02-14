using UnityEngine;

namespace AnimatorStateMachine.AnimatorStates.ActionsScripts
{
    [CreateAssetMenu(fileName = "NewInstantiatePrefabState", menuName = "AnimatorStates/InstantiatePrefabState")]
    public class InstantiatePrefab : AnimatorStateData
    {
        [SerializeField] private string _resourceName;
        private GameObject _resource;
        private GameObject _currentInstance;
        public override AnimatorStateData Clone()
        {
            var instance = CreateInstance<InstantiatePrefab>();
            instance._resourceName = _resourceName;
            return instance;
        }
        
        public override void StartState(Animator animator, AnimatorStateInfo stateInfo)
        { 
            _resource = Resources.Load<GameObject>("Enemies/MiniGroot/" + _resourceName);
            _currentInstance = Instantiate(_resource, animator.transform);
        }

        public override void FinishState(Animator animator, AnimatorStateInfo stateInfo)
        {
            Destroy(_currentInstance);
        }
    }
}