using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap : IAction
{
    private Abobus abobus1; // звероник которого выбираем
    private Abobus abobus2;
    private HexCell applied_to; //хекс 1-ого звероника
    private GameManager game_manager;


    public Swap(HexCell applied_to, Abobus abobus1, Abobus abobus2)
    {
        this.abobus1 = abobus1;
        this.abobus2 = abobus2;

        this.applied_to = applied_to;

        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        
    }
    public HexCell AppliedTo
    {
        get => applied_to;
        set => applied_to = value;
    }

    public string DebugMessage()
    {
        return "Swaping " + abobus1.abobus_name + " with " + abobus2.abobus_name + " between " + abobus1.cell.hex_coordinates.ToString() + abobus2.cell.hex_coordinates.ToString();
    }


    public void Invoke()
    {
        Debug.Log("Action SWAP");

        SwapPositions();
    }

    private void SwapPositions()
    {
        var tempCoordinates = abobus1.cell.hex_coordinates;
        abobus1.MoveToHexCoordinates(abobus2.cell.hex_coordinates);
        abobus2.MoveToHexCoordinates(tempCoordinates);
    }
}