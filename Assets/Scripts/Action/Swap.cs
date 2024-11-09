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
    public Swap(HexCell applied_to, Abobus abobus2)
    {
        this.abobus2 = abobus2;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        this.applied_to = applied_to;

        this.abobus1 = game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates);
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
        var temp_coordinates = abobus1.cell.hex_coordinates;
        abobus1.MoveToHexCoordinates(abobus2.cell.hex_coordinates);
        abobus2.MoveToHexCoordinates(temp_coordinates);
    }
}