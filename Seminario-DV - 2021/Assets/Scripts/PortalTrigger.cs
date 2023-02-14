using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTrigger : MonoBehaviour
{



    private GameObject player;

    [SerializeField]
    private GameObject exit;

    void Teleport()
    {
        player.transform.position = exit.transform.position;
        player.transform.rotation = exit.transform.rotation;
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>())
        {
            player = other.gameObject;
            Teleport();

        }
    }
}
