using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnhighlightMovement : IAction
{
    private HexCell applied_to;
    private GameManager game_manager;
    public UnhighlightMovement (HexCell applied_to)
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
        return "Unhighlight movement";
    }

    public void Invoke()
    {
        Debug.Log("<color=red>Unhighlight</color> <color=green>movement</color>");
        Abobus abobus = applied_to.abobus;
        if (abobus) {
            foreach (HexCell cell in abobus.GetPossibleMovementTurns()) {
                cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.default_);
            }
            game_manager.EnableAbobi(abobus);
            applied_to.actions.AddLast(new HighlightMovement(applied_to));
        }
    }
}
