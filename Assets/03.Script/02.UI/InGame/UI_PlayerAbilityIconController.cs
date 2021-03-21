using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class UI_PlayerAbilityIconController : MonoBehaviour
{
    [SerializeField] PoolableContainer UIPlayerIconContainer;

    Dictionary<string, UI_PlayerAbilityIcon> dic_UIPlayerAbilityIcon = new Dictionary<string, UI_PlayerAbilityIcon>();
    public void UpdateIcon(AbilityContainer changeAbilityContainer, Dictionary<string, int> playerStats )
    {
        foreach(var ps in playerStats)
        {
            //데이터가 있느것들만
            if(ps.Value > 0)
            {
                bool isMax = false; 
                //최초 생성시 딕셔너리 추가 및 이미지 생성
                if(!dic_UIPlayerAbilityIcon.ContainsKey(ps.Key))
                {
                    print("최초생성");
                    var playerIcon = ObjectPoolManger.Instance.PopPoolableObject(UIPlayerIconContainer.sId) as UI_PlayerAbilityIcon;
                    playerIcon.Setup(changeAbilityContainer);
                    playerIcon.transform.SetParent(this.transform);
                    dic_UIPlayerAbilityIcon.Add(ps.Key, playerIcon);
                }
                //레벨 업데이트
                if (ps.Value >= 3) isMax = true;        //최대 맥스레벨을 3으로 예상중
                dic_UIPlayerAbilityIcon[ps.Key].UpdateStats(ps.Value, isMax);
                
            }
        }

        SortIcon();
    }

    /// <summary>
    /// 아이콘 순서 정리
    /// </summary>
    void SortIcon()
    {
        var dic = dic_UIPlayerAbilityIcon.OrderByDescending(x => x.Value.arrayIndex);
        int i = 0;
        foreach(var ui in dic)
        {
            ui.Value.transform.SetSiblingIndex(i++);
        }
    }
}
