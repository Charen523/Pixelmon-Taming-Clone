
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PixelmonStateMachine : StateMachine
{
    public Pixelmon Pixelmon { get; private set; }

    public PixelmonIdleState IdleState { get;}
    public PixelmonMoveState WalkState { get;}
    public PixelmonAttackState AttackState { get; }
    public PixelmonStateMachine(Pixelmon pixelmon)
    {
        this.Pixelmon = pixelmon;
        IdleState = new PixelmonIdleState(this);
        WalkState = new PixelmonMoveState(this);
        AttackState = new PixelmonAttackState(this);

        //ChangeState(detectState);

    }
}