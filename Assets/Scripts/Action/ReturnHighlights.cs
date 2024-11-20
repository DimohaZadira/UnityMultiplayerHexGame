using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ReturnHighlights : IAction
{
    private HexCell applied_to;
    private GameManager game_manager;
    private Abobus abobus;
    public ReturnHighlights (HexCell applied_to, Abobus abobus)
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
        return "Unselect return highlights " + abobus.abobus_name + " abobus";
    }

    public void Invoke()
    {
        Debug.Log("Unselect return highlights <color=green>" + abobus.abobus_name + " </color> abobus");
        
        if (!game_manager.moved_this_turn) {
            abobus.cell.actions.AddLast(new HighlightMovement(abobus.cell));
        }
        abobus.cell.actions.AddLast(new HighlightSkillTrigger(abobus.cell));
        abobus.cell.actions.AddLast(new SelectAbobus(abobus.cell, abobus));

    }

    
}
