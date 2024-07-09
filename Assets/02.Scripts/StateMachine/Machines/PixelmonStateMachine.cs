
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PixelmonStateMachine : StateMachine
{
    public Pixelmon Pixelmon { get; private set; }

    public IdleState IdleState { get; private set; }
    public PixelmonMoveState MoveState { get; private set; }
    public PixelmonAttackState AttackState { get; private set; }
    private void Start()
    {
        IdleState = new IdleState(this);
        MoveState = new PixelmonMoveState(this, null);
        AttackState = new PixelmonAttackState(this);
        
        ChangeState(IdleState);
    }
}