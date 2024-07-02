
using System.Collections.Generic;
using UnityEngine;

public class PixelmonStateMachine : BaseStateMachine
{
    public Pixelmon Pixelmon { get; private set; }

    public PixelmonIdleState IdleState { get;}
    public PixelmonWalkState WalkState { get;}
    public PixelmonAttackState AttaxkState { get; }
    public PixelmonStateMachine(Pixelmon pixelmon)
    {
        this.Pixelmon = pixelmon;
    }
}