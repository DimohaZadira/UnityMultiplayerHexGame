using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnhighlightSkillTrigger : IAction
{
    private List<HexCell> to_unhighligt;
    public UnhighlightSkillTrigger (HexCell applied_to, List<HexCell> to_unhighligt)
    {
        this.applied_to = applied_to;
        this.to_unhighligt = to_unhighligt;
    }
    private HexCell applied_to;
    private GameManager game_manager;
    
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
        foreach (HexCell cell in to_unhighligt) {
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.default_);
        }
        applied_to.actions.AddLast(new HighlightSkillTrigger(applied_to));
        
    }
}
