using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class SohadSkill : IAction
{
    private HexCell applied_to;
    private Abobus abobus;
    private GameManager game_manager;
<<<<<<< HEAD
    
=======
    private List<HexCell> skill_turns = new List<HexCell>(); // Список клеток для возможных прыжков

>>>>>>> 4008afa9555443b9ef3c3becf0e47858cc4f5a46
    public SohadSkill (HexCell applied_to, Abobus abobus)
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
        return "Sohad skill";
    }
/*
    public void Invoke()
    {
        Debug.Log("Sohad invokes skill");
        List<HexCell> skill_turns = abobus.GetPossibleSkillTurns(applied_to);
        foreach (HexCell cell in skill_turns) {
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
            cell.actions.AddLast(new SimpleMovement(cell, game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates)));
            cell.actions.AddLast(new SimpleMovement(applied_to, abobus));
            cell.actions.AddLast(new SimpleUnhighlight(applied_to, skill_turns));
            cell.actions.AddLast(new ClearActions<SohadSkill>(applied_to, skill_turns));
        }
    }
*/

    public void Invoke()
    {
        Debug.Log("Sohad skill invoked");
        
        // Подсветка клеток в радиусе 2
        HighlightSkillArea();

        // Для каждой клетки в радиусе возможных прыжков добавляем действие
        foreach (HexCell cell in skill_turns)
        {
            // Подсветка клеток, через которые возможно совершить прыжок
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);

            // Действия на перемещение
            cell.actions.AddLast(new SimpleMovement(cell, game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates)));
            cell.actions.AddLast(new SimpleMovement(applied_to, abobus));

            // Действие на отмену подсветки
            cell.actions.AddLast(new SimpleUnhighlight(applied_to, skill_turns));

            // Очистка списка действий 
            cell.actions.AddLast(new ClearActions<SohadSkill>(applied_to, skill_turns));
        }
    }

    private void HighlightSkillArea()
    {
        List<HexCell> cellsInRange = GetCellsInRange(applied_to, 2); // Получаем клетки в радиусе 2
        skill_turns.Clear(); // Очистка предыдущих возможных клеток

        foreach (var cell in cellsInRange)
        {
            if (cell.state == HexCell.State.empty || cell.abobus == null) // Подсвечиваем пустые клетки или клетки без звероников
            {
                cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_green);
                skill_turns.Add(cell);
            }
        }

        // Теперь проверяем возможность прыжков через звероников
        foreach (var cell in cellsInRange)
        {
            if (cell.abobus != null && cell.abobus != abobus) // Если это другой звероник
            {
                Vector3 direction = new Vector3(
                    cell.hex_coordinates.X - abobus.cell.hex_coordinates.X, 
                    cell.hex_coordinates.Y - abobus.cell.hex_coordinates.Y, 0).normalized;
                HexCoordinates nextHexCoords = HexCoordinates.FromXY(cell.hex_coordinates.X + (int)direction.x, cell.hex_coordinates.Y + (int)direction.y);
                if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(nextHexCoords))
                {
                    HexCell nextCell = game_manager.hex_grid.GetCellByHexCoordinates(nextHexCoords);
                    if (nextCell.state == HexCell.State.empty) // Если следующая клетка свободна
                    {
                        nextCell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);
                        skill_turns.Add(nextCell); // Добавляем её в возможные клетки для прыжков
                    }
                }
            }
        }
    }

    private List<HexCell> GetCellsInRange(HexCell startCell, int range)
    {
        List<HexCell> cellsInRange = new List<HexCell>();
        Vector3[] directions = RangeOneComponent.GetBasisTurns(); // Получаем направления для проверки

        // Ищем клетки в пределах радиуса range
        for (int dx = -range; dx <= range; dx++)
        {
            for (int dy = -range; dy <= range; dy++)
            {
                if (Mathf.Abs(dx + dy) <= range)
                {
                    HexCoordinates targetCoord = HexCoordinates.FromXY(startCell.hex_coordinates.X + dx, startCell.hex_coordinates.Y + dy);
                    if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(targetCoord))
                    {
                        cellsInRange.Add(game_manager.hex_grid.GetCellByHexCoordinates(targetCoord));
                    }
                }
            }
        }

        return cellsInRange;
    }

    public void OnCellClicked(HexCell targetCell)
    {
        if (skill_turns.Contains(targetCell))
        {
            MoveToCell(targetCell);
        }
    }

    private void MoveToCell(HexCell targetCell)
    {
        abobus.cell = targetCell; // Перемещаем Сохада на выбранную клетку
        if (CanJump())
        {
            HighlightSkillArea(); // Подсвечиваем возможные клетки для следующего прыжка
        }
        else
        {
            EndTurn(); // Завершаем ход
        }
    }

    private bool CanJump()
    {
        List<HexCell> cellsInRange = GetCellsInRange(abobus.cell, 2); // Получаем клетки в радиусе 2
        foreach (var cell in cellsInRange)
        {
            if (cell.abobus != null && cell.abobus != abobus) // Если есть соседний звероник
            {
                Vector3 direction = new Vector3(
                    cell.hex_coordinates.X - abobus.cell.hex_coordinates.X, 
                    cell.hex_coordinates.Y - abobus.cell.hex_coordinates.Y, 0).normalized;
                HexCoordinates nextHexCoords = HexCoordinates.FromXY(cell.hex_coordinates.X + (int)direction.x, cell.hex_coordinates.Y + (int)direction.y);
                if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(nextHexCoords))
                {
                    HexCell nextCell = game_manager.hex_grid.GetCellByHexCoordinates(nextHexCoords);
                    if (nextCell.state == HexCell.State.empty) // Если можно прыгнуть дальше
                    {
                        return true;
                    }
                }
            }
        }
        return false; // Прыжков больше нет
    }

    private void EndTurn()
    {
        Debug.Log("End of turn for Sohad");
        game_manager.EndTurn(); // Завершаем ход
    }
}


