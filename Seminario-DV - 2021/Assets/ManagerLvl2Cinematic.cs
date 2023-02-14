using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManagerLvl2Cinematic : MonoBehaviour
{

    [SerializeField] GameObject fadeIn;

    public float timer;
    public float timeToChange;
    // Start is called before the first frame update
    void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToChange)
            {
            fadeIn.GetComponent<Animator>().SetTrigger("fadein");

            StartCoroutine("ChangeScene");
            }


        if (Input.GetKeyDown(KeyCode.F))
        {
            fadeIn.GetComponent<Animator>().SetTrigger("fadein");
            StartCoroutine("ChangeScene");
        }
    }




    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
