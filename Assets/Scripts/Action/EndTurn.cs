using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class EndTurn : IAction
{
    private HexCell applied_to;
    private GameManager game_manager;
    public EndTurn (HexCell applied_to)
    {
        this.applied_to = applied_to;
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
        return "End turn";
    }

    public void Invoke()
    {
        Debug.Log("End turn");
        game_manager.SwitchTurn();        
    }

    
}
