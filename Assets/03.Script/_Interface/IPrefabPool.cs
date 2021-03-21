
using UnityEngine;

public interface IPrefabPool<T> 
{
    void Allocate(int count, T poolablePrefab);

    T PopObject();
    void PushObject(T Object);


}
