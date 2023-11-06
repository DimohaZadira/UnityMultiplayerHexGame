using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class SohadSkill : IAction
{
    private HexCell applied_to;
    private Abobus abobus;
    private GameManager game_manager;
    public SohadSkill (HexCell applied_to, Abobus abobus)
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
        return "Slong skill";
    }

    public void Invoke()
    {
        Debug.Log("Slong invokes skill");
        List<HexCell> skill_turns = abobus.GetPossibleSkillTurns(applied_to);
        foreach (HexCell cell in skill_turns) {
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
            cell.actions.AddLast(new SimpleMovement(cell, game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates)));
            cell.actions.AddLast(new SimpleMovement(applied_to, abobus));
            cell.actions.AddLast(new SimpleUnhighlight(applied_to, skill_turns));
            cell.actions.AddLast(new ClearActions<SohadSkill>(applied_to, skill_turns));
        }
    }
}
