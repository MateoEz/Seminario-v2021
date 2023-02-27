using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{

    
    [SerializeField]
    private GameObject exit;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>() && !other.gameObject.GetComponent<DashPlayerFeedback>())
        {
            other.transform.position = exit.transform.position;
        }
    }
}
