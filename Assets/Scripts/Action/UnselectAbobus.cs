using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class UnselectAbobus : IAction
{
    private HexCell applied_to;
    private GameManager game_manager;
    private Abobus abobus;
    public UnselectAbobus (HexCell applied_to, Abobus abobus)
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
        return "Unselect " + abobus.abobus_name + " abobus";
    }

    public void Invoke()
    {
        Debug.Log("Unselect <color=green>" + abobus.abobus_name + " </color> abobus");
        game_manager.selected_abobus = null;
        abobus.transform.position += new Vector3(0, -10, 0);
    }

    
}
