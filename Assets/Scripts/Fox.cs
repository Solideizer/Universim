 using System.Collections.Generic;
 using System.Collections;
 using UnityEngine;
 public class Fox : Animal {
     public override void Reproduce () {
         if (thirstAmount < thirstThreshold / 2 && hungerAmount < hungerThreshold / 2) {
             Transform partner = FindClosest ("Fox");
             Move (partner);
             //transform.Translate(partner.position);         
         }
     }
     public override void FindFood () {
         Transform closestFood = FindClosest ("Chicken");
         Move (closestFood);
         //transform.Translate (closestFood.position);
     }

 }