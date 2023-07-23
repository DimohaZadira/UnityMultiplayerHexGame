using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusDisabledState : AbobusState
{
    public AbobusDisabledState(GayManager gm, Abobus abobus_) : base(gm, abobus_) {}
    override public void Enter() 
    {
        entered = true;
    }
    override public void HandleInput(HexCell hex_cell = null)
    {
    }
    override public void Refresh()
    {
        entered = false;
    }
    override public void Exit()
    {

    }

}