using System;
using System.Collections;
using System.Collections.Generic;
using AI.Enemies.ImplementingStateReader;
using UnityEngine;

public class BossFightBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject[] blockWalls;
    [SerializeField] private GameObject bossLifeContainer;
    [SerializeField] private CameraView cameraView;
    [SerializeField] private GameObject bossCamera;
    [SerializeField] private GameObject bossGolemGameObject;
    [SerializeField] private GameObject playerView;
    [SerializeField] private MeshCollider throneCollider;
    [SerializeField] private GameObject myCanvas;
    [SerializeField] private GameObject bossStartPos;
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
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>())
        {
            SetFightStatus(true);
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

        myCanvas.SetActive(false);
        bossCamera.SetActive(true);
        playerView.gameObject.SetActive(false);
        mainSongFade.SetFightStatus(true);
        mainSongFade.BossFighting = true;
        yield return new WaitForSeconds(1);
        bossGolemGameObject.transform.position = bossStartPos.transform.position;
        bossGolemGameObject.GetComponent<Animator>().SetTrigger(BossGameOn);
        bossGolemGameObject.GetComponent<Animator>().ApplyBuiltinRootMotion();
        yield return new WaitForSeconds(5);
        playerView.gameObject.SetActive(true);
        playerView.gameObject.GetComponent<PlayerView>().DashPlayerFeedback.DisablePlayerFeedback();
        bossCamera.SetActive(false);
        myCanvas.SetActive(true);
        yield return new WaitForSeconds(1);
        bossGolemGameObject.GetComponent<GolemEnemy>().enabled = true;
        yield return new WaitForSeconds(3);
        throneCollider.enabled = true;
    }
}
