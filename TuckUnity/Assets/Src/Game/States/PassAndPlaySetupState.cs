using GhostGen;

public class PassAndPlaySetupState : IGameState
{
    private PassPlaySetupController _passPlaySetupController;
    private ScreenFader _fader;
    private GameStateMachine _stateMachine;

	public void Init( GameStateMachine stateMachine, object changeStateData)
	{
        _stateMachine = stateMachine;

        _passPlaySetupController = new PassPlaySetupController();
        _passPlaySetupController.Start(onStartGame, onPassSetupCancel);

        _fader = Singleton.instance.gui.screenFader;
        _fader.FadeIn(0.35f);       
    }
    
    public void Step( float p_deltaTime )
	{
		
    }

    public void Exit()
	{
        _passPlaySetupController.RemoveView();
	}

    private void onStartGame()
    {
        Singleton.instance.sessionFlags.gameContext = _createGameContext();

        _fader.FadeOut(0.35f, () =>
        {
            _stateMachine.ChangeState(TuckState.PASS_PLAY_GAMEPLAY);
        });
    }

    private GameContext _createGameContext()
    {
        //    int randomSeed = Environment.TickCount;

        //    CardDeck customerDeck = CardDeck.FromFile("Decks/CustomerDeck");
        //    customerDeck.Shuffle(randomSeed);
        //    CardDeck ingredientDeck = CardDeck.FromFile("Decks/IngredientDeck");
        //    ingredientDeck.Shuffle(randomSeed);

        //    PlayerState[] playerStateList = _passPlaySetupController.GetPlayerList();
        GameContext context = GameContext.Create(GameMode.PASS_AND_PLAY);
    //    context.isMasterClient = true;
    //    context.ingredientDeck = ingredientDeck;
    //    context.customerDeck = customerDeck;
        return context;
    }

    private void onPassSetupCancel()
    {
        _fader.FadeOut(0.35f, () =>
        {
            _stateMachine.ChangeState(TuckState.MAIN_MENU);
        });
    }
}
