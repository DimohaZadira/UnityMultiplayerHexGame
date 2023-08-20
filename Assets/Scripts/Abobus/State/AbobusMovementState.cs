using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusMovementState : AbobusState
{
    public AbobusMovementState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {
        Refresh();
    }
    override public void Enter()
    {
        gay_manager.DisableAbobi(abobus.team, abobus);
        entered = true;
    }
    
    override public void HandleInput(HexCell hex_cell = null)
    {
        if (hex_cell != null) {
            Debug.Log("Movement state handles input", abobus);
            abobus.MoveToHexCoordinates(hex_cell.hex_coordinates);
        }
        abobus.SwitchState(abobus.selected_state);
    }

    override public void Refresh()
    {
        entered = false;
    }
    
    override public void Exit()
    {
    }

}