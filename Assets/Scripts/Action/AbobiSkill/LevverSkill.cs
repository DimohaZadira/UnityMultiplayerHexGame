using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Linq;
public class LevverSkill : IAction
{
    private readonly HexCell target_cell;
    private readonly Levver levver;
    private readonly GameManager game_manager;

    public LevverSkill(HexCell target_cell, Levver levver)
    {
        this.target_cell = target_cell;
        this.levver = levver;
        game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public HexCell AppliedTo 
    { 
        get => target_cell;
        set { } // Игнорируем изменения, так как поле read-only
    }

    public string DebugMessage()
    {
        return "Levver skill";
    }

    private static HighlightableCell HexCellToHighlightable(HexCell hex_cell)
    {
        return hex_cell.GetComponent<HighlightableCell>();
    }

    public void Invoke()
    {
        
        // Если это клик по зверонику противника
        if (target_cell.abobus != null && target_cell.abobus.team != levver.team)
        {
            // Сохраняем выбранную цель
            levver.SetSelectedTarget(target_cell.abobus);
            
            // Подсвечиваем возможные клетки для перемещения цели
            List<HexCell> destination_cells = levver.GetPushDestinationTurns(target_cell);
            if (destination_cells.Count == 0)
            {
                EndTurn end_turn = new EndTurn(target_cell);
                end_turn.Invoke();
                return;
            }

            // Подсвечиваем клетки желтым
            HighlightCells highlight_action = new HighlightCells(
                target_cell, 
                null, 
                Utils.Map(destination_cells, HexCellToHighlightable).ToList(), 
                HighlightableCell.State.highlighted_yellow
            );
            highlight_action.Invoke();

            // Добавляем действия для каждой возможной клетки
            foreach (HexCell cell in destination_cells)
            {
                cell.actions.AddLast(new SimpleUnhighlight(target_cell, null));
                cell.actions.AddLast(new HighlightCells(
                    target_cell, 
                    null, 
                    new List<HighlightableCell>() { target_cell.GetComponent<HighlightableCell>() }, 
                    HighlightableCell.State.highlighted_blue
                ));
                cell.actions.AddLast(new PerformSkill(cell, levver));
            }
        }
        // Если это клик по свободной клетке для перемещения цели
        else if (target_cell.state == HexCell.State.empty && levver.GetSelectedTarget() != null)
        {
            // Перемещаем выбранного звероника на эту клетку
            Abobus target_abobus = levver.GetSelectedTarget();
            target_abobus.MoveToHexCoordinates(target_cell.hex_coordinates);
            
            // Очищаем выбранную цель
            levver.ClearSelectedTarget();
            
            // Завершаем ход
            EndTurn end_turn = new EndTurn(target_cell);
            end_turn.Invoke();
        }
    }
}
