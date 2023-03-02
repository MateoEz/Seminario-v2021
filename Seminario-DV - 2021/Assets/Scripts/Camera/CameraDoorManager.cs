using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CameraDoorManager : MonoBehaviour
{

    [SerializeField] private PlayerView _playerView;
    [SerializeField] private Animator newAnimator;

    public UnityEvent OnDoorOpened;

    private bool goForward = false;
    private bool playerGoForward = false;
    

    private void Update()
    {
        if (goForward)
        {
            transform.position += transform.forward * 3.5f * Time.deltaTime;
        }

        if (playerGoForward)
        {
            _playerView.gameObject.transform.position += _playerView.gameObject.transform.forward * 2 * Time.deltaTime;
        }
    }

    public void StartAnimation()
    {
        _playerView.gameObject.GetComponent<Animator>().runtimeAnimatorController = GetComponent<Animator>().runtimeAnimatorController;
        _playerView.SetFootsteps(false);
        _playerView.enabled = false;
        GetComponent<Camera>().enabled = true;
        OnDoorOpened.Invoke();
        StartCoroutine(ChangeAnimationPlayer());
    }


    IEnumerator ChangeAnimationPlayer()
    {
        yield return new WaitForSeconds(9);
        _playerView.gameObject.GetComponent<Animator>().SetTrigger("JumpTrigger");
        playerGoForward = true;
        yield return new WaitForSeconds(3);
        goForward = true;
        GameObject.Find("FadeOut").GetComponent<Animator>().SetTrigger("fadein");
        yield return new WaitForSeconds(5);
        foreach (Transform child in ManagerKeys.instance.transform)
        {
            Destroy(child.gameObject);
        }
        SceneManager.LoadScene("Level3");

    }
}
