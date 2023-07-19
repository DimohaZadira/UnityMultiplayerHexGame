using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusSkillPerformingState : AbobusState
{
    public AbobusSkillPerformingState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {
        Refresh();
    }
    
    public bool entered;
    public HexCell applied_to;

    override public void Enter()
    {
        if (applied_to == null) {
            Debug.Log("ERROR Cannot perform skill with applied_to = null!");
            return;
        }
        entered = true;
        Debug.Log("Skill performing state enter");
        gay_manager.DisableAbobi(abobus);

        abobus.state = abobus.skill_performing_state;
        // abobus.chosen_state.Enter();
    }
    
    override public void HandleInput(HexCell hex_cell = null)
    {
        if (hex_cell != null) {
            if (hex_cell.GetComponent<HighlightableCell>().GetState() == HighlightableCell.State.highlighted_green) {
                Debug.Log("Skill performing state handles input");
                abobus.PerformSkill(applied_to, hex_cell);
                applied_to = null;
            }
        }
        
    }

    override public void Refresh()
    {
        applied_to = null;
        entered = false;
    }
}