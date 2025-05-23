﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{

    [SerializeField] private AudioMixer audioMixer;
    [FormerlySerializedAs("dropdown")] [SerializeField] private Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;
    [SerializeField] private Dropdown resolutionDropdown;
    [SerializeField] private Button backButton;
    [SerializeField] private Toggle vSyncToggle;

    private Resolution[] _resolutions;
    public void Init()
    {
        backButton.onClick.AddListener(() =>
        {
            if (SceneManager.GetActiveScene().name != "Menu")
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            gameObject.SetActive(false);
        });
        
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(QualitySettings.names.ToList());
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        

        fullScreenToggle.isOn = Screen.fullScreen;


        _resolutions = Screen.resolutions;
        
        
        resolutionDropdown.ClearOptions();
        
        var options = new List<string>();
        int currentResolutionIndex = 0;
        for (var index = 0; index < _resolutions.Length; index++)
        {
            var res = _resolutions[index];
            string option = res.width + " x " + res.height;
            options.Add(option);

            if (res.width == Screen.currentResolution.width && res.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = index;
            }
        }
        
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();


        vSyncToggle.isOn = QualitySettings.vSyncCount == 1;
        vSyncToggle.onValueChanged.AddListener((status) =>
        { 
            QualitySettings.vSyncCount = status ? 1 : 0;
            vSyncToggle.SetIsOnWithoutNotify(status);
            Debug.Log(QualitySettings.vSyncCount);
        });
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume",volume);
    }
    
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel((index));
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width,resolution.height,Screen.fullScreen);
    }
}
