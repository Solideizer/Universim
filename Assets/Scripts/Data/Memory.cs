using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Memory
{
    public enum MemoryType{ WATER, FOOD }

    private Queue<Transform> waters;
    private Queue<Transform> foods;

    private readonly int waterCount;
    private readonly int foodCount;

    public Memory(int waterCount, int foodCount)
    {
        this.waterCount = waterCount;
        this.foodCount = foodCount;

        waters = new Queue<Transform>();
        foods = new Queue<Transform>();
    }

    public void FillMemory(Transform location, MemoryType memoryType)
    {
        if(memoryType == MemoryType.WATER)
        {
            if(waters.Count > waterCount)
                waters.Dequeue();
            
            waters.Enqueue(location);
        }
        else
        {
            if(foods.Count > foodCount)
                foods.Dequeue();

            foods.Enqueue(location);
        }
    }

    public Transform GetPoint(Vector3 position, MemoryType memoryType)
    {
        int count = waters.Count;
        if (count == 0)
            return null;
        else if (count == 1)
            return waters.Peek();
        else
            return FindNearest(position);
    }

    private Transform FindNearest(Vector3 position)
    {
        var item1 = waters.Dequeue();
        var item2 = waters.Dequeue();

        waters.Enqueue(item1);
        waters.Enqueue(item2);

        var d1 = Vector3.Distance(item1.position, position);
        var d2 = Vector3.Distance(item2.position, position);

        return d1 > d2 ? item2 : item1;
    }
}

