using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusDisabledState : AbobusState
{
    public AbobusDisabledState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    override public void Enter() 
    {
        if (abobus.state == abobus.chosen_state) {
            gay_manager.ClearAllHighlightedCells();
            abobus.transform.position += new Vector3(0, -10, 0);
        }
        abobus.state = abobus.disabled_state;
    }
    
    override public void HandleInput(HexCell hex_cell)
    {
    }

}