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
            other.gameObject.SetActive(false);
            StartCoroutine(MovePlayerCoroutine(other.gameObject));
        }
    }

    private IEnumerator MovePlayerCoroutine(GameObject player)
    {
        yield return new WaitForSeconds(0.5f);
        player.transform.position = exit.transform.position;
        yield return new WaitForSeconds(0.5f);
        player.SetActive(true);
    }
}
