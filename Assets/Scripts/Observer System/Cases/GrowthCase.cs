using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthCase : MonoBehaviour, ICase
{
    [SerializeField] bool isRunning;
    [SerializeField, Range(3f, 6f)] float growthTime = 4f;
    [SerializeField] int phaseCount = 3;
    [SerializeField] int growthMultiplier;

    float growth = 0;
    int phase;
    AnimalAI ai;

    bool canGrow;

    private void Start() 
    {
        ai = GetComponent<AnimalAI>();
        ai.CaseChanged += OnCaseChanged;    
        phase = phaseCount;
        isRunning = false;
    }

    private IEnumerator Growth()
    {
        ai.Identity.canReproduce = false;
        while(phase > 0)
        {
            growth += Time.deltaTime;
            if (growth > growthTime)
            {
                growth = 0;
                GrowUp();
                phase--;
            }
            yield return new WaitForFixedUpdate();
        }
        
        ai.Identity.canReproduce = true;
        this.enabled = false;
        ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.IDENTITY_UPDATE));
    }

    private void GrowUp()
    {
        Vector3 scale = transform.localScale;
        // TODO Bu kısmın daha genelleşmesi lazım.
        scale += Vector3.one * growthMultiplier;
        transform.localScale = scale;
    }

    private void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if(e.state == Case.GROWTH)
        {
            StartCoroutine(Growth());
        }
        else if(e.state == Case.IDENTITY_UPDATE)
        {
            // TODO GROWTH MULTIPLIER DEGISECEK.
        }
        else if(e.state == Case.RESET)
        {
            phase = phaseCount;
            growth = 0;
        }
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}
