using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class CollectibleHandler : MonoBehaviour
{
    [SerializeField] private GameObject collectiblesContainer;
    [SerializeField] private TMP_Text collectiblesText;

    private int _currentIndex = 0;

    private int _collectiblesTotal;
    private void Start()
    {
        _collectiblesTotal = FindObjectsOfType<CollectibleBehaviour>().Length;
    }

    public void OnGrabbed()
    {
        _currentIndex++;
        UpdateText();
        Show();
    }

    private void UpdateText()
    {
        collectiblesText.SetText($"{_currentIndex}/{_collectiblesTotal}");
    }

    private void Show()
    {
        collectiblesContainer.SetActive(true);
    }
}
