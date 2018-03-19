using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GhostGen;

public class Board : NotificationDispatcher
{
    public const int kStartingPegIndex          = 15;
    public const int kGoalTrackEntranceIndex    = 12;
    public const int kPegsPerEdge               = 22;
    public const int kMainTrackPipCount         = 88;
    public const int kTotalPegCount             = 120;
    public const int kPerPlayerGoalCount        = 4;

    private List<BoardPosition>     _mainTrack          = new List<BoardPosition>(kMainTrackPipCount);
    private List<BoardPosition[]>   _homeTrack          = new List<BoardPosition[]>(PlayerGroup.kMaxPlayerCount);
    private List<BoardPosition[]>   _goalTrack          = new List<BoardPosition[]>(PlayerGroup.kMaxPlayerCount);
    private List<BoardPosition>     _boardPositionList  = new List<BoardPosition>(kTotalPegCount);
    private List<BoardPieceGroup>   _pieceGroupList     = new List<BoardPieceGroup>(PlayerGroup.kMaxPlayerCount);
    private List<BoardPosition>     _startingPositions  = new List<BoardPosition>(PlayerGroup.kMaxPlayerCount);
    private Dictionary<BoardPosition, BoardPiece> _positionPieceMap = new Dictionary<BoardPosition, BoardPiece>(kTotalPegCount);

    public static Board Create(List<PlayerState> playerList)
    {
        Board board = new Board();
        
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

            BoardPosition position = BoardPosition.Create(type, i, owner);
            board._mainTrack.Add(position);
            if(type == PositionType.START_PEG)
            {
                board._startingPositions.Add(position);
            }
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

            BoardPieceGroup group = BoardPieceGroup.Create(board, i);
            board._pieceGroupList.Add(group);     
        }
        
        return board;
    }

    public List<BoardPosition> GetBoardPositionList()
    {
        return _boardPositionList;
    }

    public List<BoardPieceGroup> GetPieceGroupList()
    {
        return _pieceGroupList;
    }

    public void SetPiecePosition(BoardPiece piece, BoardPosition pos )
    {
        piece.boardPosition = pos;        
        _positionPieceMap[pos] = piece;
    }

    public BoardPiece GetPieceAtPosition(BoardPosition pos)
    {
        BoardPiece piece = null;
        _positionPieceMap.TryGetValue(pos, out piece);
        return piece;
    }

    public BoardPosition GetStartingPosition(int playerIndex)
    {
        return _startingPositions[playerIndex];
    }

    public bool GetNextPositions(BoardPosition position, int playerIndex, bool forward, ref List<BoardPosition> result)
    {
        bool positionsFound = false;
        BoardPosition goalPos;
        BoardPosition trackPos;

        int nextIndex = (forward) ? 1 : -1;
        int nextWrappedIndex = BoardPositionUtil.GetWrappedMainTrackIndex(position.trackIndex + nextIndex);
        Debug.Log("Test: " + BoardPositionUtil.GetWrappedMainTrackIndex(-1));
        switch(position.type)
        {
            case PositionType.GOAL_TRACK_ENTRANCE:
                {
                    if(position.ownerIndex == playerIndex && forward)
                    {
                        goalPos = _goalTrack[playerIndex][0];
                        result.Add(goalPos);
                    }trackPos = _mainTrack[nextWrappedIndex];
                    result.Add(trackPos);
                    positionsFound = true;
                }
                break;
            case PositionType.GOAL_TRACK:
                {
                    if(position.trackIndex < kPerPlayerGoalCount - 1 && forward)
                    {
                        int goalIndex = position.trackIndex + nextIndex;
                        goalPos = _goalTrack[playerIndex][goalIndex];

                        BoardPiece tmpPiece;
                        if(!IsPositionOccupied(goalPos, out tmpPiece))
                        {
                            result.Add(goalPos);
                            positionsFound = true;
                        }
                    }
                }
                break;
            case PositionType.MAIN_TRACK:
            case PositionType.START_PEG:
                {
                    result.Add(_mainTrack[nextWrappedIndex]);
                    positionsFound = true;
                }
                break;
        }
        return positionsFound;
    }


    public bool IsPositionOccupied(BoardPosition position, out BoardPiece piece)
    {
        piece = GetPieceAtPosition(position);
        return piece != null;
    }
}
