using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSkill : IAction
{
    GameManager game_manager;
    Abobus abobus;
    private HexCell applied_to;
    public HexCell AppliedTo {
        get {
            return applied_to;
        }
        set {
            applied_to = value;
        }
    }
    public CheckSkill (HexCell applied_to) 
    {
        this.applied_to = applied_to;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        abobus = game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates);
        
    }

    public void Invoke()
    {
        if (abobus) {
            
        }
        
    }

    public string DebugMessage()
    {
        return "Checking " + abobus.abobus_name + " abobus's skill";
    }
}
