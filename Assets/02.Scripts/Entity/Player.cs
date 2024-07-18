using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Player : Singleton<Player>
{
    public PlayerData data;
    public PlayerFSM fsm;
    public PlayerHealthSystem healthSystem;

    [Header("LocatedPixelmon")]
    public float radius = 2.0f;
    public int currentPixelmonCount;
    public Pixelmon[] pixelmons = new Pixelmon[5];

    private void Start()
    {
        fsm.Init();
        LocatedPixelmon();
    }

    private void Update()
    {
        fsm.Update();
    }

    public void ChangePixelmonsState(PixelmonState newState)
    {
        for(int i = 0;  i < pixelmons.Length; i++)
        {
            if (pixelmons[i] == null) continue;
            switch (newState)
            {
                case PixelmonState.Attack:
                    pixelmons[i].fsm.ChangeState(pixelmons[i].fsm.AttackState);
                    break;
                case PixelmonState.Idle:
                    pixelmons[i].fsm.ChangeState(pixelmons[i].fsm.IdleState);
                    break;
                case PixelmonState.Move:
                    pixelmons[i].fsm.ChangeState(pixelmons[i].fsm.MoveState);
                    break;
            }
        }
    }

    public void SetPixelmonsTarget(GameObject target)
    {
        for(int i = 0;i < pixelmons.Length; i++)
        {
            if (pixelmons[i] == null) continue;
            pixelmons[i].fsm.target = target;
        }
    }

    private void LocatedPixelmon()
    {
        int angle = 360 / currentPixelmonCount;
        int currentAngle = -90;
        for (int i = 0; i < currentPixelmonCount; i++)
        {
            Vector3 pos = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius, Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius - 0.6f, 0);
            pixelmons[i].transform.position = pos;
            currentAngle += angle;
        }
    }
}