using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EnableAbobi : IAction
{
    private HexCell applied_to;
    private GameManager game_manager;
    private Abobus abobus;
    public EnableAbobi (HexCell applied_to, Abobus abobus)
    {
        this.applied_to = applied_to;
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
        return "Enable all except " + abobus.abobus_name + " abobus";
    }

    public void Invoke()
    {
        Debug.Log("Select <color=green>" + abobus.abobus_name + " </color> abobus");
        game_manager.EnableAbobi(abobus);
    }

    
}
