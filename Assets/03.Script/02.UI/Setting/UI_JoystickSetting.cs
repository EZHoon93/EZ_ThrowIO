using System.Collections;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_JoystickSetting : MonoBehaviour
{

    UltimateJoystick currentEditJoystick; //현재 움직이는 유아이
    [SerializeField] Slider slider_joystickSize;
    private void OnEnable()
    {
        UIManager.instance.SetActiveJoysticks(true);
        foreach (var u in PlayerInfo.optionData.joystickSettings)
        {
            UltimateJoystick.GetUltimateJoystick(u.joystickName).ChangeSettingMode(true);
        }

    }
    private void OnDisable()
    {
        if (UIManager.instance == null) return;
        UIManager.instance.SetActiveJoysticks(false);
        foreach (var u in PlayerInfo.optionData.joystickSettings)
        {
            UltimateJoystick.GetUltimateJoystick(u.joystickName).ChangeSettingMode(false);
            UltimateJoystick.GetUltimateJoystick(u.joystickName).SetupJoystick(u);
        }
    }
    private void Update()
    {
        if (currentEditJoystick != null)
        {
            currentEditJoystick.ChangeSize(slider_joystickSize.value);
            currentEditJoystick.joystickSize = slider_joystickSize.value;
            //currentEditJoystick.UpdatePositioning();
        }
        else
        {
            slider_joystickSize.interactable = false;
        }
    }
    public void Setup(UltimateJoystick selectJoystick)
    {
        currentEditJoystick = selectJoystick;
        float size = selectJoystick.joystickSize;
        slider_joystickSize.value = size;
        slider_joystickSize.interactable = true;

    }

    public void Click_Reset()
    {
        foreach (var u in UISetting.Instance.joystickSettings)
        {
            UltimateJoystick.GetUltimateJoystick(u.joystickName).SetupJoystick(u);
        }
        currentEditJoystick = null;
    }



    public void Click_Save()
    {
        var joystickList = PlayerInfo.optionData.joystickSettings;
        for (int i =0; i < joystickList.Count; i++)
        {
            var joystickName = joystickList[i].joystickName;
            joystickList[i].size = UltimateJoystick.GetUltimateJoystick(joystickName).joystickSize;
            joystickList[i].vector2 = new Vector2(UltimateJoystick.GetUltimateJoystick(joystickName).positionHorizontal, UltimateJoystick.GetUltimateJoystick(joystickName).positionVertical);
        }

        PlayerInfo.Save();
    }


}
