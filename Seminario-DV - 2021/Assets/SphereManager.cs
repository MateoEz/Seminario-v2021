using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereManager : MonoBehaviour
{

    [SerializeField] List<Transform> spheres = new List<Transform>();
    [SerializeField] float speed;
    PlayerView playerView;
    bool delay;
    private void Start()
    {
        playerView = FindObjectOfType<PlayerView>();
        delay = true;
    }


    private void Update()
    {
        /*if (delay)
        {
            StartCoroutine("DelayToFollow", 1f);
        }*/
    }
    void GoToPlayer()
    {
        foreach (var item in spheres)
        {
            if (item == null) return;
            item.transform.LookAt(playerView.transform.position);
            item.transform.position += ((playerView.transform.position + playerView.transform.up * 1f) - item.transform.position) * speed * Time.deltaTime;
           // StartCoroutine("DestroyRb",0.5f);
        }
    }

    IEnumerator DelayToFollow(int num)
    {
        yield return new WaitForSeconds(num);
        GoToPlayer();
    }

   /* IEnumerator DestroyRb(int num)
    {
        yield return new WaitForSeconds(num);
        foreach (var item in spheres)
        {
            if (item == null) break;
            Destroy(item.GetComponent<Rigidbody>());
        }
    }*/

}
