using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : MonoBehaviour
{


    [SerializeField]
    PlayerView playerView;

    [Header("Targeting feedback")]
    [SerializeField] private Material normalMaterial;
    [SerializeField] private Material targetingMaterial;


    void Update()
    {
        if(playerView.CurrentTarget.GetTransform().gameObject == this.gameObject)
        {
            ShowFeedback();
        }
        else
        {
            HideFeedback();
        }
    }


    void ShowFeedback()
    {
        if (this == null) return;
        var currentMat = transform.GetComponent<MeshRenderer>();
        currentMat.material = targetingMaterial;
    }

    void HideFeedback()
    {
        if (this == null) return;
        var currentMat = transform.GetComponent<MeshRenderer>();
        currentMat.material = targetingMaterial;
    }
}
