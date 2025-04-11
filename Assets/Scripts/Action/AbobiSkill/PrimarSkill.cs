using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.Dependencies.Sqlite;
using JetBrains.Annotations;
using System;

public class PrimarSkill : IAction {
    private readonly HexCell target_cell;
    private readonly Primar primar;
    private readonly GameManager game_manager;
    private HexCoordinates initial_position;
    private HexCell start_cell;

    public PrimarSkill(HexCell target_cell, Primar primar)
    {
        this.target_cell = target_cell;
        this.primar = primar;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        initial_position = primar.cell.hex_coordinates;
        start_cell = game_manager.hex_grid.GetCellByHexCoordinates(initial_position);
    }

    public HexCell AppliedTo 
    { 
        get => target_cell;
        set { } // Игнорируем изменения, так как поле read-only
    }

    public string DebugMessage()
    {
        return "Primar skill";
    }

    private static HighlightableCell HexCellToHighlightable(HexCell hex_cell)
    {
        return hex_cell.GetComponent<HighlightableCell>();
    }

    public void Invoke()
    {
        Debug.Log("Primar invokes skill");
        
        // Если это клик по самому Примару
        if (target_cell.abobus == primar)
        {
            // Возвращаем модель Примара в исходное положение
            primar.transform.position = target_cell.transform.position;
            
            // Очищаем список посещенных клеток
            primar.visited.Clear();
         
        }
        
        // Если это клик по зверонику в радиусе 1
        if (target_cell.abobus != null && !primar.visited.Contains(target_cell.hex_coordinates))
        {
            // Очищаем все предыдущие подсветки и действия
            game_manager.ClearAllHighlightedCells();
            game_manager.ClearAllActions();
            
            // Добавляем клетку в посещенные
            primar.visited.Add(target_cell.hex_coordinates);
            // Перемещаем модель Примара над выбранным звероником
            primar.transform.position = target_cell.transform.position + new Vector3(0, 15, 0);
            
            // Подсвечиваем выбранного звероника синим
            target_cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_blue);
            
            // Подсвечиваем свободные клетки зеленым
            List<HexCell> empty_cells = primar.GetPossibleTurns(target_cell, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
        
            if (!empty_cells.Contains(start_cell))
            {
                empty_cells.Add(start_cell);
            }
        
            // Подсвечиваем звероников желтым (исключая посещенные)
            List<HexCell> abobi_cells = primar.GetPossibleTurns(target_cell, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus)
                .Where(cell => !primar.visited.Contains(cell.hex_coordinates) && !cell.hex_coordinates.Equals(initial_position))
                .ToList();
                
            if (empty_cells.Count == 0 && abobi_cells.Count == 0)
            {
                primar.MoveToHexCoordinates(initial_position);
                EndTurn end_turn = new EndTurn(target_cell);
                end_turn.Invoke();
                return;
            }
            // Подсвечиваем свободные клетки зеленым
            if (empty_cells.Count > 0)
            {
                HighlightCells highlight_empty = new HighlightCells(
                    target_cell,
                    null,
                    Utils.Map(empty_cells, HexCellToHighlightable).ToList(),
                    HighlightableCell.State.highlighted_green
                );
                highlight_empty.Invoke();
            }

            // Подсвечиваем звероников желтым
            if (abobi_cells.Count > 0)
            {
                HighlightCells highlight_abobi = new HighlightCells(
                    target_cell,
                    null,
                    Utils.Map(abobi_cells, HexCellToHighlightable).ToList(),
                    HighlightableCell.State.highlighted_yellow
                );
                highlight_abobi.Invoke();
            }

            // Добавляем действия для каждой клетки
            foreach (HexCell cell in empty_cells)
            {
                cell.actions.AddLast(new SimpleMovement(cell, primar));
                cell.actions.AddLast(new EndSkill(cell, primar));
                cell.actions.AddLast(new EndTurn(cell));
            }

            foreach (HexCell cell in abobi_cells)
            {
                cell.actions.AddLast(new SimpleUnhighlight(target_cell, null));
                cell.actions.AddLast(new HighlightCells(
                    target_cell,
                    null,
                    new List<HighlightableCell>() { target_cell.GetComponent<HighlightableCell>() },
                    HighlightableCell.State.highlighted_blue
                ));
                cell.actions.AddLast(new PerformSkill(cell, primar));
            }
        }
        
    }

    private class EndSkill : IAction
    {
        private readonly HexCell target_cell;
        private readonly Primar primar;

        public EndSkill(HexCell target_cell, Primar primar)
        {
            this.target_cell = target_cell;
            this.primar = primar;
        }

        public HexCell AppliedTo
        {
            get => target_cell;
            set { }
        }

        public string DebugMessage()
        {
            return "Set Primar position";
        }

        public void Invoke()
        {
            primar.visited.Clear();
            primar.transform.position = target_cell.transform.position;
        }
    }
}