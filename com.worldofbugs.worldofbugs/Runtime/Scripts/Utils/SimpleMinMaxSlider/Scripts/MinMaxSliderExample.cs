using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GD.MinMaxSlider;

public class MinMaxSliderExample : MonoBehaviour
{

    public string info = "Hover over a slider to see a tooltip";

    [MinMaxSlider(0,10)]
    public Vector2 floatRange = new Vector2(2,8);
    [MinMaxSlider(0,10)]
    public Vector2Int intRange = new Vector2Int(1,5);

}
