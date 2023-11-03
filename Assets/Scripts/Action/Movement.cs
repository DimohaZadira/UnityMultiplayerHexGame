using System.Collections;
using System.Collections.Generic;
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
        return "Moving " + abobus.abobus_name + " to " + applied_to.hex_coordinates.ToString();
    }

    public void Invoke()
    {
        game_manager.hex_grid.GetCellByHexCoordinates(abobus.hex_coordinates).React();
        abobus.MoveToHexCoordinates(applied_to.hex_coordinates);
    }
}
