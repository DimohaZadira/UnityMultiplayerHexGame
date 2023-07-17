using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusChosenState : AbobusState
{
    public AbobusChosenState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    override public void Enter()
    {
        abobus.transform.position += new Vector3(0, 10, 0);
        if (abobus.state == abobus.idle_state) {
            // Debug.Log("Entered chosen state from idle state");
            List<HexCoordinates> movement_coords_list = abobus.GetPossibleMovementTurns();
            foreach (HexCoordinates hex_coordinates in movement_coords_list) {
                HexCell hex_cell = gay_manager.hex_grid.GetCellByHexCoordinates(hex_coordinates);
                hex_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_green);
            }
        }
        if (abobus.state == abobus.movement_state) {
            gay_manager.ClearAllHighlightedCells();
            if (abobus.GetPossibleSkillTurns().Count == 0) {
                abobus.state = abobus.chosen_state;
                abobus.idle_state.Enter();
                return;
            }
        }
        List<HexCoordinates> skill_coords_list = abobus.GetPossibleSkillTurns();
        foreach (HexCoordinates hex_coords in skill_coords_list) {
            HexCell hex_cell = gay_manager.hex_grid.GetCellByHexCoordinates(hex_coords);
            hex_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
        }
        abobus.state = abobus.chosen_state;
    }
    
    override public void HandleInput(HexCell hex_cell)
    {
        if (hex_cell.GetComponent<HighlightableCell>().GetState() == HighlightableCell.State.highlighted_green) {
            abobus.movement_state.Enter();
            abobus.movement_state.HandleInput(hex_cell);
        }
    }

}