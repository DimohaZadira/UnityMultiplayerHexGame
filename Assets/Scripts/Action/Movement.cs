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
        if (!game_manager.moved_this_turn) {
            HexCell from = abobus.cell;
            from.React();
            from.actions.Clear();
            abobus.MoveToHexCoordinates(applied_to.hex_coordinates);
            game_manager.moved_this_turn = true;

            var highlight_ = new HighlightSkillTrigger(applied_to);
            highlight_.Invoke();
            var select_ = new SelectAbobus(applied_to, abobus);
            select_.Invoke();
        }
    }
}
