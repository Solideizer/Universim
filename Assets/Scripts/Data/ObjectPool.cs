using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private T prefab;
    private Stack<T> objectPool = new Stack<T>();

    public void SetObject(T prefab)
    {
        this.prefab = prefab;
    }

    public int GetLength()
    {
        return objectPool.Count;
    }

    public void Fill(int count)
    {
        for (int i = 0; i < count; i++)
        {
            T obj = Object.Instantiate(prefab) as T;
            obj.gameObject.SetActive(false);

            Push(obj);
        }
    }

    public T Pop()
    {
        T obj;
        if (objectPool.Count > 0)
            obj = objectPool.Pop();
        else
            obj = Object.Instantiate(prefab) as T;
            
        obj.gameObject.SetActive(true);
        return obj;
    }

    public void Push(T obj)
    {
        if (obj != null)
        {
            objectPool.Push(obj);
            obj.gameObject.SetActive(false);
        }
            
    }

    public bool IsObjectNull()
    {
        return prefab == null;
    }
}
