using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TestPost : MonoBehaviour
{
    public PostProcessVolume dashPostPro;
    public PostProcessVolume normalPostPro;

    bool change;

    void Start()
    {
        normalPostPro.enabled = true;
        dashPostPro.enabled = false;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.I) && !change)
        {

            dashPostPro.enabled = true;
            normalPostPro.enabled = false;
            change = true;
        }
        else if (Input.GetKeyDown(KeyCode.I) && change)
        {
            dashPostPro.enabled = false;
            normalPostPro.enabled = true;
            change = false;
        }
    }
}
