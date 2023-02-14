using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continuara : MonoBehaviour
{


    public GameObject tobecontinue;
    public GameObject fadeIn;
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
        if (other.gameObject.GetComponent<PlayerView>() || other.gameObject.GetComponent<DashPlayerFeedback>())
        {

            fadeIn.GetComponent<Animator>().SetTrigger("fadein");
            tobecontinue.gameObject.SetActive(true);
            StartCoroutine("ChangeScene");
        }

    }


    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(4f);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}
