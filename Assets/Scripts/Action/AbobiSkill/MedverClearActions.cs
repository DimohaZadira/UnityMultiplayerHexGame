using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedverClearActions<T> : IAction
{
    private HexCell applied_to;
    private GameManager game_manager;
    private List<HexCell> to_clear;

    public MedverClearActions (HexCell applied_to, List<HexCell> cells)
    {
        this.applied_to = applied_to;
        to_clear = cells;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    
    public HexCell AppliedTo { 
        get => applied_to; 
        set {
            applied_to = value;
        } 
    }

    public string DebugMessage()
    {
        return "Clear " + typeof(T).FullName + " actions";
    }

    public void Invoke()
    {
        Debug.Log("Clear <color=yellow>" + typeof(T).FullName + "</color> actions");

        foreach (HexCell cell in to_clear)
        {
            var actionsToRemove = new LinkedList<IAction>();

            foreach (var action in cell.actions)
            {
                if (action is T && !(action is ReturnHighlights))
                {
                    actionsToRemove.AddLast(action);
                }
            }

            foreach (var action in actionsToRemove)
            {
                cell.actions.Remove(action);
            }
        }
    }
}

