using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    private static VFXManager instance;
    public static VFXManager Instance { get => instance; }

    [SerializeField] VFXScript love;
    [SerializeField] VFXScript hunger;
    [SerializeField] VFXScript thirst;
    [SerializeField] int vfxCount = 500;

    public Vector3 vfxSize = new Vector3(0.5f, 0.5f, 0.5f);

    public ObjectPool<VFXScript> lovePool;
    public ObjectPool<VFXScript> hungerPool;
    public ObjectPool<VFXScript> thirstPool;

    private void Awake()
    {
        instance = this;

        lovePool = new ObjectPool<VFXScript>();
        hungerPool = new ObjectPool<VFXScript>();
        thirstPool = new ObjectPool<VFXScript>();

        FillPool(lovePool, love, vfxCount);
        FillPool(hungerPool, hunger, vfxCount);
        FillPool(thirstPool, thirst, vfxCount);
    }

    private void FillPool(ObjectPool<VFXScript> pool, VFXScript vfx, int count)
    {
        pool.SetObject(vfx);
        pool.Fill(count);
    }

    public VFXScript GetLove(Vector3 pos, AnimalAI ai)
    {
        var love = lovePool.Pop();
        float up = 6;
        love.transform.parent = ai.transform;
        love.transform.position = new Vector3(pos.x, pos.y + up, pos.z);
        return love;
    }

    public VFXScript GetHunger(Vector3 pos, AnimalAI ai)
    {
        var hunger = hungerPool.Pop();
        float up = 6;
        hunger.transform.parent = ai.transform;
        hunger.transform.position = new Vector3(pos.x, pos.y + up, pos.z);
        return hunger;
    }

    public VFXScript GetThirst(Vector3 pos, AnimalAI ai)
    {
        var thirst = thirstPool.Pop();
        float up = 6;
        thirst.transform.parent = ai.transform;
        thirst.transform.position = new Vector3(pos.x, pos.y + up, pos.z);
        return thirst;
    }
}
