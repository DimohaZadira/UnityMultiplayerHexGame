using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;


public class LevverSkill : IAction
{
    private HexCell applied_to;
    private Abobus abobus;
    private GameManager game_manager;

    public LevverSkill(HexCell applied_to, Abobus abobus)
    {
        this.applied_to = applied_to;
        this.abobus = abobus;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public HexCell AppliedTo 
    { 
        get => applied_to; 
        set { applied_to = value; } 
    }

    public string DebugMessage()
    {
        return "Levver skill";
    }

    public void Invoke()
    {
        Debug.Log("Levver invokes skill");
    
        if (game_manager.selected_abobus == null) 
        {
            SelectAbobus select_abobus = new SelectAbobus(abobus.cell, abobus);
            select_abobus.Invoke();
        }

    // Получаем список возможных ходов 
        List<HexCell> skill_turns = abobus.GetPossibleSkillTurns(applied_to);

        // Подсвечивание и добавление действий для зеленых клеток
        foreach (HexCell cell in skill_turns) 
        {
            if (GetDistance(abobus.cell, cell) <= 1 && cell.state == HexCell.State.empty) 
            {
                cell.state = HexCell.State.empty; // Используем состояние "empty" как "highlighted_green"
                cell.actions.AddLast(new SimpleMovement(cell, abobus));
                cell.actions.AddLast(new SimpleUnhighlight(cell, skill_turns));
                cell.actions.AddLast(new UnselectAbobus(cell, abobus));
            }
            // Подсвечивание и добавление действий для жёлтых клеток (если рядом враг)
            else if (cell.abobus != null && cell.abobus != abobus) 
            {
                HexCell destinationCell = GetDestinationCell(cell, abobus);

                if (destinationCell != null && destinationCell.state == HexCell.State.empty) 
                {
                    destinationCell.state = HexCell.State.empty; // Используем состояние как "highlighted_yellow"
                    
                    //destinationCell.debug_str = "performing yellow abobus";
                    
                    destinationCell.actions.AddLast(new PerformSkill(destinationCell, cell.abobus));
                    destinationCell.actions.AddLast(new SimpleMovement(destinationCell, game_manager.GetAbobusByHexCoordinates(cell.hex_coordinates)));
                    destinationCell.actions.AddLast(new SimpleMovement(cell, abobus));
                    destinationCell.actions.AddLast(new SimpleUnhighlight(destinationCell, skill_turns));
                    destinationCell.actions.AddLast(new UnselectAbobus(destinationCell, abobus));
                }
            }
        }

    // Завершение хода для текущего абобуса, убираем подсветку со всех клеток

    abobus.cell.actions.AddLast(new SimpleUnhighlight(abobus.cell, skill_turns));
    abobus.cell.actions.AddLast(new ClearActions<IAction>(abobus.cell, skill_turns));
    abobus.cell.actions.AddLast(new ReturnHighlights(abobus.cell, abobus));
    abobus.cell.actions.AddLast(new EndTurn(abobus.cell));
}

    // Метод для получения следующей клетки, если по прямой находится другой звероник
    
    private HexCell GetDestinationCell(HexCell startCell, Abobus target_abobus)
    {
        int deltaX = startCell.hex_coordinates.X - abobus.cell.hex_coordinates.X;
        int deltaY = startCell.hex_coordinates.Y - abobus.cell.hex_coordinates.Y;
        
        Vector3 direction = new Vector3(deltaX, deltaY, 0);

        HexCoordinates destinationCoords = HexCoordinates.FromXY(
            startCell.hex_coordinates.X + (int)direction.x,
            startCell.hex_coordinates.Y + (int)direction.y
        );

        if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(destinationCoords))
        {
            HexCell destinationCell = game_manager.hex_grid.GetCellByHexCoordinates(destinationCoords);
            if (destinationCell.state == HexCell.State.empty)
            {
                return destinationCell;
            }
        }
        
        return null;
    }

// Метод для вычисления расстояния между двумя клетками
    private int GetDistance(HexCell a, HexCell b)
    {
        return Mathf.Max(
            Mathf.Abs(a.hex_coordinates.X - b.hex_coordinates.X),
            Mathf.Abs(a.hex_coordinates.Y - b.hex_coordinates.Y),
            Mathf.Abs(a.hex_coordinates.Z - b.hex_coordinates.Z)
        );
    }
}