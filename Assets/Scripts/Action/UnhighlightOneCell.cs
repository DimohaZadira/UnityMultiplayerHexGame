using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnhighlightOneCell : IAction
{
    private List<HexCell> to_unhighligt;
    public UnhighlightOneCell (HexCell applied_to)
    {
        this.applied_to = applied_to;
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
        return "Unhighlight one cell  " + to_unhighligt;
    }

    public void Invoke()
    {
        Debug.Log("Unhighlighting one cell");
        applied_to.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.default_);
    }
}

