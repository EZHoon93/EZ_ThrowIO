using UnityEngine;



[CreateAssetMenu(fileName = "AbD_AP_", menuName = "EZ_Poolable/Create SkillContainer", order = 5)]
public class SkillContainer : AbilityContainer
{
    [SerializeField] float coolTime;
    [SerializeField] float durationTime;    

    public float sCoolTime => coolTime;
    public float sDurationTime => durationTime;

}
