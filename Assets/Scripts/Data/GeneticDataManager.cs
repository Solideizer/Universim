using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticDataManager : MonoBehaviour
{
    private static GeneticDataManager instance;
    public static GeneticDataManager Instance { get => instance; }

    public float[] vision;
    public Speed[] speed;
    public int[] fertility;
    public int[] memory;
    public Vector3[] scale;

    private void Awake() 
    {
        instance = this;    
    }
}
