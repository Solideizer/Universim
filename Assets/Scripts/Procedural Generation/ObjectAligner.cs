using UnityEngine;

public class ObjectAligner : MonoBehaviour
{
    #region Variable Declarations
#pragma warning disable 0649
    [SerializeField] GameObject randomObjectSpawnerPrefab;
    [SerializeField] public float raycastDistance = 100f;
    [SerializeField] private LayerMask spawnedObjectLayer;
    [SerializeField] private float overlapTestBoxSize = 5f;
#pragma warning restore 0649
    #endregion

    private void Start ()
    {
        RaycastObjectAligner ();
    }
    private void RaycastObjectAligner ()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast (transform.position, Vector3.down, out hitInfo, raycastDistance))
        {
            Quaternion spawnRot = Quaternion.FromToRotation (Vector3.up, hitInfo.normal);
            //overlap avoidance
            Vector3 overlapTestBoxScale = new Vector3 (overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
            Collider[] collidersInsideOverlapBox = new Collider[1];
            int numberOfCollidersFound = Physics.OverlapBoxNonAlloc (hitInfo.point, overlapTestBoxScale, collidersInsideOverlapBox,
                spawnRot, spawnedObjectLayer);

            if (numberOfCollidersFound == 0)
            {
                Instantiate (randomObjectSpawnerPrefab, hitInfo.point, spawnRot);
            }
        }
        GameObject.Destroy (this.gameObject);
    }
}