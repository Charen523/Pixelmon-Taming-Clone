using UnityEngine;

public class Player : Singleton<Player>
{
    public PlayerData Data;
    public PlayerStateMachine StateMachine;

    protected override void Awake()
    {
        base.Awake();

        foreach (var pixelmon in PixelmonManager.Instance.Pixelmons)
        {
            if (pixelmon != null)
            {
                pixelmon.StateMachine = new PixelmonStateMachine(pixelmon);
            }
            else
            {
                Debug.Log("null");
            }
        }
    }
}