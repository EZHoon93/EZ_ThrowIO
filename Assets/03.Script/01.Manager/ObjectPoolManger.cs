using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;



public class ObjectPoolManger : MonoBehaviour  
{
    #region 씽글톤
    public static ObjectPoolManger Instance
    {
        get
        {
            // 만약 싱글톤 변수에 아직 오브젝트가 할당되지 않았다면
            if (m_instance == null)
            {
                // 씬에서 GameManager 오브젝트를 찾아 할당
                m_instance = FindObjectOfType<ObjectPoolManger>();
            }

            // 싱글톤 오브젝트를 반환
            return m_instance;
        }
    }

    private static ObjectPoolManger m_instance; // 싱글톤이 할당될 static 변수

    #endregion

    [SerializeField] Transform projectilePoolList;
    [SerializeField] Transform genericPoolList;
    [SerializeField] Transform effectPoolList;
    [SerializeField] Transform characterPoolList;



    public Dictionary<string, GenericPool<PoolableObject>> projectileDic = new Dictionary<string, GenericPool<PoolableObject>>();
    public Dictionary<string, GenericPool<PoolableObject>> genericDic = new Dictionary<string, GenericPool<PoolableObject>>();
    public Dictionary<EffectType, GenericPool<PoolableObject>> effectDic = new Dictionary<EffectType, GenericPool<PoolableObject>>();
    public Dictionary<string, GenericPool<PoolableObject>> characterDic = new Dictionary<string, GenericPool<PoolableObject>>();



    #region Photon
    public readonly Dictionary<string, PunObjectPool> punDic = new Dictionary<string, PunObjectPool>();
    public PunObjectPool[] punObjectPools;
    public PunPoolManagerBridge poolManagerBridge;
    #endregion


