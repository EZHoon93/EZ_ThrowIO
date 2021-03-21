using UnityEngine;


[CreateAssetMenu(fileName = "CC_", menuName = "EZ_Poolable/CharacterContainer", order = 1)]

[System.Serializable]
public class CharacterContainer : PoolableContainer
{
    [SerializeField] CharacterStatsData characterStatsData;
    [SerializeField] Sprite characterSprite;

    [SerializeField] string characterUIName;
    [SerializeField] int needLevel;
    [SerializeField] string expText;        //특이사항
    
    public CharacterStatsData sCharacterStatsData => characterStatsData;

    public Sprite sCharacterSrpite => characterSprite;

    public string sUIName=> characterUIName;

    public int sNeedLevel => needLevel;



}