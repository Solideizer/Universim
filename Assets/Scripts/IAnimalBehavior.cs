using UnityEngine;
using System.Collections;
interface IAnimalBehavior {
    void GetHungry();
    void GetThirsty();
    void FindFood();
    void FindWater();
    void Eat(Transform food);
    void Drink(Transform water);  
    void Move(Transform destination);  
    //void TakeDamage(float amount);
    void Reproduce();   
    IEnumerator ReproduceDuration(); 
    void Die();
}