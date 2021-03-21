using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop_Character : MonoBehaviour , IEZ_Initilazed
{
    [SerializeField] PoolableContainer poolableContainer;
    public Transform contentPanel;
    public Transform viewPanel;

    CharacterObject currentViewCharacter;
    CharacterUI currentCharacterUI;
    public List<CharacterUI> characterUIList = new List<CharacterUI>();

    public Slider hpSlider;
    public Slider speedSlider;
    public Slider ammorSlider;
    public TextMeshPro expText;

    public List<int> test = new List<int>();
    public void EZ_Initialized()
    {
        print("Shop Inialized");
        //최초 1번 실행
        var avaterScriptables = DataContainer.Instance.sCharacterContainers;
        for (int i = 0; i < avaterScriptables.Length; i++)
        {
            var characterUI = ObjectPoolManger.Instance.PopPoolableObject(poolableContainer.name) as CharacterUI;
            var characterData = avaterScriptables[i];
            characterUI.characterImage.sprite = characterData.sCharacterSrpite;
            characterUI.characterContainer = characterData;
            characterUI.characterNameText.text = characterData.sUIName;
            characterUI.needLevelText.text = characterData.sNeedLevel.ToString();
            characterUI.transform.SetParent(contentPanel.transform);
            characterUI.transform.localPosition = Vector3.zero;
            characterUI.transform.rotation = Quaternion.identity;
            characterUI.transform.localScale = new Vector3(1, 1, 1);
            characterUI.click += SetupCharacter_ViewUI;      //해당 UI클릭시 발생할 이벤트 등록
            characterUI.SetActiveFocus(false);
            characterUI.SetActiveBuyButton(true);

            characterUIList.Add(characterUI);
        }
    
    }

    private void OnEnable()
    {
        //SetupCharacter_ViewUI(this, PlayerInfo.GetUsingCharacterData());    //현재사용중인 캐릭터 뷰 

        UpdateCharacterList(PlayerInfo.userData.characterKeys.ToArray());

     

    }

    void SetupCharacter_ViewUI(object sender, CharacterUI characterUI)
    {
        if (currentViewCharacter)
        {
            currentViewCharacter.Push();
            currentCharacterUI.SetActiveFocus(false);
            characterUI.SetActiveFocus(true);
        }

        var characterObject = ObjectPoolManger.Instance.PopCharacterObject(characterUI.characterContainer.name) as CharacterObject;
        characterObject.transform.SetParent(viewPanel);
        characterObject.transform.localPosition = Vector3.zero;
        characterObject.transform.rotation = Quaternion.Euler(0,180,0);
        characterObject.transform.localScale = new Vector3(1, 1, 1);

        hpSlider.value = characterUI.characterContainer.sCharacterStatsData.sHp;
        speedSlider.value = characterUI.characterContainer.sCharacterStatsData.sSpeed;
        currentViewCharacter = characterObject;
        currentCharacterUI = characterUI;
    }

    public void UpdateCharacterList(UserHasSeverKey[] _userHasSeverKey)
    {
        //쿼리문,linq으로도 가능하나.. 메모리낭비를 하지않기 위해
        for(int i =0; i< _userHasSeverKey.Length; i++)
        {
            for (int j = 0; j < characterUIList.Count; j++)
            {
                
                //보유중인 캐릭터를 찾음
                if(_userHasSeverKey[i].severKey == characterUIList[j].characterContainer.sCharacterStatsData.sServerKey)
                {
                    characterUIList[j].SetActiveBuyButton(false);
                    //사용중인 캐릭터
                    if (_userHasSeverKey[i].isSelect)
                    {
                        print(_userHasSeverKey[i].severKey);
                        characterUIList[j].ChangeUseButton(true);
                        characterUIList[j].SetActiveFocus(true);
                        characterUIList[j].ShowCharacterUIOnShop();     //해당 캐릭을 뷰 에 보여준다.
                    }
                    //보유중이나 사용하지않는 캐릭터
                    else
                    {
                        characterUIList[j].ChangeUseButton(false);
                        characterUIList[j].SetActiveFocus(false);
                    }
                }

            }

        }
    }




}
