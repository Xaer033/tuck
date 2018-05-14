using System.Collections;

namespace GhostGen
{
	public interface IGameState
	{
		void Init(GameStateMachine stateMatchine, Hashtable changeStateData);

		void Step(float deltaTime);

		void Exit();
	}
}
