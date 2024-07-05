using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    //public Player Player { get; }
    public string EnemyTag = "Slime";

    public Vector2 MovementInput { get; set; }

    // States
    public IdleState IdleState { get; private set; }
    public PlayerDetectState DetectState { get; private set; }
    public PlayerMoveState MoveState { get; private set; }

    private void Start()
    {
        MovementSpeed = 3f;
        AttackRange = 2f;

        IdleState = new IdleState(this);
        DetectState = new PlayerDetectState(this);
        MoveState = new PlayerMoveState(this, null);

        GameManager.Instance.OnStageStart += () => ChangeState(DetectState);   
        GameManager.Instance.OnStageTimeOut += () => ChangeState(IdleState);
        MoveState.OnTargetReached += ChangeAttackState;

        ChangeState(DetectState);
    }

    // 공격 상태로 변경(플레이어는 idle 상태)
    private void ChangeAttackState()
    {
        ChangeState(IdleState);
    }
}