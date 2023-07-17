using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusSkillPerformingState : AbobusState
{
    public AbobusSkillPerformingState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    
    override public void Enter()
    {
        Debug.Log("Skill performing state enter");
        abobus.moved_this_turn = true;
        abobus.state = abobus.movement_state;
        gay_manager.DisableAbobi(abobus);
    }
    
    override public void HandleInput(HexCell hex_cell)
    {
        Debug.Log("Skill performing state handles input");
        abobus.chosen_state.Enter();
    }

}