using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusIdleState : AbobusState
{
    public AbobusIdleState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    override public void Enter() 
    {
        if (abobus.state == abobus.chosen_state) {
            gay_manager.ClearAllHighlightedCells();
            abobus.transform.position += new Vector3(0, -10, 0);
        }
        abobus.state = abobus.idle_state;
    }
    
    override public void HandleInput(HexCell hex_cell)
    {
        abobus.chosen_state.Enter();
    }

}