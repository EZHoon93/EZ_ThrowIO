using System.Collections;


using UnityEngine;

/// <summary>
/// 기타 상품들
/// </summary>
public class ProductExp : MonoBehaviour
{

    [SerializeField] int price;
    [SerializeField] ProductType productType;

    public ProductType ProductType => productType;
    //Shop_Exp shopUI;

    //public void SetUpShopUI(Shop_Exp _shopUI)
    //{
    //    shopUI = _shopUI;
    //}

    public void ClickBuyButton()
    {
        //shopUI.Recive_ClickProductEvent(this.productType, this.price);

    }
}
