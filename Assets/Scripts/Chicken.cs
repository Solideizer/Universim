 using System.Collections.Generic;
 using System.Collections;
 using UnityEngine;
 public class Chicken : Animal {
     [SerializeField] private GameObject chickenPrefab;
     public override void Reproduce () {
         if (thirstAmount < thirstThreshold / 2 && hungerAmount < hungerThreshold / 2) {

             Transform partner = FindClosest ("Chicken");
             Move (partner);
             float dist = Vector3.Distance (partner.position, _transform.position);
             if (dist < 1f) {
                 StartCoroutine (ReproduceDuration ());
                 Instantiate (chickenPrefab, _transform.position, Quaternion.identity);
             }
         }
     }

 }