using UnityEditor;

using UnityEngine;



[CreateAssetMenu(fileName = "CSD_", menuName = "EZ_Data/CharacterStats", order = 1)]
public class CharacterStatsData : ProductData
{
    [SerializeField] int hp;
    [SerializeField] float speed;
    public int sHp => hp;
    public float sSpeed => speed;


}
