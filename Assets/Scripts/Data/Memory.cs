using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Memory
{
    public enum MemoryType{ WATER, FOOD }

    private List<Transform> waters;
    private List<Transform> foods;

    private readonly int waterCount;
    private readonly int foodCount;

    public Memory(int waterCount, int foodCount)
    {
        this.waterCount = waterCount;
        this.foodCount = foodCount;

        waters = new List<Transform>();
        foods = new List<Transform>();
    }

    public Transform CompareLocations(Vector3 position, Transform target, MemoryType memoryType)
    {
        if(memoryType == MemoryType.WATER)
        {
            waters = SetList(waters, position, target, waterCount);
            return waters.First();
        }
        else
        {
            foods = SetList(foods, position, target, foodCount);
            return foods.First();
        }
    }

    public Transform GetNearest(Vector3 position, MemoryType memoryType)
    {
        if(memoryType == MemoryType.WATER)
        {
            if(waters.Count == 0) return null;

            waters = Sort(waters, position);
            return waters.First();
        }
        else
        {
            if(foods.Count == 0) return null;

            foods = Sort(foods, position);
            return foods.First();
        }
    }

    private List<Transform> SetList(List<Transform> transforms, Vector3 position, Transform target, int maxCount)
    {
        if (!transforms.Contains(target))
            transforms.Add(target);
        // sort
        transforms = Sort(transforms, position);

        if (transforms.Count > maxCount)
            transforms.RemoveAt(transforms.Count - 1);

        return transforms;
    }

    private List<Transform> Sort(List<Transform> transforms, Vector3 position)
    {
        transforms = transforms.OrderBy(
                    x => Vector3.Distance(position, x.transform.position)
                ).ToList();
        return transforms;
    }

}

