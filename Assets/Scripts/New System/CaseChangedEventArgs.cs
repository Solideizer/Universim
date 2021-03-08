using System;
using UnityEngine;

public class CaseChangedEventArgs : EventArgs
{
    public CaseChangedEventArgs(Transform transform, Case c)
    {
        this.transform = transform;
        this.state = c;
    }

    public Transform transform;
    public Case state;
}

public enum Case { AVAILABLE, IDLE, WANDER, HUNGER, THIRST }
