using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AnimationType
{
    Idle = 0,
    Walk = 1
}

public class AnimationManager : MonoBehaviour
{
    private Animator _animator;
    private AnimationType _animationType;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _animationType = AnimationType.Idle;
    }

    void Update()
    {
        runAnimation();
    }

    private void runAnimation()
    {
        switch (_animationType)
        {
            case AnimationType.Walk:
                _animator.SetInteger("animation", (int)_animationType);
                break;
            case AnimationType.Idle:
                _animator.SetInteger("animation", (int)_animationType);
                break;
            default:
                break;
        }
    }

    public void SetState(AnimationType state)
    {
        _animationType = state;
    }
}
