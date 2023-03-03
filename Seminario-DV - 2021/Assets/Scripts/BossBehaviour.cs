using System;
using System.Collections;
using System.Collections.Generic;
using AI.Enemies.ImplementingStateReader;
using UnityEngine;
using UnityEngine.UI;

public class BossBehaviour : MonoBehaviour
{
    [SerializeField] private Slider lifeSlider;
    [SerializeField] bool needLifeBar;

    private GolemEnemy _enemy;

    private void Start()
    {
        _enemy = GetComponent<GolemEnemy>();

        lifeSlider.maxValue = _enemy.MaxHealth;
        lifeSlider.value = _enemy.MaxHealth;
        Debug.Log(_enemy.CurrentHealth);
    }

    public void UpdateLifeView()
    {
        Debug.Log("Update");
        if(needLifeBar)
            lifeSlider.value = _enemy.CurrentHealth;
    }

    public void Scream()
    {
        AudioMaster.Instance.PlayClip("BossScream", 0.8f, .8f);
    }

    public void RocksFalling()
    {
        AudioMaster.Instance.PlayClip("BossAnimSounds", 0.8f, .8f);
    }
}
