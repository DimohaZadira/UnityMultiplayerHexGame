using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformSkill : IAction
{
    private HexCell applied_to;
    private Abobus abobus;
    private GameManager game_manager;
    public PerformSkill (HexCell applied_to, Abobus abobus)
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
        return abobus.abobus_name + "abobus perform skill";
    }

    public void Invoke()
    {
        throw new System.NotImplementedException();
    }
}
