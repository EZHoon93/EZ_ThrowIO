using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowPool : MonoBehaviour , IPrefabPool<ThrowObject>
{
    [SerializeField]
    protected ThrowObject poolobj;
    
    [SerializeField]
    public Stack<ThrowObject> stack = new Stack<ThrowObject>();
    public void Allocate(int count, ThrowObject poolablePrefab)
    {
        poolobj = poolablePrefab;
        for (int i = 0; i < count; i++)
        {
            var tObj = Instantiate(poolablePrefab);
            tObj.Create(this);
            PushObject(tObj);
        }
    }

    public ThrowObject PopObject()
    {
        //비어있다면 하나 생성
        if (stackEmpty())
        {
            //2개 생성
            Allocate(2, poolobj);
            return PopObject();
        }
        else
        {
            ThrowObject obj = stack.Pop();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }

    }


    public void PushObject(ThrowObject obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(this.transform);
        stack.Push(obj);
    }

    public virtual bool stackEmpty()
    {
        if (stack.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
