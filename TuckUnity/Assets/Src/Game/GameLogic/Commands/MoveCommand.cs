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

        private List<BoardPiece> killList = new List<BoardPiece>();

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
            playedCard = player.hand.GetCard(moveRequest.handIndex);
            Assert.IsNotNull(playedCard);

            if(moveRequest.movement != null)
            {
                BoardPieceGroup pieceGroup = board.GetPieceGroupList()[moveRequest.pieceIndex];
                BoardPiece piece = pieceGroup.GetPiece(moveRequest.pieceIndex);

                //validator.GetValidPaths(piece, )

            }


        }

        public void Undo()
        {

        }
    }
}
