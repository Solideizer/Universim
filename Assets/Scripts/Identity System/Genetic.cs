using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Genetic
{
    public byte sex;
    public byte vision;
    public byte speed;
    public byte fertility;
    public byte memory;
    public byte scale;

    public void SetGen(AnimalAI child, AnimalAI parent1, AnimalAI parent2)
    {
        Genetic newGen = Cross(parent1.genetic, parent2.genetic);

        sex = newGen.sex;
        vision = newGen.vision;
        speed = newGen.speed;
        fertility = newGen.fertility;
        memory = newGen.memory;
        scale = newGen.scale;

        SetSex(child);
        SetVision(child);
        SetSpeed(child);
        SetFertility(child);
        SetMemory(child);
        SetScale(child);
    }

    #region CROSSOVER

    public Genetic Cross(Genetic gen1, Genetic gen2)
    {
        Genetic newGen = new Genetic();

        byte _sex = GetByte(gen1.sex, gen2.sex);
        byte _vision = GetByte(gen1.vision, gen2.vision);
        byte _speed = GetByte(gen1.speed, gen2.speed);
        byte _fertility = GetByte(gen1.fertility, gen2.fertility);
        byte _memory = GetByte(gen1.memory, gen2.memory);
        byte _scale = GetByte(gen1.scale, gen2.scale);

        return newGen;
    }

    private byte GetByte(byte bit1, byte bit2)
    {
        byte bit = bit1;
        if(bit1 != bit2)
        {
            byte[] arr = { bit1, bit2 };
            int number = UnityEngine.Random.Range(0, arr.Length);
            bit = arr[number];
        }

        return bit;
    }

    #endregion

    #region CONFIGURE METHODS

    public void SetSex(AnimalAI animalAI)
    {
        animalAI.AnimalIdentity.sex = (Sex)sex;
    }

    public void SetVision(AnimalAI animalAI)
    {
        // AnimalAI'da vision ayarlanır ve diğer case'lere haber gider.
    }

    public void SetSpeed(AnimalAI animalAI)
    {
        // Burada ki speed'i struct gibi düşündüm. Kaçarken farklı bir hız gezerken farklı bir hıza böylelikle sahip olabilecek.
    }

    public void SetFertility(AnimalAI animalAI)
    {
        // Pregnancy case'e bir event gönderilmesi şeklinde olacak.
    }

    public void SetMemory(AnimalAI animalAI)
    {
        // Aşağıdaki kullanım yapılmalı.
        // animalAI.memory = new Memory(2, 2);
    }

    public void SetScale(AnimalAI animalAI)
    {
        // Transform üzerinden değişiklik
    }

    #endregion
}
