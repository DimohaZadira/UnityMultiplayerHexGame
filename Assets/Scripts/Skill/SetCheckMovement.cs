using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCheckMovement : IAction
{
    HexCell applied_to;
    GameManager game_manager;
    public SetCheckMovement (HexCell applied_to)
    {
        this.applied_to = applied_to;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public HexCell AppliedTo {
        get {
            return applied_to;
        }
        set {
            applied_to = value;
        }
    }

    public string DebugMessage()
    {
        return "SetCheckAction";
    }

    public void Invoke()
    {
        applied_to.actions.Add(new CheckMovement(applied_to));
    }
}
