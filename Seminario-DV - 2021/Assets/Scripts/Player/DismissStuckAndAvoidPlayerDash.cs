using System;
using MyUtilities;
using UnityEngine;

namespace Player
{
    public class DismissStuckAndAvoidPlayerDash : MonoBehaviour, IIgnoreDash
    {
        [SerializeField] private float forceAmount;
        [SerializeField] private Rigidbody _rigidbody;

        private void OnTriggerEnter(Collider other)
        {
            var playerDash = other.GetComponent<DashPlayerFeedback>();
            if (playerDash != null)
            {
                _rigidbody.AddForce(Utils.GetDirIgnoringHeight(playerDash.transform.position, transform.position) * forceAmount);
            }
        }
    }

    public interface IIgnoreDash
    {
        
    }
}