using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region test
    public GameObject PlayerObject; //오브젝트풀에서 Instantiate할 때 초기화해줄 것.
    #endregion

    #region Enemy Components
    public EnemyData data;
    public Rigidbody2D rb;
    private Animator anim;
    private EnemyStateMachine stateMachine;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        
        stateMachine = new EnemyStateMachine(this);
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.ChaseState);
    }

    private void Update()
    {
        stateMachine.Update();
    }

    private void LoadEnemyData()
    { 

    }
}
