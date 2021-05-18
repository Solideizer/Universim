using System;
using UnityEngine;

[Serializable]
public class Identity 
{   
    public Genetic geneticCode;

    public Sex sex;
    public float vision;
    public Speed speed;
    public int fertility;
    public Memory memory;
    public Vector3 scale;

    public bool canReproduce;

    public Identity(Sex sex, bool isBaby)
    {
        this.sex = sex;
        if(isBaby)
            canReproduce = false;
        else
            canReproduce = true;
        
    }

    public void SetSex(AnimalAI animalAI)
    {
        sex = (Sex)geneticCode.sex;
    }

    public void SetVision(AnimalAI animalAI)
    {
        vision = GeneticDataManager.Instance.vision[geneticCode.vision];
    }

    public void SetSpeed(AnimalAI animalAI)
    {
        // Burada ki speed'i struct gibi düşündüm. Kaçarken farklı bir hız gezerken farklı bir hıza böylelikle sahip olabilecek.
        speed = GeneticDataManager.Instance.speed[geneticCode.speed];
    }

    public void SetFertility(AnimalAI animalAI)
    {
        // Pregnancy case'e bir event gönderilmesi şeklinde olacak.
        fertility = GeneticDataManager.Instance.fertility[geneticCode.fertility];
    }

    public void SetMemory(AnimalAI animalAI)
    {
        // Aşağıdaki kullanım yapılmalı.
        // animalAI.memory = new Memory(2, 2);
        int count = GeneticDataManager.Instance.memory[geneticCode.memory];
        memory = new Memory(count, count);
    }

    public void SetScale(AnimalAI animalAI)
    {
        // Transform üzerinden değişiklik
        scale = GeneticDataManager.Instance.scale[geneticCode.scale];
    }
}

public enum Sex { MALE, FEMALE }
