using System;

public interface IGameModeController : GhostGen.IEventDispatcher
{
    void Start();
    void Step(double deltaTime);
    void CleanUp();
}
