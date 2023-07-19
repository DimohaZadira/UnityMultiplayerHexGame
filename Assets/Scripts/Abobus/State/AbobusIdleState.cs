using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusIdleState : AbobusState
{
    public AbobusIdleState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    override public void Enter() 
    {
        if (abobus.state == abobus.disabled_state) {
            return;
        }
        // Debug.Log("Idle state enter");
        abobus.state = abobus.idle_state;
        
    }
    
    override public void HandleInput(HexCell? hex_cell = null)
    {
        // Debug.Log("Switching to chosen state");
        abobus.chosen_state.Enter();
    }
    
    override public void Refresh()
    {

    }

}