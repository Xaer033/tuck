using System.Collections.Generic;
using UnityEngine.Assertions;

namespace GameCommands
{
    public class MoveCommand : ICommand
    {
        private MoveValidator validator;
        private Board board;
        private PlayerGroup playerGroup;
        private MoveRequest moveRequest;
        private CardDeck deck;
        private CardData playedCard;
        private PlayerState player;

        private List<BoardPiece> hitList = new List<BoardPiece>();
        private Dictionary<BoardPiece, BoardPosition> killedPositionMap = new Dictionary<BoardPiece, BoardPosition>();
        private List<MoveRequest.PiecePathData> piecePathList = new List<MoveRequest.PiecePathData>();

        public static MoveCommand Create(MoveRequest request, Board board, PlayerGroup group, MoveValidator validator, CardDeck deck)
        {
            MoveCommand command = new MoveCommand();
            command.board = board;
            command.playerGroup = group;
            command.moveRequest = request;
            command.validator = validator;
            command.deck = deck;
            return command;
        }

        public bool isLinked
        {
            get { return false; }
        }

        public void Execute()
        {
            player = playerGroup.GetPlayerByIndex(moveRequest.playerIndex);
            
            // Remove card from hand
            Assert.IsNotNull(player);
            playedCard = player.hand.ReplaceCard(moveRequest.handIndex, null);
            Assert.IsNotNull(playedCard);
            deck.Discard(playedCard);

            if(moveRequest.piecePathList != null)
            {
                piecePathList.AddRange(moveRequest.piecePathList);

                for(int i = 0; i < piecePathList.Count; ++i)
                {
                    var piecePath = piecePathList[i];
                    List<BoardPieceGroup> pieceGroupList = board.GetPieceGroupList();
                    BoardPieceGroup pieceGroup = pieceGroupList[player.index];
                    BoardPiece piece = pieceGroup.GetPiece(piecePath.pieceIndex);

                    bool isKiller = _isSplitStomp(playedCard);
                    // Kill Other Pieces
                    if(validator.GetPieceHitList(piecePath.path, isKiller, ref hitList))
                    {
                        _killPieces(hitList);
                    }

                    // Move Piece
                    BoardPosition newPiecePos = piecePath.path.end;
                    board.SetPiecePosition(piece, newPiecePos);
                }
            }
        }

        public void Undo()
        {
            //Un-kill things
            foreach(var entry in killedPositionMap)
            {
                board.SetPiecePosition(entry.Key, entry.Value);
            }

            //Move back to the old positions
            if(piecePathList.Count > 0)
            {
                for(int i = piecePathList.Count - 1; i >= 0; --i)
                {
                    var piecePath = piecePathList[i];
                    List<BoardPieceGroup> pieceGroupList = board.GetPieceGroupList();
                    BoardPieceGroup pieceGroup = pieceGroupList[player.index];
                    BoardPiece piece = pieceGroup.GetPiece(piecePath.pieceIndex);
                    BoardPosition newPiecePos = piecePath.path.start;

                    board.SetPiecePosition(piece, newPiecePos);
                }
            }

            // Un discard played card            
            CardData card = deck.PopDiscard();
            player.hand.ReplaceCard(moveRequest.handIndex, card);
        }

        private void _killPieces(List<BoardPiece> pieces)
        {
            for(int i = 0; i < pieces.Count; ++i)
            {
                BoardPiece piece = pieces[i];
                killedPositionMap[piece] = piece.boardPosition;
                board.KillPiece(piece);
            }
        }

        private bool _isSplitStomp(CardData card)
        {
            foreach(var movement in card.pieceMovementList)
            {
                if(movement.type == MoveType.SPLIT_STOMP)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
