using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbobusChosenState : AbobusState
{
    override public void HandleInput(Abobus abobus, InputAction.CallbackContext? value = null)
    {
        abobus.state = abobus.idle_state;
        Vector3 cur_pos = abobus.transform.position;
        abobus.transform.position += new Vector3(0, -10, 0);
    }
    // override public void Meow() 
    // {
    //     Debug.Log("Meow from chosen state");
    // }

}