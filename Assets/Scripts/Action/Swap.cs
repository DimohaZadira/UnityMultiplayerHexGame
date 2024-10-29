using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap : IAction
{
    private Abobus abobus1; // звероник которого выбираем
    private Abobus abobus2; // звероник на которого тыкаем
    private HexCell cell1; //хекс 1-ого звероника
    private HexCell cell2; //хекс 2-ого звероника
    private GameManager game_manager;


    public Swap(Abobus abobus1, Abobus abobus2)
    {
        this.abobus1 = abobus1;
        this.abobus2 = abobus2;

        this.cell1 = abobus1.cell;
        this.cell2 = abobus2.cell;

        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        
    }
    public HexCell AppliedTo
    {
        get => cell1;
        set => cell1 = value;
    }

    // public HexCell AppliedFrom
    // {
    //     get => cell2;
    //     set => cell2 = value;
    // }


    public string DebugMessage()
    {
        return "Swapping " + abobus1.abobus_name + " with " + abobus2.abobus_name;
    }

    public void Invoke()
    {
        Debug.Log("Action SWAP");

        SwapPositions();
    }

    private void SwapPositions()
    {
        abobus1.MoveToHexCoordinates(cell2.hex_coordinates);
        abobus2.MoveToHexCoordinates(cell1.hex_coordinates);
    }
}