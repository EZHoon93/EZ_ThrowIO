using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GenericPool<T>  where T : PoolableObject
{
    private int allocateCount;
    private int addCount;

    public delegate T Initializer();
    private Initializer initializer;

    public Stack<T> objStack;
    public List<T> objList;

    //public T prefab;

    public GenericPool()
    {
        // default constructor
    }


    public GenericPool(int ac, int _addCount,  Initializer fn)
    {
        this.allocateCount = ac;
        this.addCount = _addCount;
        this.initializer = fn;
        this.objStack = new Stack<T>();
        this.objList = new List<T>();
    }

    public void Allocate()
    {
        for (int index = 0; index < this.allocateCount; ++index)
        {
            this.objStack.Push(this.initializer());
        }
    }

    public T PopObject()
    {
        if (this.objStack.Count <= 0)
        {
            for (int index = 0; index < addCount; ++index)
            {
                this.objStack.Push(this.initializer());
            }
        }

        T obj = this.objStack.Pop();
        this.objList.Add(obj);
        obj.transform.SetParent(null);
        
        obj.gameObject.SetActive(true);

        return obj;
    }

    public void PushObject(T obj)
    {
        obj.gameObject.SetActive(false);
        this.objList.Remove(obj);
        this.objStack.Push(obj);
    }

    public void Dispose()
    {
        if (this.objStack == null || this.objList == null)
            return;

        //this.objList.ForEach(obj => this.objStack.Push(obj));

        //while (this.objStack.Count > 0)
        //{
        //    GameObject.Destroy(this.objStack.Pop());
        //}
        if (objList.Count == 0) return;
        var count = objList.Count;
        while(objList.Count > 0)
        {
            for (int i = 0; i < objList.Count; i++)
            {
                objList[i].Push();
            }
        }
        

        //this.objList.Clear();
        //this.objStack.Clear();
    }



}
