using System.Collections.Generic;
using UnityEngine;
using GameCommands;

public class CommandFactory
{
    private Stack<ICommand> _undoStack = new Stack<ICommand>();
    private Stack<ICommand> _redoStack = new Stack<ICommand>();

    public void Execute(ICommand command)
    {
        Debug.Log("Executing command: " + command.GetType().ToString());
        command.Execute();
        _undoStack.Push(command);
        _redoStack.Clear();
    }
   
    public bool Redo()
    {
        ICommand command;
        do
        {
            if(_redoStack.Count == 0)
            {
                Debug.Log("Redo Command Stack is empty!");
                return false;
            }

            command = _redoStack.Pop();
            
            Debug.Log("Redoing command: " + command.GetType().ToString());
    
            command.Execute();
            _undoStack.Push(command);
        }
        while (command.isLinked);

        return true;
    }

    public bool Undo()
    {
        
        ICommand command;
        do
        {
            if(_undoStack.Count == 0)
            {
                Debug.Log("Undo Command Stack is empty!");
                return false;
            }

            command = _undoStack.Pop();

            Debug.Log("Undoing command: " + command.GetType().ToString());
    
            command.Undo();
            _redoStack.Push(command);
        }
        while (command.isLinked);

        return true;
    }

    public void Clear()
    {
        _undoStack.Clear();
        _redoStack.Clear();
    }
}
