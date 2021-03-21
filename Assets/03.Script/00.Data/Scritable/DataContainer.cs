using UnityEngine;
using System.Linq;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class DataContainer : ScriptableObject
{
    private const string SettingFileDirectory = "Assets/Resources";
    private const string SettingFilePath = "Assets/Resources/DataContainer.asset";//정확한 파일 위치,만약파
    private const string poolableFilePath = "Assets/08.prefabs/PoolableObject";

    private static DataContainer _instacne;

    public static DataContainer Instance
    {
        get
        {

            if (_instacne != null)
            {
                return _instacne;
            }

            //만약 없으면 파일경로에 해당 파일이 있는지.
            _instacne = Resources.Load<DataContainer>(path: "DataContainer");
            //에디터에서만 작용하므로 전처리기.
            //런타임이 아니라 에디터에서 사용 => usingUenityEditor
            //해당파일도 없으면
            if (_instacne == null)
            {
#if UNITY_EDITOR
                //존재하지않으면 폴더만듬
                if (!AssetDatabase.IsValidFolder(path: SettingFileDirectory))
                {
                    //폴더 만듬
                    AssetDatabase.CreateFolder(parentFolder: "Assets", newFolderName: "Resources");   //파일IO.도되지만 유니티 에디터에 즉각생성안되므로
                }

                //어떠한 이유로 실패해서 안가져올수도있으므로 하드하게 강제적으로 직접 설정.
                _instacne = AssetDatabase.LoadAssetAtPath<DataContainer>(SettingFilePath);

                //그래도 null이라면 만든다.
                if (_instacne == null)
                {
                    _instacne = CreateInstance<DataContainer>();   //아직 메모리에만 존재
                    AssetDatabase.CreateAsset(_instacne, SettingFilePath); //생성 및 저장파일 경로
                }
#endif


            }
            return _instacne;
        }

    }

    [SerializeField] PoolableContainer[] poolableContainers;
    [SerializeField] CharacterContainer[] characterContainers;
    [SerializeField] ProjectileContainer[] projectileContainers;
    [SerializeField] EffectContainer[] effectContainers;
    [SerializeField] AbilityContainer[] abilityContainers;

    //[SerializeField] PoolableObject[] poolableDatas;
    //[SerializeField] EffectObject[] effectObjects;
    //[SerializeField] ProjectileObject[] projectileObjects;



    public PoolableContainer[] sPoolableContainers => poolableContainers;
    public CharacterContainer[] sCharacterContainers => characterContainers;
    public ProjectileContainer[] sProjectileContainers => projectileContainers;

    public EffectContainer[] sEffectContainers => effectContainers;

    public AbilityContainer[] sAbilityContainers => abilityContainers;

    [SerializeField] Item[] item;

    public Item[] Sitems => item;

    [Header("기본 옵션")]
    [Range(0, 1)]
    public float bgmSoundValue;
    [Range(0, 1)]
    public float effectSoundValue;
    public bool isLeftHnad;

    /// <summary>
    /// 리소스 폴더에데이터들을가져온다 
    /// </summary>
    public void Iniatilaze()
    {

        //characterContainers = Resources.LoadAll<CharacterContainer>("Character");
        effectContainers = Resources.LoadAll<EffectContainer>("Container/Effect");
        foreach(var e in effectContainers)
        {
            e.Inialize();
        }
    }

    #region Character 

    public CharacterContainer GetCharacterContainerBySeverKey(string serverKey)
    {
        for(int i =0; i < characterContainers.Length; i++)
        {
            if (string.Compare(characterContainers[i].sCharacterStatsData.sServerKey, serverKey) == 0)
            {
                return characterContainers[i];
            }
        }
        return null;
    }

    public CharacterContainer GetCharacterContainerByContainerId(string containerId)
    {
        
        for (int i = 0; i < characterContainers.Length; i++)
        {
            if (string.Compare(characterContainers[i].sId, containerId) == 0)
            {
                return characterContainers[i];
            }
        }
        return null;
    }
    #endregion

    #region Projectile

  

    public ProjectileContainer GetProjectileContainerByServerKey(string serverKey)
    {
        for (int i = 0; i < projectileContainers.Length; i++)
        {
            if (string.Compare(projectileContainers[i].sProjectileData.sServerKey, serverKey) == 0)
            {
                return projectileContainers[i];
            }
        }
        return null;
    }

    public ProjectileContainer GetProjectileContainerByContainerId(string containerId)
    {
        for (int i = 0; i < projectileContainers.Length; i++)
        {
            if (string.Compare(projectileContainers[i].sId, containerId) == 0)
            {
                return projectileContainers[i];
            }
        }
        return null;
    }
    #endregion


    
    public PoolableContainer GetPoolableContainerByContainerId(string containerId)
    {
        for(int i =0; i< poolableContainers.Length; i++)
        {
            if(string.Compare(poolableContainers[i].sId , containerId) == 0)
            { 
                return poolableContainers[i];
            }
        }

        return null;
    }

    public EffectContainer GetEffectContainerByEffectType(EffectType effectType)
    {
        for (int i = 0; i < effectContainers.Length; i++)
        {
            
            if (effectContainers[i].sEffectType == effectType) return effectContainers[i];

        }

        return null;
    }
    

    public AbilityContainer GetAbilityByCode(string code)
    {
        foreach(var d in abilityContainers)
        {
            if(string.Compare(d.sCode , code) == 0)
            {
                return d;
            }
        }

        return null;
    }


    public string GetAbilityByType(AbilityType abilityType)
    {
        foreach (var d in abilityContainers)
        {
            if(d.sAbilityType == abilityType)
            {
                return d.sCode;
            }
        }

        return null;
    }

    public BuffType GetBuffTypeByCode(int code)
    {
        switch (code)
        {
            case 0:
                return BuffType.Null;
            case 1:
                return BuffType.Slow;
            case 2:
                return BuffType.Shield;
            case 3:
                return BuffType.Stun;
            case 4:
                return BuffType.Posion;
            case 5:
                return BuffType.Run;
            case 6:
                return BuffType.Sleep;
            case 7:
                return BuffType.Dark;
        }

        return BuffType.Null;
    }

    public int GetBuffCodeByType(BuffType buffType)
    {
        switch (buffType)
        {
            case BuffType.Null:
                return 0;
            case BuffType.Slow:
                return 1;
            case BuffType.Shield:
                return 2;
            case BuffType.Stun:
                return 3;
            case BuffType.Posion:
                return 4;
            case BuffType.Run:
                return 5;
            case BuffType.Sleep:
                return 6;
            case BuffType.Dark:
                return 7;
        }

        return 0;
    }
}