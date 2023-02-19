using System;
using System.Collections;
using System.Collections.Generic;
using AI.Enemies.ImplementingStateReader;
using UnityEngine;
using UnityEngine.UI;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] private Slider lifeSlider;

    private GolemEnemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<GolemEnemy>();

        lifeSlider.maxValue = _enemy.MaxHealth;
        lifeSlider.value = _enemy.MaxHealth;
    }

    public void UpdateLifeView()
    {
        lifeSlider.value = _enemy.CurrentHealth;
    }
}
