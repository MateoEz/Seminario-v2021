using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Answer : MonoBehaviour
{
    [SerializeField] float distanceLimit;
    [SerializeField] Collider colliderAnswer;
    [SerializeField] bool correctAnswer;
    public Ray myRay;

    void Update()
    {
       RaycastHit hit;       
        if (Physics.Raycast(transform.position,transform.forward,out hit, distanceLimit))
        {
            if (hit.transform.GetComponent<PuzzleRoscaManager>() != null)
            {
                correctAnswer = true;
            }
            else correctAnswer = false;        
        }       
    }

    public bool IsAnswerCorrect()
    {
        return correctAnswer;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawRay(transform.position, transform.forward);
    }

}
