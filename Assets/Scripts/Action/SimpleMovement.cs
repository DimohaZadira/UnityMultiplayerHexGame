using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class SimpleMovement : IAction
{
    private HexCell applied_to;
    private Abobus abobus;
    private GameManager game_manager;
    public SimpleMovement (HexCell applied_to, Abobus abobus)
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
        return "Simply moving " + abobus.abobus_name + " abobus to " + applied_to.hex_coordinates.ToString();
    }

    public void Invoke()
    {
        Debug.Log("Simply move");
        abobus.MoveToHexCoordinates(applied_to.hex_coordinates);
    }
}
