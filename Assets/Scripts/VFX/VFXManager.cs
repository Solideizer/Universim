using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VFXType { HUNGER, THIRST, LOVE, HUNGERDEAD, THIRSTDEAD, EATDEAD }

public class VFXManager : MonoBehaviour
{
    private static VFXManager instance;
    public static VFXManager Instance { get => instance; }

    [SerializeField] VFXScript lovePrefab;
    [SerializeField] VFXScript hungerPrefab;
    [SerializeField] VFXScript thirstPrefab;
    [SerializeField] VFXScript hungerDeadPrefab;
    [SerializeField] VFXScript thirstDeadPrefab;
    [SerializeField] VFXScript eatDeadPrefab;
    [SerializeField] int vfxCount = 500;

    public Vector3 vfxSize = new Vector3(0.5f, 0.5f, 0.5f);

    public ObjectPool<VFXScript> lovePool;
    public ObjectPool<VFXScript> hungerPool;
    public ObjectPool<VFXScript> thirstPool;
    public ObjectPool<VFXScript> hungerDeadPool;
    public ObjectPool<VFXScript> thirstDeadPool;
    public ObjectPool<VFXScript> eatDeadPool;

    private void Awake()
    {
        instance = this;

        lovePool = new ObjectPool<VFXScript>();
        hungerPool = new ObjectPool<VFXScript>();
        thirstPool = new ObjectPool<VFXScript>();
        hungerDeadPool = new ObjectPool<VFXScript>();
        thirstDeadPool = new ObjectPool<VFXScript>();
        eatDeadPool = new ObjectPool<VFXScript>();

        FillPool(lovePool, lovePrefab, vfxCount);
        FillPool(hungerPool, hungerPrefab, vfxCount);
        FillPool(thirstPool, thirstPrefab, vfxCount);
        FillPool(hungerDeadPool, hungerDeadPrefab, vfxCount);
        FillPool(thirstDeadPool, thirstDeadPrefab, vfxCount);
        FillPool(eatDeadPool, eatDeadPrefab, vfxCount);
    }

    private void FillPool(ObjectPool<VFXScript> pool, VFXScript vfx, int count)
    {
        pool.SetObject(vfx);
        pool.Fill(count);
    }

    public VFXScript GetStateVFX(Vector3 pos, AnimalAI ai, VFXType vfxType)
    {
        VFXScript vfx = null;
        switch (vfxType)
        {
            case VFXType.HUNGER:
                vfx = hungerPool.Pop();
                break;
            case VFXType.THIRST:
                vfx = thirstPool.Pop();
                break;
            case VFXType.LOVE:
                vfx = lovePool.Pop();
                break;
        }

        if (vfx == null) return null;

        float up = 6;
        vfx.transform.parent = ai.transform;
        vfx.transform.position = new Vector3(pos.x, pos.y + up, pos.z);
        return vfx;
    }

    public VFXScript GetDeadVFX(Vector3 pos, AnimalAI ai, VFXType vfxType)
    {
        VFXScript vfx = null;
        switch (vfxType)
        {
            case VFXType.HUNGERDEAD:
                vfx = hungerDeadPool.Pop();
                break;
            case VFXType.THIRSTDEAD:
                vfx = thirstDeadPool.Pop();
                break;
            case VFXType.EATDEAD:
                vfx = eatDeadPool.Pop();
                break;
        }

        if (vfx == null) return null;

        float up = 3;
        vfx.transform.position = new Vector3(pos.x, pos.y + up, pos.z);
        Debug.Log("var edicem");
        return vfx;
    }

    public void Push(VFXScript vfx, VFXType vfxType)
    {
        switch (vfxType)
        {
            case VFXType.HUNGER:
                hungerPool.Push(vfx);
                break;
            case VFXType.THIRST:
                thirstPool.Push(vfx);
                break;
            case VFXType.LOVE:
                lovePool.Push(vfx);
                break;
            case VFXType.HUNGERDEAD:
                hungerDeadPool.Push(vfx);
                break;
            case VFXType.THIRSTDEAD:
                thirstDeadPool.Push(vfx);
                break;
            case VFXType.EATDEAD:
                eatDeadPool.Push(vfx);
                break;
        }
    }
    public IEnumerator WaitAndPush(VFXScript vfx, VFXType vfxType)
    {
        Debug.Log("yok edicem");
        yield return new WaitForSeconds(1);
        Push(vfx, vfxType);
        Debug.Log("yok edebildim mi");
    }
}
