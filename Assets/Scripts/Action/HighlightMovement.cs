using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class HighlightMovement : IAction
{
    private HexCell applied_to;
    private GameManager game_manager;
    public HighlightMovement (HexCell applied_to)
    {
        this.applied_to = applied_to;
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
        return "Highlight movement";
    }

    public void Invoke()
    {
        Abobus abobus = applied_to.abobus;
        if (abobus) {
            game_manager.DisableAbobi(abobus.team, abobus);
            foreach (HexCoordinates hc in abobus.GetPossibleMovementTurns()) {
                HexCell cell = game_manager.hex_grid.GetCellByHexCoordinates(hc);
                cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_green);
                cell.actions.AddLast(new Movement(cell, abobus));
            }
        }
        applied_to.actions.AddLast(new UnhighlightMovement(applied_to));
    }

    
}
