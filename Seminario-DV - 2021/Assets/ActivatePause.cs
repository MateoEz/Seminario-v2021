using System.Collections;
using System.Collections.Generic;
using Domain;
using Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ActivatePause : MonoBehaviour
{
    [SerializeField]
    GameObject panelMenu;

    [SerializeField] private GameConfig _gameConfig;


    bool isActive;

    private void Start()
    {
        panelMenu.SetActive(false);
        isActive = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !isActive && !PlayerState.Instance.IsDead)
        {
            _gameConfig.Instance.IsPaused = true;
            panelMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isActive = true;
            //Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isActive)
        {
            _gameConfig.Instance.IsPaused = false;
            panelMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            isActive = false;
            //Time.timeScale = 1;
        }
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
