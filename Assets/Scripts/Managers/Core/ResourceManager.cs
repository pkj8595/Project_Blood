using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : ManagerBase
{
   
    public T Load<T>(string path) where T : Object
    {
        //1. original를 이미 들고 있으면 바로 사용
        if (typeof(T) == typeof(GameObject))
        {
            string name = path;
            int index = name.LastIndexOf("/");
            if (index >= 0)
                name = name.Substring(index + 1);

            GameObject go = Managers.Pool.GetOriginal(name);
            if (go != null)
                return go as T;
        }

        return Resources.Load<T>(path);
    }

    public GameObject Instantiate(string path, Transform parent = null)
    {
        GameObject original = Load<GameObject>($"Prefabs/{path}");
        if(original == null)
        {
            Debug.Log($"Faild to load prefab {path}");
            return null;
        }

        //2. 혹시 풀링된 애가 있을까?
        if (original.GetComponent<Poolable>() != null)
            return Managers.Pool.Pop(original, parent).gameObject;

        GameObject go = Object.Instantiate(original, parent);
        go.name = original.name;
        return go;
    }

    public void Destory(GameObject go)
    {
        if (go == null)
            return;

        //pool이 필요한 오브젝트면 풀링에 저장
        Poolable poolable = go.GetComponent<Poolable>();
        if(poolable != null)
        {
            Managers.Pool.Push(poolable);
            return;
        }

        Object.Destroy(go);
    }

   
}
