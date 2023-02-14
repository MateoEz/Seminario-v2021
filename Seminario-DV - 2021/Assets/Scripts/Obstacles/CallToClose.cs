using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallToClose : MonoBehaviour
{
    [SerializeField] MoveTo door;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>() || other.gameObject.GetComponent<DashPlayerFeedback>())
        {
            door.GetComponent<MoveTo>().moveColumn = false;

            door.GetComponent<MoveTo>().goingBack = true;
        }
    }
}
