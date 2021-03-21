using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop_Projectile : MonoBehaviour , IEZ_Initilazed
{
    [SerializeField] PoolableContainer poolableContainer;
    public Transform contentPanel;
    public Transform viewPanel;

    ProjectileObject currentViewObject;
    ProjectileUI currentProjectileUI;
    List<ProjectileUI> ProjectileUIList = new List<ProjectileUI>();

    public Slider hpSlider;
    public Slider speedSlider;
    public Slider ammorSlider;
    public TextMeshPro expText;

    public Transform startTr;
    public Transform endTr;



    public void EZ_Initialized()
    {
        print("Shop Projectile Inialized");
        //최초 1번 실행
        var projectileContainers = DataContainer.Instance.sProjectileContainers;
        for (int i = 0; i < projectileContainers.Length; i++)
        {
            var ProjectileUI = ObjectPoolManger.Instance.PopPoolableObject(poolableContainer.name) as ProjectileUI;
            var projectileContainer = projectileContainers[i];
            ProjectileUI.characterImage.sprite = projectileContainer.sSprite;
            ProjectileUI.projectileContainer = projectileContainer;
            ProjectileUI.productNameText.text = projectileContainer.sUIName;
            ProjectileUI.needLevelText.text = projectileContainer.sNeedLevel.ToString();

            ProjectileUI.transform.SetParent(contentPanel.transform);
            ProjectileUI.transform.localPosition = Vector3.zero;
            ProjectileUI.transform.rotation = Quaternion.identity;
            ProjectileUI.transform.localScale = new Vector3(1, 1, 1);
            ProjectileUI.click += SetupCharacter_ViewUI;      //해당 UI클릭시 발생할 이벤트 등록
            ProjectileUI.SetActiveFocus(false);
            ProjectileUI.SetActiveBuyButton(true);

            ProjectileUIList.Add(ProjectileUI);
        }

    }

    private void OnEnable()
    {
        UpdateCharacterList(PlayerInfo.userData.projectilerKeys.ToArray());
    }

    public void Show()
    {
        if (!this.gameObject.activeSelf) return;
        var projectileObject = ObjectPoolManger.Instance.PopProjectileObject(currentProjectileUI.projectileContainer.name) as ProjectileObject;
        var projectileData = currentProjectileUI.projectileContainer.sProjectileData;
        ProjectileInfo projectileInfo;
        projectileInfo.damageLevel = 0;
        projectileInfo.playerStats = null;
        projectileInfo.viewID = 0;
        projectileInfo.rangeLevel = 1;
        projectileInfo.p_fire = projectileData.sP_Fire;
        projectileInfo.p_posion = projectileData.sP_Posion;
        projectileInfo.p_ice = projectileData.sP_Ice;
        projectileInfo.p_lighting = projectileData.sP_Ice;
        projectileInfo.projectileSpeedLevel = 1;
        projectileInfo.startPoint = startTr.position;
        projectileInfo.endPoint = endTr.position;
        projectileObject.Play(projectileInfo);
        currentViewObject = projectileObject;
    }

    void SetupCharacter_ViewUI(object sender, ProjectileUI ProjectileUI)
    {
        print("클릭!!");
        if (currentViewObject)
        {
            currentProjectileUI.SetActiveFocus(false);
            currentViewObject.Push();
        }
        currentProjectileUI = ProjectileUI;
        currentProjectileUI.SetActiveFocus(true);
        //StartCoroutine(Simulator());
        CancelInvoke("Show");
        InvokeRepeating("Show", 0f,2.0f);

    }



    public void UpdateCharacterList(UserHasSeverKey[] userHasSeverKeys)
    {
        //쿼리문,linq으로도 가능하나.. 메모리낭비를 하지않기 위해
        print("업데이트....");
        for (int i = 0; i < userHasSeverKeys.Length; i++)
        {
            for (int j = 0; j < ProjectileUIList.Count; j++)
            {

                //유중인 캐릭터를 찾음
                if (userHasSeverKeys[i].severKey == ProjectileUIList[j].projectileContainer.sProjectileData.sServerKey)
                {
                    ProjectileUIList[j].SetActiveBuyButton(false);
                    //사용중인 캐릭터
                    if (userHasSeverKeys[i].isSelect)
                    {
                        ProjectileUIList[j].ChangeUseButton(true);
                        ProjectileUIList[j].SetActiveFocus(true);
                        ProjectileUIList[j].Show_UIShop();     //해당 캐릭을 뷰 에 보여준다.
                    }
                    //보유중이나 사용하지않는 캐릭터
                    else
                    {
                        print("보유중x");
                        ProjectileUIList[j].ChangeUseButton(false);
                        ProjectileUIList[j].SetActiveFocus(false);
                    }
                }

            }

        }
    }


}
