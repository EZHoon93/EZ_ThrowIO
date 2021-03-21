using System.Collections.Generic;
using System.Linq;

using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_ProductManager : MonoBehaviour
{
    enum Mode { Character , Projectile }
    Mode currentMode = Mode.Character;
    enum BuyType { Coin, Gem}
    BuyType buyType = BuyType.Coin;

    [Header("GameObject")]
    [SerializeField] GameObject gameObject_has;
    [SerializeField] GameObject gameObject_lock;
    [SerializeField] GameObject gameObject_stats;
    [SerializeField] GameObject panel_ToggleGroups;

    [SerializeField] GameObject panel_Main;  //메인 캐릭 보이는곳
    [SerializeField] GameObject panel_GameBackground;    //게임씬화면에서 배경역할 

    [Header("PopUp")]
    [SerializeField] GameObject popup_failed;  
    [SerializeField] GameObject popup_success;  
    [Header("Panel")]
    [SerializeField] Transform panel_view;



    [Header("Text")]
    [SerializeField] TextMeshProUGUI text_info;
    [SerializeField] TextMeshProUGUI text_needLevel;
    [SerializeField] TextMeshProUGUI text_coin;
    [SerializeField] TextMeshProUGUI text_gem;
    [SerializeField] TextMeshProUGUI text_use;
    [SerializeField] TextMeshProUGUI text_buyProductInfo;
    [Header("Slider")]
    [SerializeField] Slider slider_damge;
    [SerializeField] Slider slider_rangeExplosion;
    [SerializeField] Slider slider_rangeReach;
    [SerializeField] Slider slider_maxInstallCount;
    [Header("Image")]
    [SerializeField] Image[] image_installCount;
    [SerializeField] Image[] image_element;
    [Header("sprite")]
    [SerializeField] Sprite sprite_fire;
    [SerializeField] Sprite sprite_ice;
    [SerializeField] Sprite sprite_lighting;
    [SerializeField] Sprite sprite_posion;




    List<CharacterObject> characterList = new List<CharacterObject>();
    List<ProjectileObject> projectileList = new List<ProjectileObject>();
    GameObject currentGameObejct;
    ProductData currentProductData;
    int currentIndex;



    private void Start()
    {
        //최초 실행 
        CreateAllCharacter();   //모든캐릭 생성
        CreateAllProjectile();
        Show(PlayerInfo.GetUsingCharacterContainer().sCharacterStatsData.sServerKey, Mode.Character);
        
    }
  
    public void SetActive(UIState state)
    {
        switch (state)
        {
            case UIState.Lobby:
                this.gameObject.SetActive(true);
                panel_Main.SetActive(true);
                panel_GameBackground.SetActive(false);
                panel_ToggleGroups.SetActive(true);
                break;
            case UIState.Wait:
                this.gameObject.SetActive(true);
                panel_Main.SetActive(false);
                panel_GameBackground.SetActive(true);
                panel_ToggleGroups.SetActive(true);
                break;
            case UIState.Game:
                this.gameObject.SetActive(false);
                panel_Main.SetActive(false);
                panel_ToggleGroups.SetActive(false);

                break;
        }
    }
    void CreateAllCharacter()
    {
        var characters = DataContainer.Instance.sCharacterContainers;
        foreach(var c in characters)
        {
            var characterObject = ObjectPoolManger.Instance.PopCharacterObject(c.sId) as CharacterObject;
            characterObject.transform.SetParent(panel_view);
            characterObject.transform.localPosition = Vector3.zero;
            characterObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
            characterObject.transform.localScale = Vector3.one;
            characterObject.gameObject.SetActive(false);
            characterList.Add(characterObject);
        }
    }

    void CreateAllProjectile()
    {
        var projectileContainers = DataContainer.Instance.sProjectileContainers;
        foreach (var c in projectileContainers)
        {
            var projectileObject = ObjectPoolManger.Instance.PopProjectileObject(c.sId) as ProjectileObject;
            projectileObject.transform.SetParent(panel_view);
            projectileObject.transform.localPosition = new Vector3(0,1f, 0);
            projectileObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
            projectileObject.transform.localScale = Vector3.one;
            projectileObject.gameObject.SetActive(false);
            projectileList.Add(projectileObject);
        }
    }


    void SetupText(int selectIndex , ProductData productData)
    {
        switch (currentMode)
        {
            case Mode.Character:
                text_info.text = "Character " + (selectIndex + 1).ToString() + "/" + characterList.Count;
                break;
            case Mode.Projectile:
                text_info.text = "Bomb " + (selectIndex + 1).ToString() + "/" + projectileList.Count;
                break;
        }
        text_coin.text = productData.sPriceCoin.ToString();
        text_gem.text = productData.sPriceGem.ToString();
        text_needLevel.text = productData.sNeedLevel.ToString();
    }
    /// <summary>
    /// 서버키로 비교.
    /// </summary>
    /// <param name="sId"></param>
    void Show(string viewServerKey, Mode mode)
    {
        if (currentGameObejct) currentGameObejct.SetActive(false);
        if (!panel_Main.activeSelf) panel_Main.SetActive(true);
        switch (mode)
        {
            case Mode.Character:
                currentIndex = characterList.FindIndex(s => viewServerKey == s.CharacterStatsData.sServerKey);   //선택된 캐릭 보여준다
                currentGameObejct = characterList[currentIndex].gameObject;
                currentProductData = characterList[currentIndex].CharacterStatsData;
                CheckHasDataByServerKey(PlayerInfo.userData.characterKeys, currentProductData);
                gameObject_stats.SetActive(false);
                break;
            case Mode.Projectile:
                currentIndex = projectileList.FindIndex(s => viewServerKey == s.projectileData.sServerKey);   //선택된 캐릭 보여준다
                currentGameObejct = projectileList[currentIndex].gameObject;
                currentProductData = projectileList[currentIndex].projectileData;
                CheckHasDataByServerKey(PlayerInfo.userData.projectilerKeys, currentProductData);

                //스텟.
                var projectileData = currentProductData as ProjectileData;
                slider_damge.value = projectileData.sDamage;
                slider_rangeExplosion.value = projectileData.sRange_explosion;
                slider_rangeReach.value = projectileData.sRange_reach;
                ShowMaxInstallCount(projectileData.sMaxInstallCount);//최대 설치 수 
                ShowElement(projectileData);    //추가 속성 이미지 보여주기 

                gameObject_stats.SetActive(true);

                break;
        }

        currentGameObejct.SetActive(true);
        SetupText(currentIndex, currentProductData);
    }

    void ShowMaxInstallCount(int maxInstallCount)
    {
        for (int i = 0; i < 3; i++)
        {
            if (i < maxInstallCount)
            {
                image_installCount[i].enabled = true;
            }
            else
            {
                image_installCount[i].enabled = false;
            }
        }

    }

    void ShowElement(ProjectileData projectileData)
    {
        //초기화
        foreach(var i in image_element)
        {
            i.enabled = false;
        }
        if (projectileData.sP_Fire >= 1) SetupElementImage(sprite_fire, projectileData.sP_Fire);
        if (projectileData.sP_Ice >= 1) SetupElementImage(sprite_ice, projectileData.sP_Ice);
        if (projectileData.sP_Light >= 1) SetupElementImage(sprite_lighting, projectileData.sP_Light);
        if (projectileData.sP_Posion >= 1) SetupElementImage(sprite_posion, projectileData.sP_Posion);

    }

    void SetupElementImage(Sprite spriteElement, int elementLevel)
    {
        for(int i = 0; i< elementLevel; i ++)
        {
            image_element[i].sprite = spriteElement;
            image_element[i].enabled = true;
        }
    }
    /// <summary>
    /// 키를 갖고있는지 없는지 결과에 따라 셋팅을 달리해줌
    /// </summary>
    /// <param name="hasServerKey"></param>
    /// <param name="productData"></param>
    void CheckHasDataByServerKey(List<UserHasSeverKey>hasKeyList, ProductData currentProduct)
    {
        var hasServerKey = hasKeyList.Find(s => s.severKey == currentProduct.sServerKey);
        if(hasServerKey != null)
        {
            SetupHasDataByServerKey(hasServerKey.isSelect);
        }
        else
        {
            CheckLock(PlayerInfo.userData.level, currentProduct.sNeedLevel);
        }

    }

    /// <summary>
    /// 유저가 선택된 캐릭을 갖고있다면.=> 사용하는중인지 안하는중인지 에 따라판별
    /// </summary>
    /// <param name="isUsing"></param>
    void SetupHasDataByServerKey(bool isUsing)
    {
        //사용중이라면
        if (isUsing)
        {
            gameObject_has.SetActive(true);
            gameObject_lock.SetActive(false);
            text_use.text = "Using";
        }
        else
        {
            gameObject_has.SetActive(true);
            gameObject_lock.SetActive(false);
            text_use.text = "Use";
        }
    }
    void CheckLock(int playerLevel, int needLevel)
    {
        print(needLevel);
        if (playerLevel < needLevel)
        {
            gameObject_lock.SetActive(true);
            gameObject_has.SetActive(false);

        }
        else
        {
            gameObject_lock.SetActive(false);
            gameObject_has.SetActive(false);
        }
    }

    public void ToogleValueChanged_Character()
    {
        currentMode = Mode.Character;
        //switch (currentUIState)
        //{


        //}
        Show(PlayerInfo.GetUsingCharacterContainer().sCharacterStatsData.sServerKey , currentMode);
    }

    public void ToogleValueChanged_Projectile()
    {
        currentMode = Mode.Projectile;
        Show(PlayerInfo.GetUsingProjectileContainer().sProjectileData.sServerKey, currentMode);
    }

    public void Click_BuyCoin()
    {
        //text_buyProductInfo.text = "정말로 구매?";
        buyType = BuyType.Coin;
    }

    public void Click_BuyGem()
    {
        buyType = BuyType.Gem;

    }

    public void Click_Use()
    {
        switch (currentMode)
        {
            case Mode.Character:
                foreach (var p in PlayerInfo.userData.characterKeys)
                {
                    if (string.Compare(p.severKey, currentProductData.sServerKey) == 0)
                    {
                        p.isSelect = true;
                    }
                    else
                    {
                        p.isSelect = false;
                    }
                }
                break;
            case Mode.Projectile:
                foreach (var p in PlayerInfo.userData.projectilerKeys)
                {
                    if (string.Compare(p.severKey, currentProductData.sServerKey) == 0)
                    {
                        p.isSelect = true;
                    }
                    else
                    {
                        p.isSelect = false;
                    }
                }
                break;
        }
        SetupHasDataByServerKey(true);
    }

    public void Click_BuyConfirm()
    {
        print("Confirm" + buyType);
        switch (buyType)
        {
            case BuyType.Coin:
                if (PlayerInfo.userData.coin >= currentProductData.sPriceCoin)
                {
                    PlayerInfo.userData.coin -= currentProductData.sPriceCoin;
                    Process_BuyProduct();
                }
                else
                {
                    popup_failed.SetActive(true); //아무것도안되면 실패
                }
                return;
            case BuyType.Gem:
                if (PlayerInfo.userData.gem >= currentProductData.sPriceGem)
                {
                    PlayerInfo.userData.coin -= currentProductData.sPriceCoin;
                    Process_BuyProduct();
                }
                else
                {
                    popup_failed.SetActive(true); //아무것도안되면 실패
                }
                return;
        }

    }

    void Process_BuyProduct()
    {
        print("프로세스.."+currentMode);
        UserHasSeverKey userHasSeverKey = new UserHasSeverKey(currentProductData.sServerKey, false);
        switch (currentMode)
        {
            case Mode.Character:
                PlayerInfo.userData.characterKeys.Add(userHasSeverKey);
                break;
            case Mode.Projectile:
                PlayerInfo.userData.projectilerKeys.Add(userHasSeverKey);
                break;
        }
        PlayerInfo.Save();
        SetupHasDataByServerKey(false);
        popup_success.SetActive(true);
    }
    public void Click_LeftButton()
    {
        switch (currentMode)
        {
            case Mode.Character:
                currentIndex = --currentIndex <= -1 ? characterList.Count - 1 : currentIndex;
                Show(characterList[currentIndex].CharacterStatsData.sServerKey, currentMode);
                break;
            case Mode.Projectile:
                 currentIndex = --currentIndex <= -1 ? projectileList.Count - 1 : currentIndex;
                Show(projectileList[currentIndex].projectileData.sServerKey, currentMode);
                break;
        }
    }

    public void Click_RightButton()
    {
        switch (currentMode)
        {
            case Mode.Character:
                currentIndex = ++currentIndex > characterList.Count - 1 ? 0 : currentIndex;
                Show(characterList[currentIndex].CharacterStatsData.sServerKey, currentMode);
                break;
            case Mode.Projectile:
                
                currentIndex = ++currentIndex > projectileList.Count -1 ? 0 : currentIndex;
                Show(projectileList[currentIndex].projectileData.sServerKey, currentMode);
                break;
        }
    }

}
