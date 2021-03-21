using System;

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharacterUI : PoolableObject , IPointerClickHandler
{
    public Product_Code product_Code;
    public CharacterContainer characterContainer;
    public event EventHandler<CharacterUI> click;
    public Image characterImage;
    public Image focusImage;
    [Header("Text")]
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI needLevelText;
    public TextMeshProUGUI useText;

    public Button buyButton;     //구매 버튼
    public Button useButton;    //사용하기 버튼 

    public void OnPointerClick(PointerEventData eventData)
    {
        ShowCharacterUIOnShop();
    }

    public void ShowCharacterUIOnShop()
    {
        click(this, this);
    }
    public void SetActiveFocus(bool active)
    {
        focusImage.gameObject.SetActive(active);
    }

    public void SetActiveBuyButton(bool active)
    {
        buyButton.gameObject.SetActive(active);
    }

    public void ChangeUseButton(bool isUse)
    {
        if (isUse)
        {
            useButton.interactable = false;
            
            useText.text = "Using";
        }
        else
        {
            useButton.interactable = true;
            useText.text = "Use";
        }
        //사용하기버튼으로 바꾼다면
    }

    public override void Push()
    {
        click = null;
        base.Push();
    }


    public void Click_BuyButton()
    {
        //print("클릭버튼"+ this.characterContainer.sCharacterStatsData.sProductName);
        //ShopManager.Instance.Show_Product(characterContainer.sCharacterStatsData , product_Code);
    }

    public void Click_UseButton()
    {
        print("Click Use" + characterContainer.sCharacterStatsData.sServerKey);
        //유저가 갖고있는키 전체 false, 해당이랑같은값은 true
        foreach (var p in PlayerInfo.userData.characterKeys)
        {
            if (string.Compare(p.severKey, characterContainer.sCharacterStatsData.sServerKey) == 0)
            {
                print(p.severKey);
                p.isSelect = true;
            }

            else
            {
                p.isSelect = false;
            }

        }


        //ShopManager.Instance.shop_Character.UpdateCharacterList(PlayerInfo.userData.characterKeys.ToArray());
        //ShopManager.Instance.ChangeMyCharacter?.Invoke();
    }
}
