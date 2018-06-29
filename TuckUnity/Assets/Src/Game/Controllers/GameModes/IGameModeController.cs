using System;

public interface IGameModeController : GhostGen.IEventDispatcher
{
    void Start(GameContext context);
    void Step(double deltaTime);
    void CleanUp();
}
