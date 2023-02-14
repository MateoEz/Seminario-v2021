using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorText : MonoBehaviour
{

    public Button textButton;
    private Color initialColor;
    public Color highligtedColor;
    // Start is called before the first frame update
    void Start()
    {
        initialColor = GetComponent<Text>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }




}
