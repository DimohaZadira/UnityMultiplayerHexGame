using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearActions<T> : IAction
{
    private HexCell applied_to;
    private GameManager game_manager;
    private List<HexCell> to_clear;

    public ClearActions (HexCell applied_to, List<HexCell> cells)
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
        foreach (HexCell cell in to_clear) {
            cell.DeleteFromActions<T>();
        }
    }
}
