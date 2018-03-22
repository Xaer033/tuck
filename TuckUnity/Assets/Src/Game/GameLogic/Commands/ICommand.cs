using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommands
{
    public interface ICommand
    {
        bool isLinked { get; }

        void Execute();
        void Undo();
    }
}