 using System.Collections.Generic;
 using System.Collections;
 using UnityEngine;
 public class Chicken : Animal {
     [SerializeField] private GameObject chickenPrefab;
     private string chicken = "Chicken";
     protected override void Update () {
         GetHungry ();
         GetThirsty ();
         Reproduce (chicken, chickenPrefab);
     }

 }
 