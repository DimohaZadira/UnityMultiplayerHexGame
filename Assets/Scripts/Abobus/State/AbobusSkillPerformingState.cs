using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusSkillPerformingState : AbobusState
{
    public AbobusSkillPerformingState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {
        Refresh();
    }
    
    public HexCell applied_to;

    override public void Enter()
    {
        if (applied_to == null) {
            Debug.Log($"<color=red>ERROR</color> Cannot perform skill with applied_to = null!");
            return;
        }
        // entered = true;
        abobus.transform.position += new Vector3(0, 10, 0);

        Debug.Log($"Highlighting <color=yellow>SkillPerformingCells</color>");
        List<HexCoordinates> skill_performing_coords_list = abobus.GetPossibleSkillTurns(applied_to);
        foreach (HexCoordinates hex_coords in skill_performing_coords_list) {
            HexCell hex_cell = gay_manager.hex_grid.GetCellByHexCoordinates(hex_coords);
            hex_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
        }
    }
    
    override public void HandleInput(HexCell hex_cell = null)
    {
        if (abobus.GetState() == abobus.disabled_state) {
            return;
        }
        if (abobus.GetState() == abobus.skill_performing_state && hex_cell == null) {
            abobus.SwitchState(abobus.idle_state);
        }
        if (hex_cell != null) {
            if (hex_cell.GetComponent<HighlightableCell>().GetState() == HighlightableCell.State.highlighted_yellow) {
                Debug.Log("Skill performing state handles input", abobus);
                gay_manager.DisableAbobi(abobus.team, abobus);
                abobus.PerformSkill(applied_to, hex_cell);
                gay_manager.SwitchTurn();
            }
        }
        
    }

    override public void Refresh()
    {
        applied_to = null;
        // entered = false;
    }
    override public void Exit()
    {
        gay_manager.ClearAllHighlightedCells();
        applied_to = null;
        abobus.transform.position += new Vector3(0, -10, 0);
    }
}