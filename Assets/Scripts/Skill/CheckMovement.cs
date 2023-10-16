using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckMovement : Action
{
    HexCell applied_to;
    public CheckMovement (HexCell applied_to)
    {
        this.applied_to = applied_to;
    }
    override public void Invoke ()
    {
        GameManager game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        Abobus abobus = game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates);
        if (abobus) {
            game_manager.ClearAllHighlightedCells();
            game_manager.ClearAllActions();
            foreach (HexCoordinates turn_coords in abobus.GetPossibleMovementTurns()) {
                HexCell hc = game_manager.hex_grid.GetCellByHexCoordinates(turn_coords);
                hc.actions.Clear();
                hc.actions.Add(new Movement(abobus, hc, applied_to));

                HighlightableCell turn_highlightable_cell = hc.GetComponent<HighlightableCell>();
                turn_highlightable_cell.SetState(HighlightableCell.State.highlighted_green);
            }
            applied_to.actions.Clear();
            applied_to.actions.Add(new UncheckMovement(applied_to));
        }

        

    }
}
