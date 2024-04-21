using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SliderWithValue : MonoBehaviour
{
    Slider slider;
    [SerializeField] TextMeshProUGUI textValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.onValueChanged.AddListener(ChangeText);
    }

    private void ChangeText(float value)
    {
        textValue.SetText(value.ToString("0.0"));
    }

}
