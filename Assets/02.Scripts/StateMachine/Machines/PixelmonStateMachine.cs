
using System.Collections.Generic;
using UnityEngine;

public class PixelmonStateMachine : StateMachine
{
    public Pixelmon Pixelmon { get; private set; }

    public PixelmonIdleState IdleState { get;}
    public PixelmonWalkState WalkState { get;}
    public PixelmonAttackState AttackState { get; }
    public PixelmonStateMachine(Pixelmon pixelmon)
    {
        this.Pixelmon = pixelmon;
    }
}