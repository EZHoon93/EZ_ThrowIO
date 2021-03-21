using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    //[SerializeField]
    //protected PoolableObject poolobj;
    //[SerializeField]
    //protected int allocateCount;
    //[SerializeField]    
    //protected Stack<PoolableObject> stack = new Stack<PoolableObject>();
    ////protected Stack<PoolableObject> useStack = new Stack<PoolableObject>();



    //protected virtual void Start()
    //{
    //    Allocate();
    //}

    //private void Update()
    //{

    //}
    //public virtual void Allocate( )
    //{
    //    for(int i =0; i < allocateCount; i++)
    //    {
    //        PoolableObject tObj = Instantiate(poolobj, this.transform);
    //        tObj.Create(this);
    //        stack.Push(tObj);
    //    }
    //}

    //public virtual GameObject PopObject()
    //{
    //    //비어있다면 하나 생성
    //    if (stackEmpty())
    //    {
    //        PoolableObject tObj = Instantiate(poolobj, this.transform);
    //        tObj.Create(this);
    //        tObj.gameObject.SetActive(true);
    //        return tObj.gameObject;
    //    }
    //    else
    //    {
    //        PoolableObject obj = stack.Pop();
    //        obj.gameObject.SetActive(true);
    //        return obj.gameObject;
    //    }
        
    //}

   

    //public virtual PoolableObject Test()
    //{
    //    return null;
    //}
    //public virtual void PushObject (PoolableObject obj)
    //{
    //    if (ObjectPoolManger.Instance == null) return;
    //    obj.gameObject.SetActive(false);
    //    obj.gameObject.transform.SetParent(this.transform);
    //    stack.Push(obj);
    //}

    //public virtual bool stackEmpty()
    //{
    //    if (stack.Count > 0)
    //    {
    //        return false;
    //    }
    //    else
    //    {
    //        return true;
    //    }
    //}

   
    
}
