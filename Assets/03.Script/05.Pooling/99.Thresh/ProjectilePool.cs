using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectilePool : MonoBehaviour
{
    [SerializeField]
    protected ProjectileObject poolobj;

    [SerializeField]
    public Stack<ProjectileObject> stack = new Stack<ProjectileObject>();
    public void Allocate(int count, ProjectileObject poolablePrefab)
    {
        poolobj = poolablePrefab;
        for (int i = 0; i < count; i++)
        {
            var tObj = Instantiate(poolablePrefab);
            //tObj.Create(this);
            PushObject(tObj);
        }
    }

    public ProjectileObject PopObject()
    {
        //비어있다면 하나 생성
        if (stackEmpty())
        {
            print("비어있음 생성");
            //2개 생성
            Allocate(2, poolobj);
            return PopObject();
        }
        else
        {
            ProjectileObject obj = stack.Pop();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }

    }


    public void PushObject(ProjectileObject obj)
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
