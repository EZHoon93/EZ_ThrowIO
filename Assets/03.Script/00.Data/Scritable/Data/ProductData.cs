using UnityEditor;

using UnityEngine;



[CreateAssetMenu(fileName = "PrD_", menuName = "EZ_Data/Product", order = 0)]

public class ProductData : ScriptableObject
{
    [SerializeField] int price_coin;
    [SerializeField] int price_gem;
    [SerializeField] int needLevel;
    [SerializeField] string serverKey;
    [SerializeField] string productName;
    public int sPriceCoin => price_coin;
    public int sPriceGem => price_gem;

    public int sNeedLevel => needLevel;
    public string sServerKey => serverKey;

    public string sProductName => productName;

}
