using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class HighlightCells : IAction
{
    private HexCell applied_to;
    private GameManager game_manager;
    private Abobus abobus;
    private List<HighlightableCell> cells;
    private  HighlightableCell.State state;
    public HighlightCells (HexCell applied_to, Abobus abobus, List<HighlightableCell> cells, HighlightableCell.State state)
    {
        this.applied_to = applied_to;
        this.abobus = abobus;
        this.state = state;
        this.cells = cells;
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
        return "Highlight cell " + applied_to.hex_coordinates.ToString() + " into " + state;
    }

    public void Invoke()
    {
        foreach(HighlightableCell cell in cells) {
            cell.SetState(state);
        }
    }

    
}
