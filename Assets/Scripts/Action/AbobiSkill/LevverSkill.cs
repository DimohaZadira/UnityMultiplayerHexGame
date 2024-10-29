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
        set 
        {
            applied_to = value;
        } 
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

        // Получаем клетки для подсветки: зеленые клетки в радиусе 1 и желтые для пути через врага
        List<HexCell> greenCells = abobus.GetGreenCellsInRadius1(applied_to);
        List<HexCell> yellowCells = abobus.GetYellowCellsThroughEnemy(applied_to);

        // Обработка зеленых клеток: подсветка и добавление действий
        foreach (HexCell cell in greenCells) 
        {
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_green);
            cell.actions.AddLast(new SimpleMovement(cell, abobus));
            cell.actions.AddLast(new SimpleUnhighlight(applied_to, greenCells));
            cell.actions.AddLast(new UnselectAbobus(cell, abobus));}

        // Обработка желтых клеток: подсветка и добавление действий для перемещения врага
        foreach (HexCell cell in greenCells) 
        {
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_green);
            cell.actions.AddLast(new SimpleMovement(cell, abobus));
            cell.actions.AddLast(new SimpleUnhighlight(applied_to, greenCells));
            cell.actions.AddLast(new UnselectAbobus(cell, abobus));}

        // Завершение хода для текущего Abobus
        abobus.cell.actions.AddLast(new SimpleUnhighlight(abobus.cell, greenCells));
        abobus.cell.actions.AddLast(new ClearActions<IAction>(abobus.cell, greenCells));
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
}