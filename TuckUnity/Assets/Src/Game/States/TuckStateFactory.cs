using UnityEngine;
using GhostGen;

public class TuckState
{
    public const string NO_STATE                = "none";

    public const string INTRO                   = "intro";
    public const string MAIN_MENU               = "main_menu";
    public const string MULTIPLAYER_SETUP       = "muliplayer_setup";
    public const string MULTIPLAYER_GAMEPLAY    = "multiplayer_gameplay";
    public const string SINGLEPLAYER_SETUP      = "singleplayer_setup";
    public const string SINGLEPLAYER_GAMEPLAY   = "singleplayer_gameplay";
    public const string PASS_PLAY_SETUP         = "pass_and_play_setup";
    public const string PASS_PLAY_GAMEPLAY      = "pass_and_play_gameplay";
    public const string CREDITS                 = "credits";
}


public class TuckStateFactory : IStateFactory
{
    public IGameState CreateState(string stateId)
    {
        switch (stateId)
        {
            case TuckState.INTRO:                       return new IntroState();
            case TuckState.MAIN_MENU:                   return new MainMenuState();
            case TuckState.MULTIPLAYER_SETUP:           return new MultiplayerSetupState();
            case TuckState.MULTIPLAYER_GAMEPLAY:        return new GameplayState();
            case TuckState.PASS_PLAY_SETUP:             return new PassAndPlaySetupState();
            case TuckState.PASS_PLAY_GAMEPLAY:          return new GameplayState();
            case TuckState.SINGLEPLAYER_SETUP:          break;//eturn new MultiplayerSetupState();
            case TuckState.SINGLEPLAYER_GAMEPLAY:       return new GameplayState();
            case TuckState.CREDITS:                     break;
        }

        Debug.LogError("Error: state ID: " + stateId + " does not exist!");
        return null;
    }
}
