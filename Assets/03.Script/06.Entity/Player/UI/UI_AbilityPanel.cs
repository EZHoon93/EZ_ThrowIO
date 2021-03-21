using System;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using DG.Tweening;

public class UI_AbilityPanel : MonoBehaviour
{
    [SerializeField] PoolableContainer abilityButtonContainer;
    [SerializeField] Transform selectPanel;   //선택할 능력치 패널
    [SerializeField] DOTweenAnimation tweenOpenAnimation;
    bool isShow;    //보여주고있는중인지.

    

    public void SetActiveSelectUI(bool active , List<AbilityType >notSelectAbitiltiys)
    {
        if (active) //true 
        {
            if (isShow) return;
            isShow = true;
            SelectRandomAbility(notSelectAbitiltiys);
            tweenOpenAnimation.transform.localPosition = Vector3.zero;
            tweenOpenAnimation.DORestart();
            selectPanel.gameObject.SetActive(true);

        }
        else //false 
        {
            isShow = false;
            selectPanel.gameObject.SetActive(false);
            foreach (var a in selectPanel.GetComponentsInChildren<AbilityButton>())
            {
                a.Push();
            }
        }
    }
    
    void SelectRandomAbility(List<AbilityType> notSelectAbitiltiys)
    {
        var abilitys = DataContainer.Instance.sAbilityContainers;
        for (int i = 0; i<3; i++)
        {
            AbilityContainer selectAbilityContainer;
            do
            {
                int ran = UnityEngine.Random.Range(0, abilitys.Length);
                selectAbilityContainer = abilitys[ran];
                
            } while (notSelectAbitiltiys.Any(s => s == selectAbilityContainer.sAbilityType));   //같다면 다시 .
            notSelectAbitiltiys.Add(selectAbilityContainer.sAbilityType);   //뽑힌것도 불가능리스트에 추가
            var abilityButton =  ObjectPoolManger.Instance.PopPoolableObject(abilityButtonContainer.sId) as AbilityButton;
            abilityButton.transform.SetParent(selectPanel);
            abilityButton.Setup(this, selectAbilityContainer);
        }
    }


    public void ClickAblityButton(AbilityButton abilityButton, AbilityContainer abilityContainer)
    {
        var playerController = GameManager.instance.myPlayer;
        if (playerController == null) return;

        isShow = false; //꺼진 상태로 전환

        foreach (var a in selectPanel.GetComponentsInChildren<AbilityButton>())
        {
            a.Push();
        }

        //playerController.Local_UpdateAbilityStats(abilityContainer);
        playerController.playerStats.playerAbilityStats.Local_UpdateAbilityStats(abilityContainer, playerController);
        playerController.playerStats.playerScore.AbilityPoint--;

    }
}
