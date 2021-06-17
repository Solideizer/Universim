using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsManager : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private float interval = 10f;
#pragma warning restore 0649
    private int _currentNumOfFoxes;
    private int _currentNumOfChickens;
    public List<int> numberOfFoxes { get; } = new List<int> ();
    public List<int> numberOfChickens { get; } = new List<int> ();
    private void Start ()
    {
        StartCoroutine (GetNumberOfFoxes ());
        StartCoroutine (GetNumberOfChicken ());
    }

    IEnumerator GetNumberOfFoxes ()
    {
        while (true)
        {
            var foxes = GameObject.FindGameObjectsWithTag ("Fox");
            _currentNumOfFoxes = foxes.Length;
            numberOfFoxes.Add (_currentNumOfFoxes);
            Debug.Log ("fox " + _currentNumOfFoxes);
            yield return new WaitForSeconds (interval);
        }
    }

    IEnumerator GetNumberOfChicken ()
    {
        while (true)
        {
            var chicken = GameObject.FindGameObjectsWithTag ("Chicken");
            _currentNumOfChickens = chicken.Length;
            numberOfChickens.Add (_currentNumOfChickens);
            Debug.Log ("chicken " + _currentNumOfChickens);
            yield return new WaitForSeconds (interval);
        }

    }

}