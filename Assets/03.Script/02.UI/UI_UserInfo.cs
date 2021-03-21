using System.Collections;

using UnityEngine;
using TMPro;


public class UI_UserInfo : MonoBehaviour
{
    public TextMeshProUGUI userNameText;
    public TextMeshProUGUI userLevelText;
    
    
    // Use this for initialization
    public void Setup()
    {
        userNameText.text = PlayerInfo.nickName;
        userLevelText.text = PlayerInfo.userData.level.ToString();
    }

    
}
