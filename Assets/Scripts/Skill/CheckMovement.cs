using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMovement : IAction
{
    HexCell applied_to;
    private GameManager game_manager;
    private Abobus abobus;
    public CheckMovement (HexCell applied_to)
    {
        this.applied_to = applied_to;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        abobus = game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates);
    }

    public HexCell AppliedTo { 
        get {
            return applied_to;
        }
        set {
            applied_to = value;
        }
    }

    public string DebugMessage()
    {
        return "Will check movement";
    }

    public void Invoke ()
    {
        Debug.Log("Checking movement");
        if (abobus) {            
            foreach (HexCoordinates turn_coords in abobus.GetPossibleMovementTurns()) {
                HexCell hc = game_manager.hex_grid.GetCellByHexCoordinates(turn_coords);
                hc.actions.Clear();
                hc.actions.Add(new Movement(abobus, hc, applied_to));

                HighlightableCell turn_highlightable_cell = hc.GetComponent<HighlightableCell>();
                turn_highlightable_cell.SetState(HighlightableCell.State.highlighted_green);
                Debug.Log("set green state");
            }
            applied_to.actions.Clear();
            applied_to.actions.Add(new ClearAllHighlighted(applied_to));
            applied_to.actions.Add(new SetCheckMovement(applied_to));
        }
    }
}
