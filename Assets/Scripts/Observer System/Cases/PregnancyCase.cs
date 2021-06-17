using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PregnancyCase : MonoBehaviour, ICase
{
#pragma warning disable 0649
    [SerializeField, Range(15f, 35f)] float pregnancyTime = 22f;
#pragma warning restore 0649

    public float pregnancy = 0;
    public bool isRunning;

    AnimalAI ai;
    int fertility;
    Genetic partnerGene;
    bool isPregnant = false;

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
        isPregnant = true;
        while(isPregnant)
        {
            pregnancy += Time.deltaTime;
            if (pregnancy > pregnancyTime)
            {
                isPregnant = false;
                pregnancy = 0;
                GiveBirth(parent);
                ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.AVAILABLE));
                break;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    private void GiveBirth(Genetic parent)
    {
        if(gameObject.tag == "Chicken")
        {
            for (var i = 0; i < fertility; i++)
            {
                print(gameObject.transform.position);
                var animal = AnimalManager.Instance.GetHerbivore(transform.position);
                animal.AwakeAnimal(Genetic.Cross(parent, ai.Identity.GeneticCode), true);
            }
        }
        else
        {
            for (var i = 0; i < 1; i++)
            {
                var animal = AnimalManager.Instance.GetCarnivore(transform.position);
                animal.AwakeAnimal(Genetic.Cross(parent, ai.Identity.GeneticCode), true);
            }
        }

        ai.Identity.canReproduce = true;
        partnerGene = null;
        ai.OnCaseChanged(new CaseChangedEventArgs(null, Case.IDENTITY_UPDATE));
    }

    private void OnCaseChanged(object sender, CaseChangedEventArgs e)
    {
        if(e.state == Case.PREGNANCY && !isPregnant)
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
            pregnancy = 0;
        }
    }

    private void IdentityUpdate()
    {
        fertility = ai.Identity.Fertility;
    }

    public bool IsRunning() { return isRunning; }
    public void Run() { isRunning = true; }
}
