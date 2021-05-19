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

    public static Genetic Cross(Genetic gen1, Genetic gen2)
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

    private static byte GetByte(byte bit1, byte bit2)
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

    public static Sex GetSex()
    {
        byte bit = GetByte(0, 1);
        return (Sex)bit;
    }

    public static Genetic GetRandomizedGene()
    {
        Genetic genetic = new Genetic();
        genetic.sex = (byte)UnityEngine.Random.Range(0, 2);
        genetic.vision = (byte)UnityEngine.Random.Range(0, GeneticDataManager.Instance.vision.Length);
        genetic.speed = (byte)UnityEngine.Random.Range(0, GeneticDataManager.Instance.speed.Length);
        genetic.fertility = (byte)UnityEngine.Random.Range(0, GeneticDataManager.Instance.fertility.Length);
        genetic.memory = (byte)UnityEngine.Random.Range(0, GeneticDataManager.Instance.memory.Length);
        genetic.scale = (byte)UnityEngine.Random.Range(0, GeneticDataManager.Instance.scale.Length);

        return genetic;
    }
}
