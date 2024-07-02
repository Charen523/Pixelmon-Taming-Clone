using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class BaseStateMachine : StateMachine
{
    [Header("Animations")]
    public Animator anim;
    public AnimationData animationData = new AnimationData();

    [Header("Physics")]
    public Rigidbody2D rb;

    [HideInInspector] public IdleState idleState;
    [HideInInspector] public AttackState attackState;

    public float MovementSpeed = 1f;

    protected virtual void Awake()
    {
        animationData.Initialize();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        idleState = new IdleState(this);
        attackState = new AttackState(this);
    }
}
