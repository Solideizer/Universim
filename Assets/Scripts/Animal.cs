using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour, IAnimalBehavior {

    protected float health = 100f;
    protected float hungerAmount = 0f;
    protected float thirstAmount = 0f;
    protected float hungerThreshold = 10f;
    protected float thirstThreshold = 30f;
    protected float reproduceDuration;
    protected float pregnancyDuration;
    protected float movementSpeed = 2f;
    protected float visionRadius = 20f;
    protected Transform _transform;
    protected bool foundFood = false;
    protected bool foundWater = false;
    //public virtual bool isMale {get; set;}
    protected virtual void Awake () {
        _transform = GetComponent<Transform> ();
    }
    protected virtual void Update () {
        GetHungry ();
        GetThirsty ();

    }

    public virtual void GetThirsty () {
        thirstAmount += Time.deltaTime * 2;
        //Debug.Log("thirst amount "+ thirstAmount);
        //Debug.Log("thirst threshold "+ thirstThreshold);
        if (foundFood) {

            if (thirstAmount > thirstThreshold / 2) {
                foundWater = false;
                FindWater ();
                Debug.Log ("Finding Water");
            } else if (thirstAmount >= thirstThreshold) {
                Die ();
            }
        }
    }

    public virtual void GetHungry () {
        hungerAmount += Time.deltaTime * 2;
        //Debug.Log("hunger amount "+ hungerAmount);
        //Debug.Log("hunger threshold "+ hungerThreshold);
        if (hungerAmount > hungerThreshold / 2) {
            foundFood = false;
            FindFood ();
            Debug.Log ("Finding Food");
        } else if (hungerAmount >= hungerThreshold) {
            Die ();
        }
    }

    public virtual void Eat () {
        hungerAmount = 0f;
        foundFood = true;
    }
    public virtual void Drink () {
        thirstAmount = 0f;
        foundWater = true;
    }
    public virtual void FindFood () {
        Transform closestFood = FindClosest ("Plant");
        Move (closestFood);
        //_transform.Translate (closestFood.position);
    }
    public virtual void FindWater () {
        Transform closestWater = FindClosest ("Water");
        Move (closestWater);
        //_transform.Translate (closestWater.position);
    }
    public virtual void Move (Transform destination) {
        float step = movementSpeed * Time.deltaTime;
        _transform.position = Vector3.MoveTowards (transform.position, destination.position, step);
    }
    public virtual void TakeDamage (float damageAmount) {
        health -= damageAmount;
        if (health <= 0f) {
            Die ();
        }
    }
    public abstract void Reproduce ();
    public virtual void Die () {
        Destroy (gameObject);
    }

    protected Transform FindClosest (string tag) {
        GameObject[] objects = GameObject.FindGameObjectsWithTag (tag);
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector3 currentPosition = _transform.position;

        foreach (GameObject potentialTarget in objects) {
            Vector3 directionToTarget = potentialTarget.transform.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr) {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget.transform;
            }
        }

        return bestTarget;
    }
}