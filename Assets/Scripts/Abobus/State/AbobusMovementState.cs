using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusMovementState : AbobusState
{
    public AbobusMovementState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    
    override public void Enter()
    {
        abobus.state = abobus.movement_state;
    }
    
    override public void HandleInput(HexCell hex_cell)
    {
        abobus.MoveToHexCoordinates(hex_cell.hex_coordinates);
        abobus.chosen_state.Enter();
    }

}