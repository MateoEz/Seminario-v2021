using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneANIMATION : MonoBehaviour
{

    public Light light;

    public float multiply;
    float timer;
    public float timeToChange;
    public float timeToActivateLight;

    public GameObject fadeIn;
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToChange - 1)
        {
            fadeIn.GetComponent<Animator>().SetTrigger("fadein");
            StartCoroutine("ChangeScene");
        }

        if (timer >= timeToActivateLight)

        light.intensity += Time.deltaTime * multiply;



        if (Input.GetKeyDown(KeyCode.F))
        {
            fadeIn.GetComponent<Animator>().SetTrigger("fadein");
            StartCoroutine("ChangeScene");
        }
    }



    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
