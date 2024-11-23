/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolcherSkill : IAction
{
    private HexCell applied_to;
    private Volcher volcher;
    private GameManager game_manager;

    public VolcherSkill(HexCell applied_to, Volcher volcher)
    {
        this.applied_to = applied_to;
        this.volcher = volcher;
        this.game_manager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
    }

    public HexCell AppliedTo
    {
        get => applied_to;
        set => applied_to = value;
    }

    public string DebugMessage()
    {
        return "Volcher skill triggered";
    }

    public void Invoke()
{
    Debug.Log("Volcher invokes skill");

    // Получение клеток в радиусе 2
    List<HexCell> rangeTwoCells = volcher.GetPossibleTurns(volcher.cell, RangeTwoComponent.GetBasisTurns(), HexCell.State.empty);
    List<HexCell> highlightedCells = new List<HexCell>();
    HashSet<HexCell> visitedCells = new HashSet<HexCell>();

    // Подсвечиваем клетки в радиусе 2 (зеленым)
    foreach (HexCell cell in rangeTwoCells)
    {
        if (!cell.HasAbobus()) // Пропускаем клетки с другими зверониками
        {
            HighlightableCell highlightable = cell.GetComponent<HighlightableCell>();
            if (highlightable != null)
            {
                highlightable.SetState(HighlightableCell.State.highlighted_green);
                highlightedCells.Add(cell);
            }
        }
    }

    // Находим клетки перед другими зверониками (желтым)
    foreach (Vector3 turn in RangeTwoComponent.GetBasisTurns())
    {
        HexCoordinates candidate = volcher.cell.hex_coordinates + turn;
        if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate))
        {
            HexCell firstCell = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
            if (firstCell != null && firstCell.HasAbobus()) // Если есть звероник
            {
                HexCoordinates secondCandidate = volcher.cell.hex_coordinates + turn * 2;
                if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(secondCandidate))
                {
                    HexCell secondCell = game_manager.hex_grid.GetCellByHexCoordinates(secondCandidate);
                    if (secondCell != null)
                    {
                        HighlightableCell highlightable = secondCell.GetComponent<HighlightableCell>();
                        if (highlightable != null)
                        {
                            highlightable.SetState(HighlightableCell.State.highlighted_yellow);
                            highlightedCells.Add(secondCell);
                        }
                    }
                }
            }
        }
    }

    // Подсвечиваем союзных звероников рядом (желтым)
    foreach (HexCell neighbor in game_manager.hex_grid.neighbors(volcher.cell))
    {
        if (neighbor != null && neighbor.HasAbobus() && neighbor.GetAbobus().IsFriendly(volcher))
        {
            HighlightableCell highlightable = neighbor.GetComponent<HighlightableCell>();
            if (highlightable != null)
            {
                highlightable.SetState(HighlightableCell.State.highlighted_yellow);
                highlightedCells.Add(neighbor);
            }
        }
    }

    // Добавляем действия на подсвеченные клетки
    foreach (HexCell cell in highlightedCells)
    {
        HighlightableCell highlightable = cell.GetComponent<HighlightableCell>();
        if (highlightable != null)
        {
            if (highlightable.state == HighlightableCell.State.highlighted_green || highlightable.state == HighlightableCell.State.highlighted_yellow)
            {
                cell.actions.AddLast(new SimpleMovement(cell, volcher)); // Перемещение Волчера
            }

            if (cell.HasAbobus() && highlightable.state == HighlightableCell.State.highlighted_yellow)
            {
                cell.actions.AddLast(new GrantExtraTurn(cell.GetAbobus())); // Выдача хода союзному зверонику
            }
        }
    }

    // Если ходов больше нет, выводим "конец хода"
    if (highlightedCells.Count == 0)
    {
        Debug.Log("Конец хода");
    }
}
}
*/