using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelmonIdleState : IdleState
{
    private PixelmonStateMachine pixelmonStateMachine;
    public PixelmonIdleState(PixelmonStateMachine stateMachine)
        : base(stateMachine)
    {
        pixelmonStateMachine = stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        GameManager.Instance.OnStageStart -= pixelmonStateMachine.stageStart;
        GameManager.Instance.OnStageClear -= pixelmonStateMachine.stageClear;
        GameManager.Instance.OnStageTimeOut -= pixelmonStateMachine.stageTimeOut;
        GameManager.Instance.OnPlayerDie -= pixelmonStateMachine.playerDie;
    }

    public override void Exit()
    {
        base.Exit();
        GameManager.Instance.OnStageStart += pixelmonStateMachine.stageStart;
        GameManager.Instance.OnStageClear += pixelmonStateMachine.stageClear;
        GameManager.Instance.OnStageTimeOut += pixelmonStateMachine.stageTimeOut;
        GameManager.Instance.OnPlayerDie += pixelmonStateMachine.playerDie;
    }

    /*
    public GameObject Search(GameObject pixelmon, List<GameObject> enemies)
    {
        GameObject target = new GameObject();
        // 5f => 탐색범위 길이로 변경
        float minDistance = 5f;
        enemies.ForEach(enemy =>
        {
            Vector3 pos = pixelmon.transform.position - enemy.transform.position;
            float distance = pos.sqrMagnitude;

            if (distance < minDistance * minDistance)
                target = enemy;
        });
        return target;
    }*/
}
