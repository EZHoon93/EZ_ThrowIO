using System.Collections;

using TMPro;

using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{

    [SerializeField] GameObject selectLoginPanel;
    [SerializeField] TMP_InputField _InputField;

    private void Awake()
    {
        selectLoginPanel.gameObject.SetActive(false);
    }

    private void Start()
    {
        DataContainer.Instance.Iniatilaze();

        //처음접속이라면..
        if (!PlayerPrefs.HasKey("userPId"))
        {
            print("처음 접속 입니다");
            //처음 접속.
            selectLoginPanel.gameObject.SetActive(true);
        }

        UserData _userData = new UserData("TestPlayer", "test");
        OptionData optionData = new OptionData();
        PlayerInfo.userData = _userData;
        PlayerInfo.optionData = optionData;
        PlayerInfo.Save();
        PlayerInfo.nickName = "Player " + Random.Range(0, 999);
        foreach (var u in PlayerInfo.optionData.joystickSettings)
        {
            UltimateJoystick.GetUltimateJoystick(u.joystickName).SetupJoystick(u);
        }
        SceneManager.LoadScene("lobby1");
    }

    //구글로그인 클릭시 닉네임결정
    public void Login_GooglePlay()
    {

    }

    //게스트로 클릭시, 닉네임 자동결정 => Player 1313 이런식
    public void Login_Guset()
    {
        //
        PlayerInfo.nickName = "Player " + Random.Range(0, 999);

        SceneManager.LoadScene("lobby1");
    }




}
