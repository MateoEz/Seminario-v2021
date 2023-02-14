using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalDashTrigger : MonoBehaviour
{
    [SerializeField] PortalDashOpen portal;

    bool playerEntered;

    private void Update()
    {
        CheckForActivation();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 16) playerEntered = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.layer == 16) playerEntered = false;
    }
    public void CheckForActivation()
    {
        if(playerEntered && Input.GetKeyDown(KeyCode.E))
        {
            portal.ChangeTheLock();
        }
    }
}
