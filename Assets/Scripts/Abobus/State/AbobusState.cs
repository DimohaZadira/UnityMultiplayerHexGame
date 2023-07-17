using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbobusState
{    
    public Abobus abobus;
    public GayManager gay_manager;
    public AbobusState(GayManager gm, Abobus abobus_)
    {
        gay_manager = gm;
        abobus = abobus_;
    }
    public abstract void HandleInput(HexCell hex_cell = null);
    public abstract void Enter();
}
