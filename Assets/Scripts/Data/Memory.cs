using System.Collections.Generic;
using System.Linq;
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
        Queue<Transform> queue = memoryType == MemoryType.WATER ? waters : foods;

        int count = queue.Count;
        if (count == 0)
            return null;
        else if (count == 1)
            return queue.Peek();
        else
            return FindNearest(position, queue);
    }

    private Transform FindNearest(Vector3 position, Queue<Transform> queue)
    {
        List<Transform> transforms = new List<Transform>(queue);
        transforms = transforms.OrderBy(
            x => Vector3.Distance(position, x.transform.position)
        ).ToList();
        return transforms.First();
    }

}

