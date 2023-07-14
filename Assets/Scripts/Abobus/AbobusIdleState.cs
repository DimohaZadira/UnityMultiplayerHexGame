using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusIdleState : AbobusState
{
    public AbobusIdleState()
    {
        Debug.Log("idle state created");
    }
    ~AbobusIdleState()
    {
        Debug.Log("idle state died");
    }
    override public void HandleInput(Abobus abobus, InputAction.CallbackContext? value = null)
    {
        abobus.state = abobus.chosen_state;
        abobus.transform.position += new Vector3(0, 10, 0);
    }
    override public void Meow() 
    {
        Debug.Log("Meow from idle state");
    }

}