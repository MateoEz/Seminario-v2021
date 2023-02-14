using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    [SerializeField] GameObject trailKey;
    [SerializeField] GameObject inputParticle;



    bool playerCanTake;
    void Start()
    {
        trailKey.SetActive(false);
        inputParticle.SetActive(false);
    }

    void Update()
    {
        PlayerGetKey();   
    }


    void PlayerGetKey()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerCanTake)
        {
            trailKey.SetActive(true);
            inputParticle.SetActive(false);
            if (transform.parent.GetComponent<GetPowerUp>())
            {
                transform.parent.GetComponent<GetPowerUp>().Show();
                ManagerKeys.instance.playerHasPowerUp = true;
            }
            Destroy(this.gameObject);
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>())
        {
            playerCanTake = true;
            if (playerCanTake)
            {
                inputParticle.SetActive(true);
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>())
        {
            playerCanTake = false;
            inputParticle.SetActive(false);
        }

    }
}
