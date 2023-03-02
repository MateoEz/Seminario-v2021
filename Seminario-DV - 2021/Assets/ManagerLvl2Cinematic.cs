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

    private bool _alreadyPlayed = false;
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= timeToChange && !_alreadyPlayed)
            {
                fadeIn.GetComponent<Animator>().SetTrigger("fadein");
                StartCoroutine("ChangeScene");
                
                AudioMaster.Instance.PlayClip("animationPortal",0.3f);
                _alreadyPlayed = true;
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
