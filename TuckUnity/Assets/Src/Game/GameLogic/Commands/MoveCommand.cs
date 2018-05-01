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
        private CardData playedCard;
        private PlayerState player;

        private List<BoardPiece> hitList = new List<BoardPiece>();
        private Dictionary<BoardPiece, BoardPosition> killedPositionMap = new Dictionary<BoardPiece, BoardPosition>();
        private List<MoveRequest.PiecePathData> piecePathList;

        public static MoveCommand Create(MoveRequest request, Board board, PlayerGroup group, MoveValidator validator)
        {
            MoveCommand command = new MoveCommand();
            command.board = board;
            command.playerGroup = group;
            command.moveRequest = request;
            command.validator = validator;
            return command;
        }

        public bool isLinked
        {
            get { return false; }
        }

        public void Execute()
        {
            player = playerGroup.GetPlayerByIndex(moveRequest.playerIndex);

            Assert.IsNotNull(player);
            playedCard = player.hand.GetCard(moveRequest.handIndex);
            Assert.IsNotNull(playedCard);

            if(moveRequest.piecePathList != null)
            {
                piecePathList = moveRequest.piecePathList;
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
            if(moveRequest.piecePathList != null)
            {
                piecePathList = moveRequest.piecePathList;
                for(int i = 0; i < piecePathList.Count; ++i)
                {
                    var piecePath = piecePathList[i];
                    List<BoardPieceGroup> pieceGroupList = board.GetPieceGroupList();
                    BoardPieceGroup pieceGroup = pieceGroupList[player.index];
                    BoardPiece piece = pieceGroup.GetPiece(piecePath.pieceIndex);

                    board.SetPiecePosition(piece, piecePath.path.start);
                }
            }
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