    private void Awake()
    {

        // 씬에 싱글톤 오브젝트가 된 다른 GameManager 오브젝트가 있다면
        if (Instance != this)
        {
            // 자신을 파괴
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        poolManagerBridge.Setup();
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        //poolManagerBridge.Setup();
        //foreach (var p in projectileDic.Values)
        //{
        //    p.Dispose();
        //}
        //foreach (var p in genericDic.Values)
        //{
        //    p.Dispose();
        //}
        //foreach (var p in effectDic.Values)
        //{
        //    p.Dispose();
        //}
        //모두 다시 스택으로 집어넣는다.
    }

    public void AllPush()
    {
        poolManagerBridge.Setup();
        foreach (var p in projectileDic.Values)
        {
            p.Dispose();
        }
        foreach (var p in genericDic.Values)
        {
            p.Dispose();

        }
        foreach (var p in effectDic.Values)
        {
            p.Dispose();
        }
        foreach (var p in characterDic.Values)
        {
            p.Dispose();
        }
    }
    private void Start()
    {
       

        //포톤 초기화
        for(int i =0; i< punObjectPools.Length; i++)
        {
            PunObjectPool _punObjectPool = null;
            var _prefabId = punObjectPools[i].prefabObject.name;
            bool cached = punDic.TryGetValue(_prefabId, out _punObjectPool);
            //최초 생성이라면
            if (!cached)
            {
                _punObjectPool = punObjectPools[i];
                _punObjectPool.IniailizePool();
            }
            punDic.Add(_prefabId, _punObjectPool);
        }
        //
        //foreach (var e in DataContainer.Instance.sEffectContainers)
        //{
        //    var eo = PopEffectObject(e.sEffectType);
        //    eo.Push();
        //}

        //foreach (var p in DataContainer.Instance.sPoolableContainers)
        //{
        //    PopPoolableObject(p.sId);
        //}
    }

    #region

    public GameObject PunPop(string _prefabId)
    {
        return punDic[_prefabId].Pop();
    }
    #endregion

    #region Poolable
    public PoolableObject PopPoolableObject(string containerId)
    {
        GenericPool<PoolableObject> genericPool = null;
        bool cached = genericDic.TryGetValue(containerId, out genericPool);
        //캐시되지 않았다면, 최초로 생성.
        if (!cached)
        {
            var _poolableContainer = DataContainer.Instance.GetPoolableContainerByContainerId(containerId);
            GameObject emptyObject = new GameObject("Poolable "+containerId + " Pool");  //빈객체 생성 후 해당오브젝트의 자식으로 이동
            emptyObject.transform.SetParent(this.genericPoolList.transform);
            genericPool = new GenericPool<PoolableObject>(_poolableContainer.sAllowCount, _poolableContainer.sAddCount , () =>
            {
                PoolableObject pObj = Instantiate(_poolableContainer.sPrefab.GetComponent<PoolableObject>());
                pObj.Create(genericPool, emptyObject.transform);
                return pObj;
            });
            genericPool.Allocate();
            genericDic.Add(containerId, genericPool);
        }
        return genericPool.PopObject();
    }

    #endregion

  


    #region CharacterObject
    public PoolableObject PopCharacterObject(string _containerId)
    {
        GenericPool<PoolableObject> genericPool = null;
        bool cached = characterDic.TryGetValue(_containerId, out genericPool);
        //캐시되지 않았다면, 최초로 생성.
        if (!cached)
        {
            var _characterContainer = DataContainer.Instance.GetCharacterContainerByContainerId(_containerId);
            GameObject emptyObject = new GameObject("Character "+_containerId + " Pool");  //빈객체 생성 후 해당오브젝트의 자식으로 이동
            emptyObject.transform.SetParent(this.characterPoolList.transform);
            //genericPool = new GenericPool<PoolableObject>();

            genericPool = new GenericPool<PoolableObject>(_characterContainer.sAddCount, _characterContainer.sAllowCount, () =>
            {
                PoolableObject pObj = Instantiate(_characterContainer.sPrefab.GetComponent<PoolableObject>());
                var chacaterObject = pObj.GetComponent<CharacterObject>();
                chacaterObject.SetupStatsData(_characterContainer.sCharacterStatsData);
                pObj.Create(genericPool, emptyObject.transform);
                return pObj;

            });

            genericPool.Allocate();
            characterDic.Add(_containerId, genericPool);

        }
        return genericPool.PopObject();
    }
    #endregion




    #region Projectile

    public PoolableObject PopProjectileObject(string _containerId)
    {
        GenericPool<PoolableObject> genericPool = null;
        bool cached = projectileDic.TryGetValue(_containerId, out genericPool);
        //캐시되지 않았다면, 최초로 생성.
        if (!cached)
        {
            var _projectileContainer = DataContainer.Instance.GetProjectileContainerByContainerId(_containerId);
            GameObject emptyObject = new GameObject("Projectile "+_containerId + " Pool");  //빈객체 생성 후 해당오브젝트의 자식으로 이동
            emptyObject.transform.SetParent(this.projectilePoolList);
            genericPool = new GenericPool<PoolableObject>(_projectileContainer.sAllowCount, _projectileContainer.sAddCount, () =>
            {
                PoolableObject pObj = Instantiate(_projectileContainer.sPrefab.GetComponent<PoolableObject>());
                var chacaterObject = pObj.GetComponent<ProjectileObject>();
                chacaterObject.SetupData(_projectileContainer.sProjectileData);
                pObj.Create(genericPool, emptyObject.transform);
                return pObj;
            });


            genericPool.Allocate();
            projectileDic.Add(_containerId, genericPool);
        }
        return genericPool.PopObject();
    }
    #endregion

    #region EffectObeject

    public PoolableObject PopEffectObject(EffectType effectType)
    {
        GenericPool<PoolableObject> genericPool = null;
        bool cached = effectDic.TryGetValue(effectType, out genericPool);

        //캐시되지 않았다면, 최초로 생성.
        if (!cached)
        {
            var effectContainer = DataContainer.Instance.GetEffectContainerByEffectType(effectType);
            GameObject emptyObject = new GameObject("Effect "+effectType.ToString() + " Pool");  //빈객체 생성 후 해당오브젝트의 자식으로 이동
            emptyObject.transform.SetParent(this.effectPoolList);

            genericPool = new GenericPool<PoolableObject>(effectContainer.sAllowCount, effectContainer.sAddCount, () =>
            {
                var pObj = Instantiate(effectContainer.sPrefab).GetComponent<EffectObject>();
                pObj.Initalize(effectContainer);
                pObj.Create(genericPool, emptyObject.transform);
                return pObj;
            });

            genericPool.Allocate();
            effectDic.Add(effectType, genericPool);
        }


        return genericPool.PopObject();
    }
    #endregion

}
