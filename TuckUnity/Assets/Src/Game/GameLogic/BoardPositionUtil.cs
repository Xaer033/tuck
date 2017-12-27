using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardPositionUtil
{
    public static Vector3 GetWorldPosition(BoardPosition position)
    {
        Vector3 result = Vector3.zero;
        Vector3 dir = _getDirectionVector(position);
        Vector3 orthagonal = new Vector3(dir.y, -dir.x, 0.0f);
        Vector3 center;

       
        if(position.type == PositionType.HOME)
        {
            center = dir * Board.kScale * 0.65f;
            Vector3 offset = directions[position.trackIndex] * 2.0f;

            result = center + (((orthagonal * 5.0f) + offset) * Board.kPegStepSize);

        }
        else if(position.type == PositionType.GOAL_TRACK)
        {
            center = dir * Board.kScale * 0.65f;
            Vector3 offset = directions[position.trackIndex] * (position.trackIndex * Board.kPegStepSize);

            result = center + (dir * position.trackIndex * Board.kPegStepSize);
        }
        else
        {
            int orthoIndex = position.trackIndex / Board.kPegsPerEdge;
            int sideOffset = position.trackIndex % Board.kPegsPerEdge;
            center = new Vector3(dir.x, dir.y, 0.0f) * Board.kScale;

            float percentage = (float)sideOffset / (float)Board.kPegsPerEdge;
            float fullRange = percentage * 2.0f - 1.0f;
            Vector3 offset = orthagonal * fullRange * Board.kScale;// Board.kPegStepSize * Board.kPegsPerEdge;
            result = center + offset;
        }

        //if ((i + 1) % kPegsPerEdge == 0)
        //{
        //    sideIndex++;
        //}

        return result;
    }

    static private Vector3[] directions
    {
        get
        {
            Vector3[] d = { Vector3.down, Vector3.left, Vector3.up, Vector3.right };
            return d;
        }
    }

    //static private Vector3[] orthagonal
    //{
    //    get
    //    {
    //        Vector3[] d = { Vector3.left, Vector3.up, Vector3.right, Vector3.down };
    //        return d;
    //    }
    //}

    private static Vector3 _getDirectionVector(BoardPosition position)
    {
        Vector3 result = Vector3.zero;
        int index = 0;
        switch(position.type)
        {
            case PositionType.GOAL_TRACK:
            case PositionType.HOME:
                index = position.ownerIndex;
                break;
            case PositionType.START_PEG:
            case PositionType.GOAL_TRACK_ENTRANCE:
            case PositionType.MAIN_TRACK:
            {
                index = position.trackIndex / Board.kPegsPerEdge;
                break;
            }
        }
        return directions[index];
    }
}
