using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class fpsTest : MonoBehaviour
{

    private TMP_Text _fpsText;

    private void Start()
    {
        _fpsText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        _fpsText.text = (1f / Time.deltaTime).ToString(CultureInfo.InvariantCulture);
    }
}
