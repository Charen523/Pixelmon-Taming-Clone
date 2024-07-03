using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    //public Player Player { get; }
    public string EnemyTag = "Slime";

    public Vector2 MovementInput { get; set; }

   // public bool IsAttacking { get; set; }

    public Transform MainCameraTransform { get; set; } // 카메라가 플레이어 따라다님

    // States
    public IdleState idleState { get; private set; }
    public PlayerDetectState detectState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    private void Start()
    {
        MovementSpeed = 3f;
        AttackRange = 2f;

        idleState = new IdleState(this);
        detectState = new PlayerDetectState(this);
        moveState = new PlayerMoveState(this, null);

        GameManager.Instance.OnGameStarted += () => ChangeState(detectState);   
        GameManager.Instance.OnGameEnded += () => ChangeState(idleState);
        moveState.OnTargetReached += ChangeAttackState;

        ChangeState(detectState);
    }

    // 공격 상태로 변경(플레이어는 idle 상태)
    private void ChangeAttackState()
    {
        ChangeState(idleState);
    }
}