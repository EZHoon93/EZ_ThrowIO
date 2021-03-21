using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TestGenericObject<T> : MonoBehaviour 
{

    public T Pool;
    

    public virtual void Create(T pool)
    {
        Pool = pool;
        
    }

    public virtual void Push()
    {
        //Pool.PushObject(thisObject);
    }

}
