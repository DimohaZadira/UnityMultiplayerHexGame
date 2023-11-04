using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnhighlightSkillTrigger : IAction
{
    private HexCell applied_to;
    private GameManager game_manager;
    public UnhighlightSkillTrigger (HexCell applied_to)
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
        return "Unhighlight skilltrigger";
    }

    public void Invoke()
    {
        Debug.Log("<color=red>Unhighlight</color> <color=yellow>skill trigger</color>");
        Abobus abobus = applied_to.abobus;
        if (abobus) {
            foreach (HexCell cell in abobus.GetPossibleSkillTriggerTurns()) {
                // cell.DeleteFromActions<PerformSkill>();
                cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.default_);
            }
            applied_to.actions.AddLast(new HighlightSkillTrigger(applied_to));
        }
    }
}
