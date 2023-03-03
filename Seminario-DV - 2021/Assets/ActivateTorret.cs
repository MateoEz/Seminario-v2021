using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTorret : MonoBehaviour
{
 

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>() || other.gameObject.GetComponent<DashPlayerFeedback>())
        {
            Torreta.activated = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>() || other.gameObject.GetComponent<DashPlayerFeedback>())
        {
            Torreta.activated = false;
        }
    }
}
