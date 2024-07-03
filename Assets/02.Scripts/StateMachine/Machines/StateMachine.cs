
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected IState currentState;

    [Header("Animations")]
    public Animator anim;
    public AnimationData animationData = new AnimationData();

    [Header("Physics")]
    public Rigidbody2D rb;

    public float MovementSpeed = 1f;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    protected virtual void Awake()
    {
        animationData.Initialize();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        currentState?.Execute();
    }
}
