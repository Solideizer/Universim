using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class Fox : Animal {
    [SerializeField] private GameObject foxPrefab;   
    private string fox = "Fox";
    protected override void Update () {
         GetHungry ();
         GetThirsty ();
         Reproduce (fox, foxPrefab);
     }
    public override void FindFood () {
        Transform closestFood = FindClosest ("Chicken");
        Move (closestFood);

    }
    

 }