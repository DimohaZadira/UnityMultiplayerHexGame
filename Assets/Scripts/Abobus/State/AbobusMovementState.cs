using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusMovementState : AbobusState
{
    public AbobusMovementState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {
        Refresh();
    }
    
    public bool entered;
    override public void Enter()
    {
        Debug.Log("Movement state enter");
        this.entered = true;
        abobus.state = abobus.movement_state;
    }
    
    override public void HandleInput(HexCell hex_cell = null)
    {
        if (hex_cell != null) {
            Debug.Log("Movement state handles input");
            abobus.MoveToHexCoordinates(hex_cell.hex_coordinates);
            gay_manager.DisableAbobi(abobus);
            // abobus.chosen_state.Enter();
        }
        
    }
    override public void Refresh()
    {
        entered = false;
    }

}