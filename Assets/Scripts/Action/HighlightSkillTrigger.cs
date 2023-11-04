using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightSkillTrigger : IAction
{
    private HexCell applied_to;
    private GameManager game_manager;
    public HighlightSkillTrigger (HexCell applied_to)
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
        return "Highlight skill trigger";
    }
    public void Invoke()
    {
        Debug.Log("Highlight <color=yellow>skill trigger</color>");
        Abobus abobus = applied_to.abobus;
        if (abobus) {
            foreach (HexCell cell in abobus.GetPossibleSkillTriggerTurns()) {
                cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
                cell.actions.AddLast(new PerformSkill(cell, abobus));
            }
            applied_to.actions.AddLast(new UnhighlightSkillTrigger(applied_to));
        }
    }
}
