using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class UI_playerExp : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI levelText;

    IEnumerator enumatorProcesss;

    private void Awake()
    {
        slider.value = 0;
        slider.maxValue = 20;
    }
    public void UpdateSliderUI(float value)
    {
        if (Mathf.Abs(slider.value - value) <= 1)
        {
            slider.value = Mathf.Clamp(value, 0, slider.maxValue);
        }
        else
        {
            slider.value = Mathf.Lerp(slider.value, value, Time.deltaTime * 4f);
        }
    }

   


    public float GetSliderValue() => slider.value;
    public Slider GetSlider() => slider;

    public void SetUpMaxValue(float _maxValue)
    {
        slider.maxValue = _maxValue;
        slider.value = 0;
    }

    public void UpdateLevelText(int _level)
    {
        levelText.text = _level.ToString();
    }

}
