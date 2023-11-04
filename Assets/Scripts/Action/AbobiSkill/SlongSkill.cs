using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlongSkill : IAction
{
    private HexCell applied_to;
    private Abobus abobus;
    private GameManager game_manager;
    public SlongSkill (HexCell applied_to, Abobus abobus)
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
        return "SlongSkill";
    }

    public void Invoke()
    {
        // foreach (HexCoordinates coords in abobus.GetPossibleSkillTurns(applied_to)) {
        //     HexCell cell = game_manager.hex_grid.GetCellByHexCoordinates(coords);
        //     // cell.actions.AddLast(new );
        // }
    }
}
