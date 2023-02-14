using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvas : MonoBehaviour
{
    [SerializeField] private Button optionsButton;
    [SerializeField] private SettingsMenu settingsMenu;
    void Start()
    {
        settingsMenu.Init();
        optionsButton.onClick.AddListener(() =>
        {
            settingsMenu.gameObject.SetActive(!settingsMenu.gameObject.activeSelf);
        });
    }
}
