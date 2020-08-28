using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Animal : MonoBehaviour, IAnimalBehavior {

    protected float health = 100f;
    protected static float hungerAmount = 0f;
    protected static float thirstAmount = 0f;
    protected float hungerThreshold = 50f;
    protected float thirstThreshold = 50f;
    protected float reproduceDuration;
    protected float pregnancyDuration;
    protected float movementSpeed = 2f;
    protected float visionRadius = 20f;
    protected Transform _transform;
    protected virtual void Awake () {
        _transform = GetComponent<Transform> ();
    }
    protected virtual void Update () {
        GetHungry ();
        GetThirsty ();
        Reproduce ("",null);

    }

    public virtual void GetThirsty () {
        thirstAmount += Time.deltaTime * 0.5f;
        //Debug.Log("thirst amount "+ thirstAmount);
        //Debug.Log("thirst threshold "+ thirstThreshold);
        if (thirstAmount > thirstThreshold / 3 && thirstAmount > hungerAmount) {
            FindWater ();
            //Debug.Log ("Finding Water");
        } else if (thirstAmount >= thirstThreshold) {
            Die ();
        }
    }

    public virtual void GetHungry () {
        hungerAmount += Time.deltaTime;
        Debug.Log ("hunger amount " + hungerAmount);
        //Debug.Log("hunger threshold "+ hungerThreshold);
        if (hungerAmount > hungerThreshold / 3 && hungerAmount > thirstAmount) {
            FindFood ();
            //Debug.Log ("Finding Food");
        } else if (hungerAmount >= hungerThreshold) {
            Die ();
        }
    }
    public virtual void Eat (Transform food) {
        hungerAmount = 0f;
        food.localScale -= Vector3.one * Time.deltaTime;
        Destroy (food.gameObject, 3f);
        Debug.Log ("eating");

    }
    public virtual void Drink (Transform water) {
        thirstAmount = 0f;
        Debug.Log ("drinking");

    }
    public virtual void FindFood () {
        Transform closestFood = FindClosest ("Plant");
        Move (closestFood);
        float dist = Vector3.Distance (closestFood.position, _transform.position);
        if (dist < 3f) {
            Eat (closestFood);
        }

    }
    public virtual void FindWater () {
        Transform closestWater = FindClosest ("Water");
        Move (closestWater);
        float dist = Vector3.Distance (closestWater.position, _transform.position);
        if (dist < 3f) {
            Drink (closestWater);
        }

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

    protected void Reproduce (string tag, GameObject prefab) {

        if (thirstAmount < thirstThreshold / 2 && hungerAmount < hungerThreshold / 2)
        {
            Transform partner = FindClosest (tag);
            Move (partner);

            float dist = Vector3.Distance (partner.position, _transform.position);

            if (dist < 1f)
            {
                StartCoroutine (ReproduceDuration ());
                Instantiate (prefab, _transform.position, Quaternion.identity);
            }
        }

    }
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

    public IEnumerator ReproduceDuration () {
        yield return new WaitForSeconds (reproduceDuration);
    }
}