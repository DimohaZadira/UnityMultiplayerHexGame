using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;

public class PerformSkill : IAction
{
    private HexCell applied_to;
    private Abobus abobus;
    private GameManager game_manager;
    public PerformSkill (HexCell applied_to, Abobus abobus)
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
        return "Performing " + abobus.abobus_name + " abobus skill";
    }

    public void Invoke()
    {
        HexCell from = abobus.cell;
        from.React();
        from.actions.Clear();
        var disable_ = new DisableAbobi(applied_to, abobus);
        disable_.Invoke();
        IAction abobus_skill = (IAction)Activator.CreateInstance(abobus.action_type, applied_to, abobus);
        abobus_skill.Invoke();
    }
}