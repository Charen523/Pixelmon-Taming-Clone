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
    public List<Pixelmon> pixelmons = new List<Pixelmon>(5);

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
        foreach (Pixelmon pixelmon in pixelmons)
        {
            switch (newState)
            {
                case PixelmonState.Attack:
                    pixelmon.fsm.ChangeState(pixelmon.fsm.AttackState);
                    break;
                case PixelmonState.Idle:
                    pixelmon.fsm.ChangeState(pixelmon.fsm.IdleState);
                    break;
                case PixelmonState.Move:
                    pixelmon.fsm.ChangeState(pixelmon.fsm.MoveState);
                    break;
            }
        }
    }

    public void SetPixelmonsTarget(GameObject target)
    {
        foreach(Pixelmon pixelmon in pixelmons)
        {
            pixelmon.fsm.target = target;
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