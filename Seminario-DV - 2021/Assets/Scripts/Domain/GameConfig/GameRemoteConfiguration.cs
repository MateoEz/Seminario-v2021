using System;
using UnityEngine;

namespace Domain
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig")]
    public class GameRemoteConfiguration : ScriptableObject
    {
        [Header("Player Values:")] 
        public float DashEnergyCost;
        public float DashSpeed;
        public float PlayerSpeed;
        public float DashEnergyCostBySecond;
        public float SurroundingBallsEnergyCost;
        public float SurroundingBallsCooldown;
        public float StunnedTimeOfMiniGrootSpell;
        public bool IsPaused { get; set; }
    }
}