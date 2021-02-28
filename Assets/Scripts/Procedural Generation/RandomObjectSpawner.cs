using UnityEngine;

namespace Procedural_Generation
{
    public class RandomObjectSpawner : MonoBehaviour
    {
        #region Variable Declarations
#pragma warning disable 0649
        [SerializeField] private GameObject[] objects;
        [SerializeField] private Vector2 scaleRange = new Vector2 (1.0f, 5.0f);
#pragma warning restore 0649
        private GameObject _parentObject;
        #endregion

        private void Start ()
        {
            _parentObject = GameObject.FindGameObjectWithTag ("Procedural Gen Object Holder");
            PickRandomObject ();
        }

        private void PickRandomObject ()
        {
            var randomIndex = UnityEngine.Random.Range (0, objects.Length);

            GameObject clone = Instantiate (objects[randomIndex], transform.position, transform.rotation);
            clone.transform.SetParent (_parentObject.transform);

            // Apply scaling.
            float scale = UnityEngine.Random.Range (scaleRange.x, scaleRange.y);
            clone.transform.localScale = Vector3.one * scale;
            GameObject.Destroy (this.gameObject);
        }
    }
}