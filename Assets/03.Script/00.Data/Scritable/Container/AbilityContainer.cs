
using UnityEngine;

[CreateAssetMenu(fileName = "AbD_", menuName = "EZ_Poolable/Create AbiltiyContainer", order = 5)]

public class AbilityContainer : ScriptableObject
{
    [SerializeField] Sprite abilityImage;    //버튼안에 이미지
    [SerializeField] Sprite outLineImage;       //버튼의색
    [SerializeField] int maxLevel;
    [SerializeField] AbilityType abilityType;
    [SerializeField] string code;
    [SerializeField] string infoContent;    //설명텍스트

    public Sprite sAbilityImage => abilityImage;

    public Sprite sOutLineImage => outLineImage;
    public int sMaxLevel => maxLevel;
    public AbilityType sAbilityType => abilityType;
    public string sCode => code;

    public string sInfoContent => infoContent;

}
