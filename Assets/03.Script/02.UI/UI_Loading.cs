
using TMPro;
using UnityEngine;

public class UI_Loading : MonoBehaviour
{
    //bool isSearch;
    string content = "씬을 불러오는 중입니다";
    [SerializeField] TextMeshProUGUI text_findInfo;
    int index;

    private void OnEnable()
    {
        //isSearch = false;
        InvokeRepeating("RepeatText", 0.0f, 1.0f);
    }

    private void OnDisable()
    {
        CancelInvoke("RepeatText");
    }

    void RepeatText()
    {
        print("반복중입니다");
        var addText =".";
        string reuslt = null;
        for(int i = 0; i < index; i++)
        {
            reuslt += addText;
        }

        if (++index > 4) index = 0;

        text_findInfo.text = content + reuslt;
        
    }


}
