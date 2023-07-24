using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusIdleState : AbobusState
{
    public AbobusIdleState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    override public void Enter() 
    {
         
    }
    override public void HandleInput(HexCell hex_cell = null)
    {
        if (abobus.skill_performing_state.entered) {
            abobus.SwitchState(abobus.selected_state);
        } else {
            abobus.SwitchState(abobus.selected_state);
        }
    }

    
    override public void Refresh()
    {

    }

    override public void Exit()
    {

    }

}