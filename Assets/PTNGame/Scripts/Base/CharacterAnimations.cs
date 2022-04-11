using System;
using UnityEngine;

public class CharacterAnimations
{
    private readonly Animator cAnimator;
    private static readonly int RunSpeed = Animator.StringToHash("RunSpeed");
    private static readonly int Falling = Animator.StringToHash("Falling");
    private static readonly int FallingDown = Animator.StringToHash("FallingDown");
    private float lastRunSpeed;

    protected internal CharacterAnimations(Animator pAnimator)
    {
        cAnimator = pAnimator;
    }

    protected internal void SetRun(float runSpeed)
    {
        if (runSpeed == lastRunSpeed) return;
        lastRunSpeed = runSpeed;
        cAnimator.SetFloat(RunSpeed, runSpeed);
    }

    protected internal void SetFalling(bool falling)
    {
        cAnimator.SetBool(Falling, falling);
    }

    protected internal void SetFallingDown(bool fallingDown)
    {
        cAnimator.SetBool(FallingDown, fallingDown);
    }
}