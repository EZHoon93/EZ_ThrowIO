using System.Collections;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameResult : MonoBehaviour
{
    [SerializeField] Slider expSlider;
    [SerializeField] TextMeshProUGUI text_userLevel;
    [SerializeField] TextMeshProUGUI text_userExp;
    [SerializeField] TextMeshProUGUI text_userNickName;

    [SerializeField] TextMeshProUGUI text_gameResult;
    [SerializeField] TextMeshProUGUI text_coin;
    [SerializeField] TextMeshProUGUI text_exp;

    int userLevel;
    PlayerGameResultScore playerGameResultScore;
    public void SetupGameResultInfo(PlayerGameResultScore _playerGameResultScore )
    {
        playerGameResultScore = _playerGameResultScore;
        var leftTime = string.Format( "{0:0.#}", (playerGameResultScore.endTime - playerGameResultScore.startTime) );
        text_gameResult.text = null;
        text_gameResult.text += "생존시간 : " + leftTime ;
        text_gameResult.text += "\n최대레벨 : " + playerGameResultScore.level;
        text_gameResult.text += "\n최대점수 : " + playerGameResultScore.score;
        text_gameResult.text += "\n최대 킬 : " + playerGameResultScore.maxKillCount;

        text_coin.text = "+" + playerGameResultScore.coin;
        text_exp.text = "+" + playerGameResultScore.exp;

        UpdateUserInfo();    //현재 유저 인포
        PlayerInfo.userData.coin += playerGameResultScore.coin;
        PlayerInfo.userData.AddExp(playerGameResultScore.exp);
        PlayerInfo.Save();

        Invoke("ShowResult", 3.0f);
    }

    void ShowResult()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(UpdateUserExpSlider(playerGameResultScore.exp));

    }

    void UpdateUserInfo()
    {
        expSlider.maxValue = PlayerInfo.userData.maxExp;
        expSlider.value = PlayerInfo.userData.exp;
        userLevel = PlayerInfo.userData.level;
        text_userLevel.text = userLevel.ToString();

    }

    IEnumerator UpdateUserExpSlider(int addAmount)
    {
        var destValue = expSlider.value + addAmount;

        while (expSlider.value -1 < destValue)
        {
            if(expSlider.value >= expSlider.maxValue)
            {
                userLevel++;
                text_userLevel.text = userLevel.ToString();
                expSlider.maxValue = Utility.GetMaxExp(userLevel);
                destValue -= expSlider.maxValue;
                expSlider.value = 0;
            }
            else
            {
                expSlider.value = Mathf.Lerp(expSlider.value, destValue, Time.deltaTime * 3f);
            }

            text_userExp.text = (int)expSlider.value + "/" + (int)expSlider.maxValue;
            yield return null;

        }
    }


}
