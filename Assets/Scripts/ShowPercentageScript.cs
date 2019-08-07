using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// script attached to volume sliders to show percentage values
public class ShowPercentageScript : MonoBehaviour
{

    // percentage text
    private Text percentageText;

    // Start is called before the first frame update
    void Start()
    {
        percentageText = gameObject.GetComponent<Text>();
    }

    // updates text value from slider input
    public void TextUpdate(float value)
    {
        percentageText.text = Mathf.RoundToInt(value * 100).ToString() + "%";
    }
}
