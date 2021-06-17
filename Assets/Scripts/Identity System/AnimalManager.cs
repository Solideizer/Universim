using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    private static AnimalManager instance;
    public static AnimalManager Instance { get => instance; }

    public Dictionary<int, AnimalAI> animals = new Dictionary<int, AnimalAI>();
    
#pragma warning disable 0649
    [SerializeField] AnimalAI herbivorCub;
    [SerializeField] AnimalAI carnivoreCub;
    [SerializeField] int herbivoreCount = 100;
    [SerializeField] int carnivoreCount = 20;
#pragma warning restore 0649

    public Vector3 chickSize = new Vector3(2, 2, 2);
    public Vector3 cubSize = new Vector3(2, 2, 2);

    public ObjectPool<AnimalAI> herbivorePool;
    public ObjectPool<AnimalAI> carnivorePool;
    
    private void Awake()
    {
        instance = this;

        herbivorePool = new ObjectPool<AnimalAI>();
        carnivorePool = new ObjectPool<AnimalAI>();

        FillPool(herbivorePool, herbivorCub, herbivoreCount);
        FillPool(carnivorePool, carnivoreCub, carnivoreCount);
    }

    private void FillPool(ObjectPool<AnimalAI> pool, AnimalAI ai, int count)
    {
        pool.SetObject(ai);
        pool.Fill(count);
    }

    public AnimalAI GetHerbivore(Vector3 pos)
    {
        var chick = herbivorePool.Pop();
        float range = UnityEngine.Random.Range(-0.5f, 0.5f);
        chick.Warp(new Vector3(pos.x - range, pos.y, pos.z - range));
        return chick;
    }

    public AnimalAI GetCarnivore(Vector3 pos)
    {
        var cub = carnivorePool.Pop();
        float range = UnityEngine.Random.Range(-0.5f, 0.5f);
        cub.Warp(new Vector3(pos.x - range, pos.y, pos.z - range));
        return cub;
    }
}
