using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnhighlightMovement : IAction
{
    private List<HexCell> to_unhighligt;
    public UnhighlightMovement (HexCell applied_to, List<HexCell> to_unhighligt)
    {
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
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
        return "Unhighlight movement";
    }

    public void Invoke()
    {
        Debug.Log("<color=red>Unhighlight</color> <color=green>movement</color>");
        foreach (HexCell cell in to_unhighligt) {
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.default_);
        }
        applied_to.actions.AddLast(new HighlightMovement(applied_to));
        
    }
}
