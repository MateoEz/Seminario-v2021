using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehaviour : MonoBehaviour
{
    [SerializeField] private Vector3 initialPosition;
    private const string CHECKPOINT_KEY = "boss_checkpoint";
    void Start()
    {
        if (PlayerPrefs.HasKey(CHECKPOINT_KEY))
        {
            transform.position = initialPosition;
        }
    }
    
}
