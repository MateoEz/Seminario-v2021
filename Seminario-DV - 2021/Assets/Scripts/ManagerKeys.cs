using System;
using System.Collections;
using System.Collections.Generic;
using AI.Core.StateMachine;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ManagerKeys : MonoBehaviour
{

    public static ManagerKeys instance;
    [SerializeField] GameObject inputParticle;

    [SerializeField] public UnityEvent OnDoorOpen;
    [SerializeField] private GameObject doorCamera;


    [SerializeField] private GameObject ballsIcon;
    public bool playerHasPowerUp;
    

    [FormerlySerializedAs("keysGameObjects")] [SerializeField] public List<string> keysNames = new List<string>();

    
    public int keys;


    [SerializeField] private UnityEvent OnHaveRune1;
    [SerializeField] private UnityEvent OnHaveRune2;
    [SerializeField] private UnityEvent OnHaveRune3;

    [SerializeField] private GameObject panelPowerUp;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetBallsIconStatus(bool status)
    {
        if (ballsIcon == null)
        {
            ballsIcon = GameObject.Find("Canvas").transform.GetChild(1).transform.GetChild(1).gameObject;
        }
        ballsIcon.SetActive(status);
    }

    public void SetPanelPowerUpStatus(bool status)
    {
        panelPowerUp.SetActive(status);
    }

    private void Start()
    {
        if (keys >= 3)
        {
            inputParticle.SetActive(true);
            doorCamera = FindObjectOfType<CameraDoorManager>().gameObject;
        }
    }

    public void UpdateKeys()
    {
        foreach (var item in keysNames)
        {
            var t = GameObject.FindWithTag(item);
            t.GetComponentInChildren<Runa>().ChangeMaterial();

            switch (item)
            {
                case "runa1":
                    OnHaveRune1?.Invoke();
                    break;
                case "runa2":
                    OnHaveRune2?.Invoke();
                    playerHasPowerUp = true;
                    break;
                case "runa3":
                    OnHaveRune3?.Invoke();
                    break;
            }
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            keys += 3;
            doorCamera = FindObjectOfType<CameraDoorManager>().gameObject;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>())
        {
            if (keys < 3) return;
            inputParticle.SetActive(true);
            doorCamera = FindObjectOfType<CameraDoorManager>().gameObject;
            if (Input.GetKey(KeyCode.E))
            {
                doorCamera.GetComponent<CameraDoorManager>().StartAnimation();
                keys = 0;
                OnDoorOpen?.Invoke();
                doorCamera.SetActive(true);

                BaseEnemyWithStateReader[] enemies = FindObjectsOfType<BaseEnemyWithStateReader>();
                foreach (var enemy in enemies)
                {
                    enemy.gameObject.SetActive(false);
                }
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.GetComponent<PlayerView>())
        {
            if (inputParticle)
            {
                inputParticle.SetActive(false);
            }
      
        }

    }
}
