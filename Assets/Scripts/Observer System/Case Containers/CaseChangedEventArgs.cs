using System;
using UnityEngine;

public class CaseChangedEventArgs : EventArgs
{
    public CaseChangedEventArgs(CaseData data, Case c)
    {
        this.data = data;
        this.state = c;
    }

    public Case state;
    public CaseData data;
}

public enum Case { AVAILABLE, IDENTITY_UPDATE, IDLE, WANDER, HUNGER, THIRST, REPRODUCTION, PREGNANCY, GROWTH }
