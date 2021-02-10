 using UnityEngine;
 public class Chicken : Animal
 {
     //[SerializeField] private GameObject chickenPrefab;
     // string chicken = "Chicken";

     // protected override void Reproduce(string tag,GameObject prefab)
     // {
     //     ReproductiveUrge += Time.deltaTime * 2;
     //     if (ReproductiveUrge > HungerAmount && ReproductiveUrge > ThirstAmount)
     //     {
     //         Transform partner = FindClosestThing(tag,transform);
     //         Move (partner);
     //
     //         float dist = Vector3.Distance (partner.position, _transform.position);
     //
     //         if (dist < 1f)
     //         {
     //             StartCoroutine (StartReproducing ());
     //             Instantiate (prefab, _transform.position + new Vector3(5f,0f,5f), Quaternion.identity);
     //         }
     //         ReproductiveUrge = 0f;
     //     }
     // }
 }