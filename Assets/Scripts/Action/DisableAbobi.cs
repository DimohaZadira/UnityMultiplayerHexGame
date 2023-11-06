using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using UnityEngine;

public class DisableAbobi : IAction
{
    private HexCell applied_to;
    private GameManager game_manager;
    private Abobus abobus;
    private bool force = false;
    public DisableAbobi (HexCell applied_to, Abobus abobus, bool force = false)
    {
        this.applied_to = applied_to;
        this.force = force;
        this.abobus = abobus;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }
    
    public HexCell AppliedTo { 
        get => applied_to; 
        set {
            applied_to = value;
        } 
    }

    public string DebugMessage()
    {
        return "Disable all except " + abobus.abobus_name + " abobus";
    }

    public void Invoke()
    {
        Debug.Log("Select <color=green>" + abobus.abobus_name + " </color> abobus");
        if (force) {
            game_manager.DisableAbobi(null);
        } else {
            game_manager.DisableAbobi(abobus);
        }
        applied_to.actions.AddLast(new EnableAbobi(applied_to, abobus));
        
    }

    
}
