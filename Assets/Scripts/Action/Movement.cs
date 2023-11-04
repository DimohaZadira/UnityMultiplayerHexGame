using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Movement : IAction
{
    private HexCell applied_to;
    private Abobus abobus;
    private GameManager game_manager;
    public Movement (HexCell applied_to, Abobus abobus)
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
        return "Moving " + abobus.abobus_name + " abobus to " + applied_to.hex_coordinates.ToString();
    }

    public void Invoke()
    {
        HexCell from = abobus.cell;
        if (typeof(UnhighlightMovement) != from.actions.PeekFirst().GetType()) {
            throw new Exception();
        }
        from.React();
        from.actions.Clear();
        abobus.MoveToHexCoordinates(applied_to.hex_coordinates);

        game_manager.DisableAbobi(abobus);

        if (applied_to.actions.size > 0) {
            throw new Exception();
        }
        applied_to.actions.AddLast(new HighlightSkillTrigger(applied_to));
        applied_to.React();
    }
}
