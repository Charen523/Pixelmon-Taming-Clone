using System;
using UnityEngine;

public class Player : Singleton<Player>
{
    public PlayerData data;
    public PlayerStateMachine stateMachine;

    [Header("Animations")]
    public float radius = 2.0f;
    public int currentPixelmonCount;

    public Pixelmon[] pixelmons = new Pixelmon[5];

    public event Action OnPlayerMove;
    public Action playerMove, targetReached;

    private void Start()
    {
        LocatedPixelmon();
        SubscribePixelmons();
    }

    public void NotifyPlayerMove()
    {
        OnPlayerMove?.Invoke();
    }

    private void SubscribePixelmons()
    {
        foreach (var pixelmon in pixelmons)
        {
            if (pixelmon != null)
            {
                OnPlayerMove += playerMove = () => pixelmon.StateMachine.ChangeState(pixelmon.StateMachine.MoveState);
                stateMachine.MoveState.OnTargetReached += targetReached = () => pixelmon.StateMachine.ChangeState(pixelmon.StateMachine.AttackState);
            }
        }
    }

    private void LocatedPixelmon()
    {
        int angle = 360 / currentPixelmonCount;
        int currentAngle = 90;
        for (int i = 0; i < currentPixelmonCount; i++)
        {
            Vector3 pos = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius, Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius, 0);
            pixelmons[i].transform.position = pos;
            currentAngle += angle;
        }
    }
}