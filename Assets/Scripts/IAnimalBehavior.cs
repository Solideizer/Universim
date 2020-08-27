using UnityEngine;
interface IAnimalBehavior {
    void GetHungry();
    void GetThirsty();
    void FindFood();
    void FindWater();
    void Eat();
    void Drink();  
    void Move(Transform destination);  
    //void TakeDamage(float amount);
    void Reproduce();    
    void Die();
}