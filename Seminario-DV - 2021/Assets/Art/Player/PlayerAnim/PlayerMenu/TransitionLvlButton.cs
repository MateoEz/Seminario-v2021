using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionLvlButton : MonoBehaviour
{


    public GameObject fadeImage;
    public GameObject buttonOptions;
    public void TransitionLevelButton()
    {

        fadeImage.GetComponent<Animator>().SetTrigger("fadein");
        GetComponent<Animator>().SetTrigger("play");
        buttonOptions.GetComponent<Animator>().SetTrigger("play");
        StartCoroutine("ChangeScene");
    }


    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(6f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
