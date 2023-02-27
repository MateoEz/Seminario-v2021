using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRosca : MonoBehaviour
{
    public static bool interactuable;
    private bool imRotating;
    private bool justPressed;
    private bool puzzleCompleted;
    private bool checkForColor;
    private bool shouldCheckColor;
    private float timeToCheck;
    private Quaternion qTo;
    [SerializeField]
    private float speed;
    [SerializeField]
    private KeyCode button;
    public List<Collider> simbolos;
    public void Start()
    {
       
    }
    void Update()
    {      
            if (Input.GetKeyDown(button) && interactuable)
            {
            if(!puzzleCompleted) justPressed = true;
            }
            if (justPressed && !imRotating) Rotate();
        if (checkForColor)
        {
        timeToCheck += Time.deltaTime;
            if(timeToCheck >= 1.6f)
            {
                checkForColor = false;
                shouldCheckColor = true;
            }
        }
    }
    public void PuzzleCompleted()
    {
        puzzleCompleted = true;
    }
    [SerializeField] AudioClip slidePuzzleRock;
    [SerializeField] AudioSource puzzleAudioSource;
    public void Rotate()
    {
        shouldCheckColor = false;
        timeToCheck = 0;
        checkForColor = true;
        imRotating = true;
        qTo = transform.rotation;
        qTo = Quaternion.AngleAxis(-60, transform.forward) * transform.rotation;
        if (slidePuzzleRock)
        {
            puzzleAudioSource.PlayOneShot(slidePuzzleRock,.8f);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, qTo, speed);
        if (transform.rotation == qTo) imRotating = false;
        justPressed = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 16) interactuable = true;
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 16) interactuable = false;
    }

}
