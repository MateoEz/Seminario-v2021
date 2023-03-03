using System;
using System.Collections;
using System.Collections.Generic;
using AI.Enemies.ImplementingStateReader;
using UnityEngine;
using UnityEngine.Events;

public class BossFightBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] blockWalls;
    [SerializeField] private GameObject bossLifeContainer;
    [SerializeField] private CameraView cameraView;
    [SerializeField] private GameObject bossCamera;
    [SerializeField] private GameObject bossGolemGameObject;
    [SerializeField] private GameObject fakeBoss;
    [SerializeField] private GameObject playerView;
    [SerializeField] private MeshCollider throneCollider;
    [SerializeField] private UnityEvent onAnimStarted;
    [SerializeField] private UnityEvent onAnimEnded;
    [SerializeField] private GameObject bossStartPos;
    [SerializeField] private FinalPuzzleManager puzzleManager;
    private bool _currentStatus;
    private float _tick;
    private static readonly int BossGameOn = Animator.StringToHash("BossGameOn");
    private const string CHECKPOINT_KEY = "boss_checkpoint";

    private void Update()
    {
        if (_currentStatus)
        {
            _tick += Time.deltaTime;
            if (_tick > 0.7f)
            {
                cameraView.LightShake();
                _tick = 0;
            }
        }
    }

    public void SetFightStatus(bool status)
    {
        foreach (var block in blockWalls)
        {
            block.SetActive(status);
            bossLifeContainer.SetActive(status);
        }

        _currentStatus = status;
        if (_currentStatus == false)
        {
            puzzleManager.on = true;
            var mainSongFade = FindObjectOfType<MainSongFade>();
            mainSongFade.BossFighting = false;
            mainSongFade.SetFightStatus(false);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>())
        {
            if (!PlayerPrefs.HasKey(CHECKPOINT_KEY))
            {
                PlayerPrefs.SetInt(CHECKPOINT_KEY,1);
            }
            
            StartCoroutine(StartAnimationCoroutine());

            GetComponent<BoxCollider>().enabled = false;
        }
    }

    private IEnumerator StartAnimationCoroutine()
    {
        var mainSongFade = FindObjectOfType<MainSongFade>();

        onAnimStarted?.Invoke();
        bossCamera.SetActive(true);
        fakeBoss.GetComponent<Animator>().SetTrigger("AnimationBoss");
        playerView.gameObject.SetActive(false);
        mainSongFade.SetFightStatus(true);
        mainSongFade.BossFighting = true;
        yield return new WaitForSeconds(1);
        //bossGolemGameObject.transform.position = bossStartPos.transform.position;
        //bossGolemGameObject.GetComponent<Animator>().SetTrigger(BossGameOn);
        //bossGolemGameObject.GetComponent<Animator>().applyRootMotion = true;
        yield return new WaitForSeconds(5);
        bossCamera.SetActive(false);
        playerView.gameObject.SetActive(true);
        playerView.gameObject.GetComponent<PlayerView>().DashPlayerFeedback.DisablePlayerFeedback();
        onAnimEnded?.Invoke();
        SetFightStatus(true);
        yield return new WaitForSeconds(1);
        fakeBoss.SetActive(false);
        bossGolemGameObject.SetActive(true);
        bossGolemGameObject.GetComponent<GolemEnemy>().enabled = true;
        yield return new WaitForSeconds(3);
        throneCollider.enabled = true;
    }
}
