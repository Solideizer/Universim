 using System.Collections.Generic;
 using System.Collections;
 using UnityEngine;
 public class Chicken : Animal {
     public override void Reproduce () {
         if (thirstAmount < thirstThreshold / 2 && hungerAmount < hungerThreshold / 2) {

             Transform partner = FindClosest ("Chicken");
             Move (partner);
             //transform.Translate(partner.position);             
         }
     }

 }