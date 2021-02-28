using UnityEngine;

namespace Procedural_Generation
{
    public class ProceduralObjectPlacement : MonoBehaviour
    {
        #region Variable Declarations
#pragma warning disable 0649
        [SerializeField] GameObject raycastAligner;
        [SerializeField] int objectAmount = 200;
        [SerializeField] private float itemXSpread = 10f;
        [SerializeField] private float itemYSpread = 0f;
        [SerializeField] private float itemZSpread = 10f;
#pragma warning restore 0649
        #endregion
        void Start ()
        {
            for (int i = 0; i < objectAmount; i++)
            {
                SpreadAligners ();
            }
        }

        private void SpreadAligners ()
        {
            Vector3 randomPos = new Vector3 (Random.Range (-itemXSpread, itemXSpread),
                Random.Range (-itemYSpread, itemYSpread),
                Random.Range (-itemZSpread, itemZSpread));
            Instantiate (raycastAligner, randomPos, Quaternion.identity);
        }

    }
}