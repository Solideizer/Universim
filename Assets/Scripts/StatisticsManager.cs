using System;
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
    public List<int> numberOfMaleFoxes { get; } = new List<int> ();
    public List<int> numberOfFemaleFoxes { get; } = new List<int> ();
    public List<int> numberOfMaleChickens { get; } = new List<int> ();
    public List<int> numberOfFemaleChickens { get; } = new List<int> ();
    private void Start ()
    {
        StartCoroutine (GetFoxStats ());
        StartCoroutine (GetChickenStats ());
    }

    IEnumerator GetFoxStats ()
    {
        while (true)
        {
            var foxes = GameObject.FindGameObjectsWithTag ("Fox");
            _currentNumOfFoxes = foxes.Length;
            numberOfFoxes.Add (_currentNumOfFoxes);
            Debug.Log ("fox " + _currentNumOfFoxes);

            int _maleFoxAmount = 0;
            int _femaleFoxAmount = 0;

            for (int i = 0; i < foxes.Length; i++)
            {
                var id = foxes[i].GetComponent<AnimalAI> ().Identity;
                if ((int) id.Sex == 0)
                {
                    _maleFoxAmount++;
                }
                else
                {
                    _femaleFoxAmount++;
                }

            }

            numberOfMaleFoxes.Add (_maleFoxAmount);
            numberOfFemaleFoxes.Add (_femaleFoxAmount);
            yield return new WaitForSeconds (interval);
        }
    }

    IEnumerator GetChickenStats ()
    {
        while (true)
        {
            var chicken = GameObject.FindGameObjectsWithTag ("Chicken");
            _currentNumOfChickens = chicken.Length;
            numberOfChickens.Add (_currentNumOfChickens);
            Debug.Log ("chicken " + _currentNumOfChickens);

            int _maleChickenAmount = 0;
            int _femaleChickenAmount = 0;

            for (int i = 0; i < chicken.Length; i++)
            {
                var id = chicken[i].GetComponent<AnimalAI> ().Identity;
                if ((int) id.Sex == 0)
                {
                    _maleChickenAmount++;
                }
                else
                {
                    _femaleChickenAmount++;
                }
            }
            numberOfMaleChickens.Add (_maleChickenAmount);
            numberOfFemaleChickens.Add (_femaleChickenAmount);

            yield return new WaitForSeconds (interval);
        }

    }

}