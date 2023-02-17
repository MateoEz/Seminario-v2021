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
    public bool needToReload;
    private bool imReloading;
    private float reloadTime;

    public void ReloadSurroundingBalls()
    {
        reloadTime = _gameConfig.Instance.SurroundingBallsCooldown;

        if (imReloading) return;
        RBallsActive.fillAmount = 1;
        StartCoroutine("FillAmountCorutine");
    }

    IEnumerable FillAmountCorutine()
    {
        imReloading = true;
        RBallsActive.fillAmount -= Time.deltaTime/100;
        yield return new WaitForSeconds(reloadTime);
        imReloading = false;
    }
}
