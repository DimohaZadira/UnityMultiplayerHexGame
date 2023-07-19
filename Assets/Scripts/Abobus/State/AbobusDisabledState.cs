using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusDisabledState : AbobusState
{
    public AbobusDisabledState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    override public void Enter() 
    {
        abobus.state = abobus.disabled_state;
    }
    
    override public void HandleInput(HexCell? hex_cell = null)
    {
    }
    override public void Refresh()
    {

    }

}