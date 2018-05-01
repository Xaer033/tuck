using System.Collections.Generic;
using GameCommands;

public class TuckMatchCore 
{
    public TuckMatchState matchState { get; private set; }


    private CommandFactory _commandFactory = new CommandFactory();
    
    public static TuckMatchCore Create(
        List<PlayerState> playerList, 
        CardDeck cardDeck)
    { 
        TuckMatchCore core = new TuckMatchCore();

        // Also no commands for starting player hands
        core.matchState = TuckMatchState.Create(playerList, cardDeck);





        //var board = core.matchState.board;

        //var murderer = board.GetPieceGroupList()[0].GetPiece(1);
        //board.SetPiecePosition(murderer, BoardPosition.Create(PositionType.MAIN_TRACK, 12));
        

        //var victim = board.GetPieceGroupList()[0].GetPiece(0);
        //board.SetPiecePosition(victim, BoardPosition.Create(PositionType.MAIN_TRACK, 18));
        

        //var pieceMovement = new PieceMovementData();
        //pieceMovement.type = MoveType.FORWARDS;
        //pieceMovement.value = 6;

        //var pathList = new List<MovePath>();
        //core.matchState.validator.GetValidPaths(murderer, pieceMovement, ref pathList);

        //var pathData = new MoveRequest.PiecePathData();
        //pathData.pieceIndex = 1;
        //pathData.path = pathList[0];

        //var pieceMoveList = new List<MoveRequest.PiecePathData>();
        //pieceMoveList.Add(pathData);

        //MoveRequest request = MoveRequest.Create(0, 0, pieceMoveList);
        //core.ApplyMoveCommand(request);

        return core;
    }

    private TuckMatchCore() { }

    public void ClearCommands()
    {
        _commandFactory.Clear();
    }

    public void AddTradeRequest(TradeRequest request)
    {
        ICommand command = AddTradeRequestCommand.Create(matchState.escrow, request);
        _commandFactory.Execute(command);
    }

    public void ApplyTrade()
    {
        ICommand command = TradeCardsCommand.Create(matchState.escrow);
        _commandFactory.Execute(command);
    }

    public void ApplyNextPlayerTurn()
    {
        ICommand command = ChangePlayerTurn.Create(matchState.playerGroup);
        _commandFactory.Execute(command);
    }

    public void ApplyChangeMatchMode(GameMatchMode newMode)
    {
        ICommand command = ChangeMatchMode.Create(matchState, newMode);
        _commandFactory.Execute(command);
    }

    public void ApplyMoveCommand(MoveRequest request)
    {
        ICommand command = MoveCommand.Create(request, matchState.board, matchState.playerGroup, matchState.validator);
        _commandFactory.Execute(command);
    }

    public bool Undo()
    {
        return _commandFactory.Undo();
    }

    public bool Redo()
    {
        return _commandFactory.Redo();
    }
}
