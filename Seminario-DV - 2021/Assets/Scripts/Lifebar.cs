using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lifebar : MonoBehaviour
{
    public Slider slider;
    public void SetMaxHealt(int healt)
    {
        slider.maxValue = healt;
        slider.value = healt;
    }

    public void SetHealt(int healt)
    {
        slider.value = healt;
    }
}
