using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class Player : Singleton<Player>
{
    public PlayerFSM fsm;
    public PlayerStatHandler statHandler;
    public PlayerHealthSystem healthSystem;
    public GameObject HitPosition;

    [Header("LocatedPixelmon")]
    public float radius = 2.0f;
    public int currentPixelmonCount;
    public Pixelmon[] pixelmons = new Pixelmon[5];

    private void Start()
    {
        fsm.Init();
        statHandler.UpdateStats();
        //LocatedPixelmon();
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

    public void LocatedPixelmon()
    {
        if(currentPixelmonCount == 0) return;
        int angle = 360 / currentPixelmonCount;
        int currentAngle = -90;

        switch (currentPixelmonCount)
        {
            case 2:
                currentAngle = 0;
                break;
            case 4:
                currentAngle = 45;
                break;
            default:
                break;
        }       

        var pxmList = pixelmons.ToList().FindAll((obj) => obj != null);
        for (int i = 0; i < currentPixelmonCount; i++)
        {
            Vector3 pos = new Vector3(Mathf.Cos(currentAngle * Mathf.Deg2Rad) * radius, Mathf.Sin(currentAngle * Mathf.Deg2Rad) * radius - 0.1f, 0);
            pxmList[i].transform.position = transform.position + pos;
            currentAngle += angle;
        }
    }
}