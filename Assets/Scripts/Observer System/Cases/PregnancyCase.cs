using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregnancyCase : MonoBehaviour, ICase
{
    [SerializeField, Range(15f, 35f)] float pregnancyTime = 22f;
    public float pregnancy = 0;
    public bool isRunning;

    bool canPregnant;
    AnimalAI ai;
    int fertility;
    Genetic partnerGene;

    public Genetic PartnerGene { set => partnerGene = value; }

    private void Start() 
    {
        ai = GetComponent<AnimalAI>();
        ai.CaseChanged += OnCaseChanged;
        isRunning = false;

        IdentityUpdate();
    }

    private IEnumerator Pregnancy(Genetic parent) 
    {
        bool isPregnant = true;
        while(isPregnant)
        {
            pregnancy += Time.deltaTime;
            if (pregnancy > pregnancyTime)
            {
                pregnancy = 0;
                isPregnant = false;
            }

            yield return new WaitForFixedUpdate();
        }

        GiveBirth(parent);
        ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE)); 
    }

    private void GiveBirth(Genetic parent)
    {
        if(gameObject.tag == "Chicken")
        {
            for (var i = 0; i < fertility; i++)
            {
                var animal = AnimalManager.Instance.GetHerbivore(transform.position);
                animal.AwakeAnimal(Genetic.Cross(parent, ai.Identity.GeneticCode));
            }
        }
        else
        {
            for (var i = 0; i < 1; i++)
            {
                var animal = AnimalManager.Instance.GetCarnivore(transform.position);
                animal.AwakeAnimal(Genetic.Cross(parent, ai.Identity.GeneticCode));
            }
        }

        ai.Identity.canReproduce = true;
        partnerGene = null;
        ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.IDENTITY_UPDATE));
    }

    private void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if(e.state == Case.PREGNANCY)
        {
            ai.currentState = Case.PREGNANCY;
            ai.Stop();
            ai.Identity.canReproduce = false;
            
            var data = e.data as PregnancyCaseData;
            data.SetData(this);

            StartCoroutine(Pregnancy(partnerGene));
        }
        else if(e.state == Case.IDENTITY_UPDATE)
            IdentityUpdate();
        else if(e.state == Case.RESET)
        {
            isRunning = false;
            canPregnant = false;
            pregnancy = 0;
        }
    }

    private void IdentityUpdate()
    {
        if (ai.Identity.Sex == Sex.MALE)
            canPregnant = false;
        else
            canPregnant = true;

        fertility = ai.Identity.Fertility;
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}
