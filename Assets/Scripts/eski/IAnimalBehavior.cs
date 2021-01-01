using UnityEngine;
using System.Collections;
interface IAnimalBehavior {
    //void Wander();
    //void GetHungry();
    //void GetThirsty();
    //void FindFood();
    //void FindWater();
    IEnumerator Eat(Transform food);
    IEnumerator Drink(Transform water);  
    void Move(Transform destination);
    IEnumerator StartReproducing(); 
    void Die();
}