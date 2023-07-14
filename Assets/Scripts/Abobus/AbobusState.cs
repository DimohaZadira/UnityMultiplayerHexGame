using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class AbobusState
{    
    public AbobusState()
    {
        Debug.Log("AbobusState created");
    }
    public abstract void HandleInput(Abobus abobus, InputAction.CallbackContext? value = null);
    
    public virtual void Meow() 
    {
        Debug.Log("Meow");
    }
}
