using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class SelectAbobus : IAction
{
    private HexCell applied_to;
    private GameManager game_manager;
    private Abobus abobus;
    public SelectAbobus (HexCell applied_to, Abobus abobus)
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
        return "Select " + abobus.abobus_name + " abobus";
    }

    public void Invoke()
    {
        Debug.Log("Select <color=green>" + abobus.abobus_name + " </color> abobus");
        
        game_manager.DisableAbobi(abobus);
        abobus.transform.position += new Vector3(0, 10, 0);
        game_manager.selected_abobus = abobus;        
        applied_to.actions.AddLast(new UnselectAbobus(applied_to, abobus));
        
    }

    
}
