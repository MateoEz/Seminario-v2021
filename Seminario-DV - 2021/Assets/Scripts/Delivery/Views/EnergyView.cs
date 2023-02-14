using System;
using Domain;
using Domain.Services;
using UnityEngine;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class EnergyView : MonoBehaviour, IEnergyView
    {
        [SerializeField] private float _maxEnergy;
        [SerializeField] private Image _energyBar;
        
        private float _currentEnergy;

        private void Update()
        {
            _currentEnergy += Time.deltaTime;
            _currentEnergy = Mathf.Clamp(_currentEnergy, 0, _maxEnergy);
            Refresh();
        }

        public void UseEnergy(float amount)
        {
            _currentEnergy -= amount;
            Refresh();
        }

        public bool IsAffordable(float amount)
        {
            return amount <= _currentEnergy;
        }

        private float GetEnergyPercentageAvailable()
        {
            return _currentEnergy / _maxEnergy;
        }

        private void Refresh()
        {
            _energyBar.fillAmount = GetEnergyPercentageAvailable();
        }
    }
}