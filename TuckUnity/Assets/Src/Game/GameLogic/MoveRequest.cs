using System.Collections.Generic;

[System.Serializable]
public class MoveRequest
{
    public int playerIndex      { get; set; }
    public int handIndex        { get; set; }

    public List<PiecePathData> piecePathList { get; set; }

    [System.Serializable]
    public class PiecePathData
    {
        public int ownerIndex;
        public int pieceIndex;
        public MovePath path;
    }

    public static MoveRequest Create(int playerIndex, int handIndex, List<PiecePathData> pieceMoveList)
    {
        MoveRequest request = new MoveRequest();
        request.playerIndex = playerIndex;
        request.handIndex = handIndex;
        request.piecePathList = pieceMoveList;

        return request;
    }
}
