using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board
{
    public const float kScale = 300.0f;

    public const int kStartingPegIndex = 15;
    public const int kGoalTrackEntranceIndex = 12;
    public const int kPegsPerEdge = 22;
    public const int kPegStepSize = 27;
    
    public const int kMainTrackPipCount = 88;

    public List<BoardPieceGroup> pieceGroupList { get; private set; }


    private BoardPosition[] _mainTrack;
    private List<BoardPosition[]> _homeTrack;
    private List<BoardPosition[]> _goalTrack;
    private List<BoardPosition> _boardPositionList;

    public static Board Create(List<PlayerState> playerList)
    {
        Board board = new Board();

        board._mainTrack = new BoardPosition[kMainTrackPipCount];
        board._homeTrack = new List<BoardPosition[]>(PlayerGroup.kMaxPlayerCount);
        board._goalTrack = new List<BoardPosition[]>(PlayerGroup.kMaxPlayerCount);
        board.pieceGroupList = new List<BoardPieceGroup>(PlayerGroup.kMaxPlayerCount);    
        board._boardPositionList = new List<BoardPosition>(120);

        // Create Main track with starting pegs
        for(int i = 0; i < kMainTrackPipCount; ++i)
        {
            int owner = -1;
            PositionType type = PositionType.MAIN_TRACK;

            int snapTo = i - ((i + kPegsPerEdge) % kPegsPerEdge);
            int snapToPrefix = (i - snapTo) + 1;
            int startingPegIndex =  snapToPrefix % kStartingPegIndex;
            int goalStartIndex = snapToPrefix % kGoalTrackEntranceIndex;

            if (i != 0 && startingPegIndex == 0)
            {
                type = PositionType.START_PEG;
                owner = i / Board.kPegsPerEdge;
            }
            else if (i != 0 && goalStartIndex == 0)
            {
                type = PositionType.GOAL_TRACK_ENTRANCE;
                owner = i / Board.kPegsPerEdge; ;
            }

            board._mainTrack[i] = BoardPosition.Create(type, i, owner);
        }

        board._boardPositionList.AddRange(board._mainTrack);

        for (int i = 0; i < playerList.Count; ++i)
        {
            BoardPosition[] homeTrack = new BoardPosition[PlayerGroup.kMaxPlayerCount];
            BoardPosition[] goalTrack = new BoardPosition[PlayerGroup.kMaxPlayerCount];
        
            for(int j = 0; j < playerList.Count; ++j)
            {
                homeTrack[j] = BoardPosition.Create(PositionType.HOME, j, i);
                goalTrack[j] = BoardPosition.Create(PositionType.GOAL_TRACK, j, i);
            }

            board._homeTrack.Add(homeTrack);
            board._goalTrack.Add(goalTrack);

            board._boardPositionList.AddRange(homeTrack);
            board._boardPositionList.AddRange(goalTrack);


            BoardPieceGroup group = BoardPieceGroup.Create(i);
            board.pieceGroupList.Add(group);     
        }
        
        return board;
    }

    public List<BoardPosition> GetBoardPositionList()
    {
        return _boardPositionList;
    }

}
