using UnityEngine;

namespace GhostGen
{
    public class GameStateMachine
	{
        private IStateFactory _stateFactory;
        private IGameState _currentState;

        private string _currentId;

        public GameStateMachine( IStateFactory p_stateFactory )
		{
			_currentState 	= null;
			_currentId 		= "-666";
			_stateFactory 	= p_stateFactory;
		}

		public void Step( float p_deltaTime )
		{
			if( _currentState != null )
				_currentState.Step( p_deltaTime );
		}

		public void ChangeState( string stateId, object changeStateInfo = null )
		{
			if (_currentId == stateId)
				return;

			if( _currentState != null )
				_currentState.Exit( );

            if(_stateFactory == null)
            {
                Debug.LogError("State Factory is null!");
                return;
            }

			_currentState = _stateFactory.CreateState( stateId );

            if(_currentState == null)
            {
                Debug.LogError("New current state: " + stateId + " is null!");
                return;
            }

			_currentState.Init(this, changeStateInfo);

		}
	}
}