using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Continuara : MonoBehaviour
{


    public GameObject tobecontinue;
    public GameObject fadeIn;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>() || other.gameObject.GetComponent<DashPlayerFeedback>())
        {

            FindObjectOfType<MainSongFade>().FadeAudio(true);
            FindObjectOfType<CameraView>().gameObject.AddComponent<AudioListener>();
            AudioMaster.Instance.PlayClip("animationPortal",0.3f);
            fadeIn.GetComponent<Animator>().SetTrigger("fadein");
            tobecontinue.gameObject.SetActive(true);
            other.gameObject.SetActive(false);
            StartCoroutine("ChangeScene");
        }

    }


    IEnumerator ChangeScene()
    {
        
        yield return new WaitForSeconds(4f);
        PlayerPrefs.DeleteKey("boss_checkpoint");
        Destroy(FindObjectOfType<ManagerKeys>().gameObject);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}
