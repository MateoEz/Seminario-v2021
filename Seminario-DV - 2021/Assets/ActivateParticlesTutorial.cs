using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateParticlesTutorial : MonoBehaviour
{


    GameObject particleChildren;
    bool hide;

    void Start()
    {
        particleChildren = transform.GetChild(0).gameObject;
        hide = false ;
    }


    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (particleChildren && !hide && other.gameObject.GetComponent<PlayerView>())
        {
            particleChildren.SetActive(true);
            //hide = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (particleChildren && other.gameObject.GetComponent<PlayerView>())
        {
            particleChildren.SetActive(false);
        }
    }
}
