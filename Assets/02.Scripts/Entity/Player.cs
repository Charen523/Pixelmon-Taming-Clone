using UnityEngine;

public class Player : Singleton<Player>
{
    public PlayerData data;
    public PlayerStateMachine stateMachine;

    [Header("Animations")]
    public float radius = 2.0f;
    public int currentPixelmonCount;
    public Pixelmon[] pixelmons = new Pixelmon[5];

    protected override void Awake()
    {
        base.Awake();
        InitiPixelmonFSM();
    }
    private void Start()
    {
        LocatedPixelmon();
    }

    private void InitiPixelmonFSM()
    {
        if (pixelmons == null || pixelmons.Length == 0)
        {
            Debug.LogError("Pixelmon 배열이 초기화되지 않았거나 비어 있음");
            return;
        }

        foreach (var pixelmon in pixelmons)
        {
            if (pixelmon != null)
            {
                pixelmon.StateMachine = new PixelmonStateMachine(pixelmon);
            }
            else
            {
                Debug.LogWarning("Pixelmon is null");
            }
        }
    }

    public void LocatedPixelmon()
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

    public void ChangePixelmonsMoveState()
    {
        foreach (var pixelmon in pixelmons)
        {
            if (pixelmon != null)
            {
                pixelmon.StateMachine.ChangeState(pixelmon.StateMachine.MoveState);
            }
            else
            {
                Debug.LogWarning("Pixelmon is null");
            }
        }
    }
}