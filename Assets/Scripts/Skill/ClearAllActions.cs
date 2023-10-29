using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearAllActions : IAction
{
    HexCell applied_to;
    GameManager game_manager;
    public ClearAllActions (HexCell applied_to)
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
        return "ClearAllActions";
    }

    public void Invoke()
    {
        game_manager.ClearAllActions();
        
    }
}
