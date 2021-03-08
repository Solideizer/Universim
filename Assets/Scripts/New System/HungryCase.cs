using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryCase : MonoBehaviour, ICase
{
    [SerializeField] LayerMask targetMask;
    [SerializeField, Range(10f, 30f)] float hungerTreshold = 0f;
    [SerializeField, Range(5f, 20f)] float targetRange = 10f;
    [SerializeField, Range(30f, 60f)] float vision = 40f;
    [SerializeField] bool isRunning;

    public float hunger = 0;
    public bool alerted;

    Transform target;
    AnimalAI ai;
    
    private void Start()
    {
        ai = GetComponent<AnimalAI>();
        ai.CaseChanged += OnCaseChanged;
        ai.caseDatas.Add(new CaseContainer(Case.HUNGER, hunger, hungerTreshold));

        isRunning = false;
        alerted = false;
    }

    private void Update()
    {
        hunger += Time.deltaTime;

        if (isRunning)
        {
            ai.Move(target.position);
            if (target != null && Vector3.Distance(target.position, transform.position) < targetRange)
            {
                hunger = 0;
                isRunning = false;
                alerted = false;
                target = null;
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.IDLE));
            }
        }

        if (!alerted && hunger > hungerTreshold)
        {
            alerted = true;
            ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
        }
    }

    private Transform FindFood()
    {
        return ai.FindClosestThing(ai.transform.position, targetMask, vision);
    }

    public void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if (e.state == Case.HUNGER)
        {
            target = FindFood();

            if(target != null)
            {
                ai.currentState = Case.HUNGER;
                Run();
                
            }
            else
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.WANDER));

        }
        else if (e.state == Case.AVAILABLE)
            CaseContainer.Adjust(ai.caseDatas, Case.HUNGER, hunger);
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}
