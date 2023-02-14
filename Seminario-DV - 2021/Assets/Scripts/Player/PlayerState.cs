using UnityEngine;

namespace Player
{
    public class PlayerState
    {
        private static PlayerState _instance = new PlayerState();

        public static PlayerState Instance
        {
            get
            {
                return _instance;
            }
        }

        public static void Clean()
        {
            _instance = new PlayerState();
        }

        public bool IsGrounded { get; set; }

        public bool IsAttacking { get; set; }

        public bool IsBlinking { get; set; }

        public Transform Transform { get; set; }

        public bool CanAffordBlink { get; set; }

        public bool IsRecoveringFromKnock { get; set; }

        public bool IsDashing { get; set; }
        public bool IsStunned { get; set; }
        public bool IsDead { get; set; }
        public bool IsKnocked { get; set; }
    }
}