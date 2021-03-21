using System.Collections;

using UnityEngine;

public interface ISettingUI  
{

    void ChangeAnchor();
    //void Update
    void ChangeSettingMode(bool isSetting);

    void SetupJoystick(JoystickSetting joystickSetting);

    string GetName();

    float GetSizeValue();

    Vector2 GetPosion();

    void ChangeSize(float sizeValue);

    //JoystickSetting GetValues();
}
