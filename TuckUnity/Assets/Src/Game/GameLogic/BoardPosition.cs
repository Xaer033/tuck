using System;

public enum PositionType
{
    MAIN_TRACK,
    HOME,
    START_PEG,
    GOAL_TRACK,
    GOAL_TRACK_ENTRANCE
}

[System.Serializable]
public struct BoardPosition : IEquatable<BoardPosition>
{
    readonly public PositionType type;
    readonly public int trackIndex;
    readonly public int ownerIndex;

    //public BoardPosition()
    //{
    //    type = PositionType.HOME;
    //    trackIndex = 0;
    //    ownerIndex = -1;
    //}

    public override string ToString()
    {
        return "Type: " + type.ToString() + ", \nTrackIndex: " + trackIndex + "\nOwnerIndex:" + ownerIndex;
    }

    public static BoardPosition Invalid
    {
        get
        {
            return Create(PositionType.HOME, -1, -1);
        }
    }

    public static bool IsInvalid(BoardPosition pos)
    {
        return pos.trackIndex == -1 && pos.type == PositionType.HOME && pos.ownerIndex == -1;
    }

    public static bool IsSame(BoardPosition a, BoardPosition b)
    {
        return  a.type == b.type && 
                a.trackIndex == b.trackIndex && 
                a.ownerIndex == b.ownerIndex;
    }

    public static BoardPosition Create(
        PositionType p_type,
        int p_trackIndex,
        int p_playerIndex = -1)
    {
        return new BoardPosition(p_type, p_trackIndex, p_playerIndex);
    }

    public BoardPosition(
        PositionType p_type,
        int p_trackIndex,
        int p_playerIndex = -1)
    {
        type = p_type;
        trackIndex = p_trackIndex;
        ownerIndex = p_playerIndex;
    }

    public bool Equals(BoardPosition other)
    {
        return IsSame(this, other);
    }

    public override bool Equals(object obj)
    {
        if(ReferenceEquals(null, obj))
            return false;
        if(obj.GetType() != typeof(BoardPosition))
            return false;
        return Equals((BoardPosition)obj);
    }

    public override int GetHashCode()
    {
        unchecked
        {
            return (trackIndex.GetHashCode() * type.GetHashCode() * 397) ^ ownerIndex.GetHashCode();
        }
    }

    public static bool operator ==(BoardPosition left, BoardPosition right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(BoardPosition left, BoardPosition right)
    {
        return !left.Equals(right);
    }
}
