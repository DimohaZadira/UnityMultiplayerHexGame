using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusSelectedState : AbobusState
{
    public AbobusSelectedState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    override public void Enter()
    { 
        gay_manager.ClearAllHighlightedCells();

        abobus.transform.position += new Vector3(0, 10, 0);
        

        if ( (!abobus.movement_state.entered) && (!abobus.skill_performing_state.entered) ) {
            Debug.Log($"Highlighting <color=yellow>Movement</color> cells", abobus);
            List<HexCoordinates> movement_coords_list = abobus.GetPossibleMovementTurns();
            foreach (HexCoordinates hex_coordinates in movement_coords_list) {
                HexCell hex_cell = gay_manager.hex_grid.GetCellByHexCoordinates(hex_coordinates);
                hex_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_green);
            }
        }
        
        if (abobus.movement_state.entered) {
            // точно походили, теперь либо возможно юзать скил, либо нет
            Debug.Log("Checking if the turn is over", abobus);
            if (abobus.GetPossibleSkillTriggerTurns().Count == 0) {
                // невозможно юзать скил, ход закончен
                gay_manager.SwitchTurn();
                return;
            } 
        }
        // подсветка ходов для апплая скила
        Debug.Log($"Highlighting <color=yellow>SkillTrigger</color> cells", abobus);
        List<HexCoordinates> skill_trigger_coords_list = abobus.GetPossibleSkillTriggerTurns();
        foreach (HexCoordinates hex_coords in skill_trigger_coords_list) {
            HexCell hex_cell = gay_manager.hex_grid.GetCellByHexCoordinates(hex_coords);
            hex_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
        }
        
    }
    
    override public void HandleInput(HexCell hex_cell = null)
    {   
        if (abobus.GetState() == abobus.disabled_state) {
            return;
        }
        if (abobus.GetState() == abobus.selected_state && hex_cell == null) {
            abobus.SwitchState(abobus.idle_state);
        }
        // Debug.Log("Highlighted state handles input");
        // gay_manager.ClearAllHighlightedCells();
        if (hex_cell != null) {
            if (hex_cell.GetComponent<HighlightableCell>().GetState() == HighlightableCell.State.highlighted_green) {
                abobus.SwitchState(abobus.movement_state);
                abobus.movement_state.HandleInput(hex_cell);
            }
            if (hex_cell.GetComponent<HighlightableCell>().GetState() == HighlightableCell.State.highlighted_yellow) {
                if (abobus.skill_performing_state.applied_to == null) {
                    abobus.skill_performing_state.applied_to = hex_cell;
                    abobus.SwitchState(abobus.skill_performing_state);
                }
            }
        }
    }

    override public void Refresh()
    {
    }

    override public void Exit()
    {
        gay_manager.ClearAllHighlightedCells();
        abobus.transform.position += new Vector3(0, -10, 0);
    }

}