using System;
using UnityEngine;

[Serializable]
public class Identity 
{
#pragma warning disable 0649
    [SerializeField] Genetic genetic;
#pragma warning restore 0649

    public bool canReproduce;

    private Sex sex;
    private float vision;
    private Speed speed;
    private int fertility;
    private Memory memory;
    private Vector3 scale;

    public Sex Sex { get => sex; }
    public float Vision { get => vision; }
    public Speed Speed { get => speed; }
    public int Fertility { get => fertility; }
    public Memory Memory { get => memory; }
    public Vector3 Scale { get => scale; }
    public Genetic GeneticCode { get => genetic; set => genetic = value; }

    public Identity(bool isBaby, Genetic genetic)
    {
        if(isBaby)
            canReproduce = false;
        else
            canReproduce = true;
        
        this.genetic = genetic;
        SetGenes();
    }

    private void SetGenes()
    {
        SetSex();
        SetVision();
        SetSpeed();
        SetFertility();
        SetMemory();
        SetScale();
    }

    private void SetSex()
    {
        sex = Genetic.GetSex();
    }

    private void SetVision()
    {
        vision = GeneticDataManager.Instance.vision[genetic.vision];
    }

    private void SetSpeed()
    {
        // Burada ki speed'i struct gibi düşündüm. Kaçarken farklı bir hız gezerken farklı bir hıza böylelikle sahip olabilecek.
        speed = GeneticDataManager.Instance.speed[genetic.speed];
    }

    private void SetFertility()
    {
        // Pregnancy case'e bir event gönderilmesi şeklinde olacak.
        fertility = GeneticDataManager.Instance.fertility[genetic.fertility];
    }

    private void SetMemory()
    {
        // Aşağıdaki kullanım yapılmalı.
        // animalAI.memory = new Memory(2, 2);
        int count = GeneticDataManager.Instance.memory[genetic.memory];
        memory = new Memory(count, count);
    }

    private void SetScale()
    {
        // Growth Multiplier üzerinen değişiklik gerekli.
        // Transform üzerinden değişiklik
        scale = GeneticDataManager.Instance.scale[genetic.scale];
    }
}

public enum Sex { MALE, FEMALE }
