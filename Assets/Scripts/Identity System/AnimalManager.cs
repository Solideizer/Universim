using System;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    private static AnimalManager instance;
    public static AnimalManager Instance { get => instance; }

    public Dictionary<int, AnimalAI> animals = new Dictionary<int, AnimalAI>();

    [SerializeField] AnimalAI herbivorCub;
    [SerializeField] AnimalAI carnivoreCub;
    [SerializeField] int herbivoreCount = 100;
    [SerializeField] int carnivoreCount = 20;

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

    public void GetHerbivore(Vector3 pos)
    {
        var chick = herbivorePool.Pop();
        chick.transform.position = new Vector3(pos.x - 0.5f, pos.y, pos.z - 0.5f);
    }
}
