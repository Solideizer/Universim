using UnityEngine;

namespace Procedural_Generation
{
    public class ObjectAligner : MonoBehaviour
    {
        #region Variable Declarations
#pragma warning disable 0649
        [SerializeField] GameObject randomObjectSpawnerPrefab;
        [SerializeField] public float raycastDistance = 100f;
        [SerializeField] private float overlapTestBoxSize = 5f;
#pragma warning restore 0649
        #endregion

        private int prefabLayer;
        private int terrainLayer;

        private void Start ()
        {
            RaycastObjectAligner ();
            prefabLayer = randomObjectSpawnerPrefab.layer;
            terrainLayer = LayerMask.GetMask("Terrain");
        }

        private void RaycastObjectAligner ()
        {
            RaycastHit hitInfo;
            if (Physics.Raycast (transform.position, Vector3.down, out hitInfo, raycastDistance))
            {
                Quaternion spawnRot = Quaternion.FromToRotation (Vector3.up, hitInfo.normal);
                //overlap avoidance
                Vector3 overlapTestBoxScale = new Vector3 (overlapTestBoxSize, overlapTestBoxSize, overlapTestBoxSize);
                Collider[] collidersInsideOverlapBox = new Collider[2];
                Physics.OverlapBoxNonAlloc (hitInfo.point, overlapTestBoxScale, collidersInsideOverlapBox, spawnRot);

                bool found = false;
                for (var i = 0; i < collidersInsideOverlapBox.Length; i++)
                {
                    if (collidersInsideOverlapBox[i] != null && collidersInsideOverlapBox[i].gameObject.layer != terrainLayer && 
                            collidersInsideOverlapBox[i].gameObject.layer == prefabLayer)
                        found = true;
                }

                if(!found)
                    Instantiate (randomObjectSpawnerPrefab, hitInfo.point, spawnRot);

                GameObject.Destroy(this.gameObject);
            }
            
        }
    }
}