using UnityEngine;

public class RandomObjectSpawner : MonoBehaviour
{
    #region Variable Declarations
#pragma warning disable 0649
    [SerializeField] private GameObject[] objects;
    [SerializeField] private Vector2 scaleRange = new Vector2 (1.0f, 5.0f);
#pragma warning restore 0649
    #endregion

    private void Start ()
    {
        PickRandomObject ();
    }

    private void PickRandomObject ()
    {
        var randomIndex = UnityEngine.Random.Range (0, objects.Length);

        GameObject clone = Instantiate (objects[randomIndex], transform.position, transform.rotation);

        // Apply scaling.
        float scale = UnityEngine.Random.Range (scaleRange.x, scaleRange.y);
        clone.transform.localScale = Vector3.one * scale;
    }
}