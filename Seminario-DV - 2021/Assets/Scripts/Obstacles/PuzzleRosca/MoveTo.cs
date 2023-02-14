using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTo : MonoBehaviour
{
    [SerializeField] Transform newSpot;
    [SerializeField] float speed;
    [SerializeField] float closeToNewSpot;

    [SerializeField] GameObject particleDoor;

    [HideInInspector]
     public bool goingBack;

    public bool moveColumn;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }
    private void Update()
    {
        if (moveColumn) Move();
        if (goingBack) MoveBack();
    }

    public void SetColumState(bool state)
    {
        moveColumn = state;
    }
    public void Move()
    {
        transform.position -= (transform.position - newSpot.transform.position) * speed * Time.deltaTime;
        if (particleDoor)
        {
            particleDoor.SetActive(true);
        }
        if (Vector3.Distance(transform.position,newSpot.position) <= closeToNewSpot)
        {
            if (particleDoor)
            {
                particleDoor.SetActive(false);
            }
            moveColumn = false;
        }   
    }   

    public void MoveBack()
    {
        transform.position -= (transform.position - startPosition) * (speed)*2 * Time.deltaTime;
        if (Vector3.Distance(transform.position, startPosition) <= closeToNewSpot)
        {
            moveColumn = false;
        }
    }
}
