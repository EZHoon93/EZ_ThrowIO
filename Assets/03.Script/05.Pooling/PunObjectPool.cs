using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunObjectPool : MonoBehaviour
{
    public GameObject prefabObject;    //프리팹 정보
    public List<GameObject> initGameObjects;//초기생성된 오브젝트들
    public Stack<GameObject> stack;


    public void IniailizePool()
    {
        stack = new Stack<GameObject>();
        for (int i =0; i< initGameObjects.Count; i++)
        {
            stack.Push(initGameObjects[i]);
        }
    }
    public void SetUpPool(GameObject _prefabObject)
    {
        prefabObject = _prefabObject;
        
    }

    public GameObject Pop()
    {
        if(this.stack.Count <= 0)
        {
            prefabObject.SetActive(false);
            var result = Instantiate(prefabObject);
            result.gameObject.name = prefabObject.name;
            result.transform.SetParent(this.transform);
            
            prefabObject.SetActive(true);

            return result;
        }
        return stack.Pop();
    }
}
