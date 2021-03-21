using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UI_PlayerAbilityIcon : PoolableObject
{
    [SerializeField] Image image_ability;
    [SerializeField] TextMeshProUGUI text_level;
    public int level;
    public int arrayIndex;  //순서 우선순위

    

    public void Setup(AbilityContainer abilityContainer)
    {
       image_ability.sprite = abilityContainer.sAbilityImage;
        arrayIndex = (int)abilityContainer.sAbilityType;

    }

    public void UpdateStats(int _level, bool max)
    {
        level = _level;
        if (max)
        {
            text_level.text = "M";
        }
        else
        {
            text_level.text = level.ToString();
        }
    }
}
