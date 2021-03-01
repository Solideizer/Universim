using UnityEngine;

public class Entity : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Properties properties;
#pragma warning restore 0649

    private int memorySize;

    public int MemorySize { get => memorySize; set => memorySize = value; }

    private void Start ()
    {
        memorySize = properties.memorySize;
    }

}