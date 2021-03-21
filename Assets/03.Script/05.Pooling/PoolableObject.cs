using System;

using UnityEngine;

public class PoolableObject : MonoBehaviour
{
    protected GenericPool<PoolableObject> Pool;
    [HideInInspector] public Transform poolPanel;

    public virtual void Create(GenericPool<PoolableObject> pool , Transform panel)
    {
        Pool = pool;
        poolPanel = panel;
        this.transform.SetParent(poolPanel);
        gameObject.SetActive(false);
    }

    public virtual void Push()
    {
        this.transform.SetParent(poolPanel);
        this.transform.localScale = new Vector3(1, 1, 1);
        Pool.PushObject(this);
    }

  
    

}
