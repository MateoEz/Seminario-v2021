using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxActivateCanvas : MonoBehaviour
{
    public GameObject textBoxCombo;


    private int life;
    public Collider swordCollider;
    public GameObject boxes;



    private void Start()
    {
        textBoxCombo.SetActive(false);
        life = 60;
    }
    private void OnTriggerStay(Collider other)
    {
        textBoxCombo.SetActive(true);
    }
    private void Update()
    {
        if (life <= 0)
        {
            Destroy(boxes.gameObject);
            Destroy(textBoxCombo.gameObject);
        }
        Debug.Log(life);
    }
    private void OnTriggerExit(Collider other)
    {
        textBoxCombo.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == swordCollider)
        {
            life -= 20;           
        }
    }
    }

