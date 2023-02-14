using UnityEngine;

namespace Domain
{
    public class GameConfig : MonoBehaviour
    {
        [SerializeField] private GameRemoteConfiguration _instance;

        public GameRemoteConfiguration Instance
        {
            get => _instance;
        }
    }
}