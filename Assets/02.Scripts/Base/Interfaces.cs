public interface IState
{
    void Enter();
    void Execute();
    void Exit();
}

public interface IPlayerState : IState
{
    void HandleInput();
}

public interface IData
{
    string Rcode { get; }
}

public interface IDamagable
{
    void OnChangeHealth(float delta);
}