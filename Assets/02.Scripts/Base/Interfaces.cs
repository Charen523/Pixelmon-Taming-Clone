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