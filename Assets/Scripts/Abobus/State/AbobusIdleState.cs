using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusIdleState : AbobusState
{
    public AbobusIdleState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    override public void Enter() 
    {
        if (abobus.GetState() == abobus.disabled_state) {
            return;
        }        
    }
    override public void HandleInput(HexCell hex_cell = null)
    {
        abobus.SwitchState(abobus.selected_state);
    }

    
    override public void Refresh()
    {

    }

    override public void Exit()
    {

    }

}