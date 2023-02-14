using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBehaviour : MonoBehaviour
{
    private bool _activated;


    private void Start()
    {
        if (!_activated)
        {
            StartCoroutine(DeactivateCoroutine(3));
        }
    }

    IEnumerator DeactivateCoroutine(float t)
    {
        yield return new WaitForSeconds(t);
        //gameObject.SetActive(false);
        _activated = true;
    }
    
}
