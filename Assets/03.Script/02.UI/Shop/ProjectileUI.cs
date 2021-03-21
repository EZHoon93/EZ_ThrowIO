using System;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProjectileUI : PoolableObject, IPointerClickHandler
{
    public Product_Code product_Code;
    public ProjectileContainer projectileContainer;
    public event EventHandler<ProjectileUI> click;
    public Image characterImage;
    public Image focusImage;
    [Header("Text")]
    public TextMeshProUGUI productNameText;
    public TextMeshProUGUI needLevelText;
    public TextMeshProUGUI useText;

    public Button buyButton;     //구매 버튼
    public Button useButton;    //사용하기 버튼 

    public void OnPointerClick(PointerEventData eventData)
    {
        Show_UIShop();
    }

    public void Show_UIShop()
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
    public void Click_BuyButton()
    {
        //ShopManager.Instance.Show_Product(projectileContainer.sProjectileData, product_Code);
    }

    public void Click_UseButton()
    {
        foreach(var p in  PlayerInfo.userData.projectilerKeys)
        {
            if(string.Compare(p.severKey , projectileContainer.sProjectileData.sServerKey ) == 0)
            {
                p.isSelect = true;
            }

            else
            {
                p.isSelect = false;
            }

        }


        //ShopManager.Instance.shop_Projectile.UpdateCharacterList(PlayerInfo.userData.projectilerKeys.ToArray());
    }
}
