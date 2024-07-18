using System;
using UnityEngine;

public class FSM : MonoBehaviour
{
    public IState currentState;

    [Header("Animations")]
    public Animator anim;
    public AnimationData animData = new AnimationData();

    public Rigidbody2D rb;
    public GameObject target;

    public virtual void ChangeState(IState newState)
    {
        if (currentState != null)
            currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    public virtual void Awake()
    {
        animData.Initialize();
    }

    public virtual void Update()
    {
        if (currentState != null)
            currentState?.Execute();
    }
}
