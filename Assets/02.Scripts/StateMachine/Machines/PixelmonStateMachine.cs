
using System;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PixelmonStateMachine : StateMachine
{
    public Pixelmon Pixelmon { get; private set; }

    public Action stageStart, stageClear, stageTimeOut, playerDie;

    #region Pixelmon States
    public PixelmonIdleState IdleState { get; private set; }
    public PixelmonMoveState MoveState { get; private set; }
    public PixelmonAttackState AttackState { get; private set; }
    #endregion

    public PixelmonStateMachine(Pixelmon pixelmon)
    {
        Pixelmon = pixelmon;
        IdleState = new PixelmonIdleState(Pixelmon.StateMachine);
        MoveState = new PixelmonMoveState(Pixelmon.StateMachine);
        AttackState = new PixelmonAttackState(Pixelmon.StateMachine);

        GameManager.Instance.OnStageStart += stageStart = () => ChangeState(IdleState);
        GameManager.Instance.OnStageClear += stageClear = () => ChangeState(IdleState);
        GameManager.Instance.OnStageTimeOut += stageTimeOut = () => ChangeState(IdleState);
        GameManager.Instance.OnPlayerDie += playerDie = () => ChangeState(IdleState);

        ChangeState(IdleState);
    }
}