
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    protected IState currentState;

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState?.Enter();
    }

    void Update()
    {
        currentState?.Execute();
    }
}
