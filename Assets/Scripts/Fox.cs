using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class Fox : Animal {
    [SerializeField] private GameObject foxPrefab;
    public override void Reproduce () {
        if (thirstAmount < thirstThreshold / 2 && hungerAmount < hungerThreshold / 2) {
            Transform partner = FindClosest ("Fox");
            Move (partner);

            float dist = Vector3.Distance(partner.position, _transform.position);
            if(dist < 1f)
            {
                StartCoroutine(ReproduceDuration());
                Instantiate(foxPrefab,_transform.position,Quaternion.identity);                
            }
        }
    }
    public override void FindFood () {
        Transform closestFood = FindClosest ("Chicken");
        Move (closestFood);

    }
    

 }