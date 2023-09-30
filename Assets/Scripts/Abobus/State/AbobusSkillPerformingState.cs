using System;
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
    public bool skill_performed;

    override public void Enter()
    {
        Debug.Log("Entering <color=yellow>SkillPerformingState</color>");
        if (applied_to == null) {
            Debug.Log($"<color=red>ERROR</color> Cannot perform skill with applied_to = null!");
            return;
        }
        gay_manager.DisableAbobi(abobus.team, abobus);
        
        abobus.transform.position += new Vector3(0, 10, 0);
        Debug.Log($"Highlighting <color=yellow>SkillPerformingCells</color>");
        List<HexCoordinates> skill_performing_coords_list = abobus.GetPossibleSkillTurns(applied_to);
        if (skill_performing_coords_list.Count == 0) {
            gay_manager.SwitchTurn();
        }
        foreach (HexCoordinates hex_coords in skill_performing_coords_list) {
            HexCell hex_cell = gay_manager.hex_grid.GetCellByHexCoordinates(hex_coords);
            hex_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
        }
        HandleInput(applied_to);
    }
    
    override public void HandleInput(HexCell hex_cell = null)
    {
        if (abobus.GetState() == abobus.disabled_state) {
            return;
        }
        if (abobus.GetState() == abobus.skill_performing_state && hex_cell == null) {
            return;
        }
        if (hex_cell != null) {
            if (hex_cell.GetComponent<HighlightableCell>().GetState() == HighlightableCell.State.highlighted_yellow
            || (!entered && abobus.perform_skill_on_enter)){
                Debug.Log("Skill performing state handles input", abobus);
                entered = true;
                gay_manager.DisableAbobi(abobus.team, abobus);
                skill_performed = abobus.PerformSkill(applied_to, hex_cell);
                if (skill_performed) {
                    gay_manager.SwitchTurn();
                }
                applied_to = hex_cell;
                abobus.SwitchState(abobus.skill_performing_state);
            }
        }
        
    }

    override public void Refresh()
    {
        applied_to = null;
        skill_performed = false;
        entered = false;
    }
    override public void Exit()
    {
        Debug.Log("Exiting <color=yellow>SkillPerformingState</color>");
        gay_manager.ClearAllHighlightedCells();
        if (skill_performed) {
            applied_to = null;
        }
        abobus.transform.position += new Vector3(0, -10, 0);
    }
}