using System.Collections;
using System.Collections.Generic;
using Player;
using Domain;
using UnityEngine.UI;
using UnityEngine;

public class PowerUpsFeedBack : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;

    [SerializeField] Image dashActive;
    [SerializeField] Image dashDefault;
    [SerializeField] Image RBallsActive;

    void Start()
    {
        ResetDashFeedback();
    }

    void Update()
    {
        if (PlayerState.Instance.IsDashing)
        {
            DashVisualFeedback();
        }
        else ResetDashFeedback();

    }
    void DashVisualFeedback()
    {
        dashActive.enabled = true;
        dashDefault.enabled = false;
    }

    private void ResetDashFeedback()
    {
        dashDefault.enabled = true;
        dashActive.enabled = false;
    }

    void SurroundingBallsFeedback()
    {    
      //_gameConfig.Instance.SurroundingBallsCooldown
    }
}
