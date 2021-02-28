using UnityEngine;

public class Entity : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private Properties properties;
#pragma warning restore 0649

    private string entityName;

    public string EntityName { get => entityName; set => entityName = value; }

    private void Start ()
    {
        entityName = properties._name;
    }

}