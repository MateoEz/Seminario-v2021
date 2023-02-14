using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifebarView : MonoBehaviour
{
    [SerializeField] private float maxHealt;
    [SerializeField] private Image lifeBar;
    private float _currentHealt;



    public void UseLife(float amount)
    {
        _currentHealt -= amount;
        Refresh();
    }

    private float GetEnergyPercentageAvailable()
    {
        return _currentHealt / maxHealt;
    }

    private void Refresh()
    {
        lifeBar.fillAmount = GetEnergyPercentageAvailable();
    }
}
