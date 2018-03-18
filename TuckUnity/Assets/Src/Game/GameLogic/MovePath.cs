using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePath
{
    private List<BoardPosition> _path = new List<BoardPosition>(13);
    private int _currentIndex;
   

    public static MovePath Clone(MovePath path)
    {
        MovePath newPath = new MovePath();
        newPath._path.AddRange(path._path);
        return newPath;
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

    public BoardPosition Get(int index)
    {
        if(index < 0 || index >= _path.Count)
        {
            Debug.LogErrorFormat("Index: {0} is out of range!", index);
            return BoardPosition.Invalid;
        }
        return _path[index];
    }

    public void Start()
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

    public BoardPosition Tail
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
