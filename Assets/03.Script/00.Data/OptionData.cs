
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class OptionData : EzData
{
    public bool bgmValue;
    public bool soundValue;
    public bool isLeftHand;
    public List<JoystickSetting> joystickSettings = new List<JoystickSetting>();
    //기본값
    public OptionData()
    {
        bgmValue = true;
        soundValue = true;
        isLeftHand = false;

        joystickSettings.Clear();
        foreach(var js in UISetting.Instance.joystickSettings)
        {
            joystickSettings.Add(js);
        }

    }
}
