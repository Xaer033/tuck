using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MovePath
{
    [SerializeField]
    private List<BoardPosition> _path;

    [SerializeField]
    private int _currentIndex;

    public static MovePath Clone(MovePath path)
    {
        MovePath newPath = new MovePath();
        newPath._path.AddRange(path._path);
        return newPath;
    }

    public MovePath()
    {
        _currentIndex = 0;
        _path = new List<BoardPosition>(13);
    }

    public MovePath(List<BoardPosition> pathList)
    {
        _currentIndex = 0;
        _path = pathList;
    }

    public int positionCount
    {
        get { return _path.Count; }
    }

    public void Add(BoardPosition pos)
    {
        _path.Add(pos);
    }

    public void RemoveAt(int index)
    {
        _path.RemoveAt(index);
    }

    public void Reverse()
    {
        _path.Reverse();
    }

    public BoardPosition Get(int index)
    {
        if(index < 0 || index >= _path.Count)
        {
            Debug.LogErrorFormat("Index: {0} is out of range!", index);
            return BoardPosition.Invalid;
        }
        return _path[index];
    }

    public void Reset()
    {
        _currentIndex = 0;
    }

    public bool GetNext(out BoardPosition pos)
    {
        if(_currentIndex < _path.Count)
        {
            pos = Get(_currentIndex);
            _currentIndex++;
            return true;
        }
        else
        {
            pos = BoardPosition.Invalid;
            return false;
        }
    }

    public BoardPosition start
    {
        get
        {
            if(_path.Count == 0)
            {
                return BoardPosition.Invalid;
            }

            return _path[0];
        }
    }
    public BoardPosition end
    {
        get
        {
            if(_path.Count == 0)
            {
                return BoardPosition.Invalid;
            }

            return _path[positionCount - 1];
        }
    }

}
