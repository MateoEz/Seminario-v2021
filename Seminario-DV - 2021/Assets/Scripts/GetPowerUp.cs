using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GetPowerUp : MonoBehaviour
{
    void Awake()
    {
        if (ManagerKeys.instance.playerHasPowerUp)
        {
            ManagerKeys.instance.SetBallsIconStatus(true);
        }

    }

    private GameObject _canvas;
    public void Show()
    {
        ManagerKeys.instance.SetBallsIconStatus(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        ManagerKeys.instance.SetPanelPowerUpStatus(true);
        _canvas = GameObject.Find("Canvas");
        _canvas.SetActive(false);
        
        Time.timeScale = 0;
    }
    public void ResetTimeScaleAndDeactivateCanvas()
    {
        _canvas.SetActive(true);
        ManagerKeys.instance.SetPanelPowerUpStatus(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }
    
    
}
