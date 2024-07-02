using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BaseStateMachine : StateMachine
{
    [Header("Animations")]
    public Animator animator;
    public AnimationData animationData = new AnimationData();

    //protected IdleState idleState;

    //public float MovementSpeed = 1f;
    //public float MovementSpeedModifier { get; set; } = 1f;

    void Awake()
    {
        animationData.Initialize();
    }

    void Start()
    {
        //idleState = new IdleState(this, animator, animationData);
        //ChangeState(idleState);
    }
}
