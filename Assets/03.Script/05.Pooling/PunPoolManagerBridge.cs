
using System.Collections.Generic;

using Photon.Pun;
using UnityEngine;



public  class PunPoolManagerBridge : MonoBehaviour , IPunPrefabPool
{
    
    
    public void Setup()
    {
        PhotonNetwork.PrefabPool = this;
    }
 
    public GameObject Instantiate(string prefabId, Vector3 position, Quaternion rotation)
    {

        var gameObject = ObjectPoolManger.Instance.PunPop(prefabId);
        gameObject.transform.position = position;
        gameObject.transform.rotation = rotation;
        gameObject.SetActive(false);

        return gameObject;
    }

    public void Destroy(GameObject gameObject)
    {
        if (ObjectPoolManger.Instance == null) return;  //싱글톤이존재해야 가능
        string prefabId = gameObject.name;
        PunObjectPool punObjectPool = null;
        bool cached = ObjectPoolManger.Instance.punDic.TryGetValue(prefabId, out punObjectPool);
        if (cached)
        {
            punObjectPool.stack.Push(gameObject);
            gameObject.transform.SetParent(punObjectPool.transform);
            var phtonView = gameObject.GetComponent<PhotonView>();
            if (phtonView)
            {
                var ionDestroy = gameObject.GetComponent<IOnPhotonViewPreNetDestroy>();
                if (ionDestroy != null)
                {
                    ionDestroy.OnPreNetDestroy(phtonView);
                }

            }
        }





    }


}