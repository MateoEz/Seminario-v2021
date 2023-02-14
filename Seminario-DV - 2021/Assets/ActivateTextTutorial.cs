using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTextTutorial : MonoBehaviour
{


    [SerializeField] GameObject text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>())
        {

            text.gameObject.SetActive(true);
        }
    }
}
