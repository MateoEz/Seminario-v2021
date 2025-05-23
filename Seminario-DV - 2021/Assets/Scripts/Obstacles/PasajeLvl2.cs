﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
    public class PasajeLvl2 : MonoBehaviour
{


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
        if (other.gameObject.layer == 16)
        {
            AudioMaster.Instance.PlayClip("animationPortal",0.3f);
            fadeIn.GetComponent<Animator>().SetTrigger("fadein");
            FindObjectOfType<MainSongFade>().FadeAudio(true);
            FindObjectOfType<CameraView>().gameObject.AddComponent<AudioListener>();
            other.gameObject.SetActive(false);
            StartCoroutine("ChangeScene");
        }
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
