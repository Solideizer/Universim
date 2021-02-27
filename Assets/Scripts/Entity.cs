using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] Properties properties;

    private string entityName;

    public string EntityName { get => entityName; set => entityName = value; }

    private void Start() 
    {
        entityName = properties._name;    
    }

}
