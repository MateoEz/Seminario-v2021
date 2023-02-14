using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPuzzle : MonoBehaviour
{

    public GameObject imageQER;
    private bool puzzleCompleted;

    private void Update()
    {
        if (puzzleCompleted) imageQER.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerView playerView) || other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {
            imageQER.SetActive(true);
            PuzzleRosca.interactuable = true;
        }

    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerView playerView)|| other.TryGetComponent(out DashPlayerFeedback dashPlayerFeedback))
        {
            PuzzleRosca.interactuable = false;
            imageQER.SetActive(false);    
        }
    }
    public void PuzzleCompleted()
    {
        puzzleCompleted = true;
    }
}
