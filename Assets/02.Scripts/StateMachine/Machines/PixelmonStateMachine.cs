
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PixelmonStateMachine : StateMachine
{
    public Pixelmon Pixelmon { get; private set; }

    public IdleState IdleState { get; private set; }
    public PixelmonMoveState MoveState { get; private set; }
    public PixelmonAttackState AttackState { get; private set; }

    public PixelmonStateMachine(Pixelmon pixelmon)
    {
        Pixelmon = pixelmon;
        IdleState = new IdleState(Pixelmon.StateMachine);
        MoveState = new PixelmonMoveState(Pixelmon.StateMachine);
        AttackState = new PixelmonAttackState(Pixelmon.StateMachine);

        GameManager.Instance.OnStageStart += () => ChangeState(IdleState);
        GameManager.Instance.OnStageClear += () => ChangeState(IdleState);
        GameManager.Instance.OnStageTimeOut += () => ChangeState(IdleState);
        GameManager.Instance.OnPlayerDie += () => ChangeState(IdleState);

        ChangeState(IdleState);
    }
}