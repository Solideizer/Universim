 using System.Collections.Generic;
 using System.Collections;
 using UnityEngine;
 public class Chicken : Animal {
     [SerializeField] private GameObject chickenPrefab;
     private string chicken = "Chicken";
     protected override void Update () {
         base.Update();
         Reproduce (chicken, chickenPrefab);
     }

     protected override void Reproduce(string tag,GameObject prefab)
     {
         reproductiveUrge += Time.deltaTime * 2;
         if (reproductiveUrge > hungerAmount && reproduceDuration > thirstAmount)
         {
             Transform partner = FindClosest (tag);
             Move (partner);

             float dist = Vector3.Distance (partner.position, _transform.position);

             if (dist < 1f)
             {
                 StartCoroutine (ReproduceDuration ());
                 Instantiate (prefab, _transform.position + new Vector3(1f,0f,1f), Quaternion.identity);
                 reproductiveUrge = 0f;
             }
         }
     }
 }
 