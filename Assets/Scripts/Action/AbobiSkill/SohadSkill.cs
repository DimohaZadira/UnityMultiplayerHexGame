using System.Collections;
using System.Collections.Generic;
using System.Security.Claims;
using UnityEngine;

public class SohadSkill : IAction
{
    private HexCell applied_to;
    private Abobus abobus;
    private GameManager game_manager;
    private HashSet<HexCell> visited_cells;
    public SohadSkill (HexCell applied_to, Abobus abobus)
    {
        
        this.applied_to = applied_to;
        this.abobus = abobus;
        visited_cells = new HashSet<HexCell>();
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

    public void Invoke()
    {
        // Логируем начало действия
        Debug.Log("Sohad invokes skill");

        // Получаем список смещений
        Vector3[] turns = RangeTwoComponent.GetBasisTurns();
        List<HexCell> skill_turns = new List<HexCell>();

        foreach (Vector3 turn in turns)
        {
            // Координаты промежуточной клетки (где должен быть персонаж)
            int intermediateX = applied_to.hex_coordinates.X + (int)turn.x;
            int intermediateZ = applied_to.hex_coordinates.Z + (int)turn.z;
            HexCoordinates intermediateCoordinates = HexCoordinates.FromXZ(intermediateX, intermediateZ);

            // Координаты целевой клетки (за персонажем)
            int targetX = intermediateX + (int)turn.x;
            int targetZ = intermediateZ + (int)turn.z;
            HexCoordinates targetCoordinates = HexCoordinates.FromXZ(targetX, targetZ);

            // Проверяем, чтобы обе клетки находились в пределах сетки
            if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(intermediateCoordinates) ||
                game_manager.hex_grid.CheckHexCoordsOutOfBounds(targetCoordinates))
            {
                continue; // Пропускаем, если клетки за пределами сетки
            }

            // Получаем промежуточную и целевую клетки
            HexCell intermediateCell = game_manager.hex_grid.GetCellByHexCoordinates(intermediateCoordinates);
            HexCell targetCell = game_manager.hex_grid.GetCellByHexCoordinates(targetCoordinates);

            // Убедимся, что в промежуточной клетке есть персонаж и целевая клетка пуста
            if (intermediateCell != null && intermediateCell.abobus != null && 
                targetCell != null && targetCell.abobus == null && 
                !visited_cells.Contains(targetCell))
            {
                Debug.Log($"Adding target cell: {targetCell.name}");
                skill_turns.Add(targetCell);
            }
        }

        // Обработка найденных клеток
        foreach (HexCell cell in skill_turns)
        {
            // Подсветка клетки
            HighlightableCell highlightable = cell.GetComponent<HighlightableCell>();
            if (highlightable != null)
            {
                Debug.Log($"Highlighting target cell: {cell.name}");
                highlightable.SetState(HighlightableCell.State.highlighted_yellow);
            }

            // Добавляем действие перемещения
            Debug.Log($"Adding move action for cell: {cell.name}");
            cell.actions.AddLast(new SimpleMovement(cell, abobus)); // Перемещение
        }

        // Добавляем текущую клетку в список посещённых
        visited_cells.Add(applied_to);

        // Убираем подсветку и очищаем действия после завершения
        foreach (HexCell cell in skill_turns)
        {
            Debug.Log($"Clearing actions for cell: {cell.name}");
            cell.actions.AddLast(new SimpleUnhighlight(cell, skill_turns)); // Снятие подсветки
            cell.actions.AddLast(new ClearActions<SohadSkill>(cell, skill_turns)); // Очистка действий
        }

        // Добавляем действия для текущей клетки
        if (abobus.cell != null)
        {
            Debug.Log($"Clearing actions for current cell: {abobus.cell.name}");
            abobus.cell.actions.AddLast(new SimpleUnhighlight(abobus.cell, skill_turns)); // Снятие подсветки
            abobus.cell.actions.AddLast(new ClearActions<IAction>(abobus.cell, skill_turns)); // Очистка действий
            abobus.cell.actions.AddLast(new ReturnHighlights(abobus.cell, abobus)); // Возвращение подсветки
        }
    }
}






    /*
    public void Invoke()
    {
        Debug.Log("Sohad invokes skill");

        // Получаем список смещений из RangeTwoComponent
        Vector3[] turns = RangeTwoComponent.GetBasisTurns(); 
        List<HexCell> skill_turns = new List<HexCell>(); // Используем координаты для вычисления доступных клеток

        foreach (Vector3 turn in turns)
        {
            // Преобразуем координаты смещения в HexCoordinates
            int targetX = applied_to.hex_coordinates.X + (int)turn.x;
            int targetZ = applied_to.hex_coordinates.Z + (int)turn.z;
            HexCoordinates targetCoordinates = HexCoordinates.FromXZ(targetX, targetZ);

            // Проверяем, находятся ли координаты в пределах сетки
            if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(targetCoordinates))
            {
                // Получаем клетку по HexCoordinates
                HexCell targetCell = game_manager.hex_grid.GetCellByHexCoordinates(targetCoordinates);

                // Проверяем, пуста ли клетка (нет абобуса) и не была ли она уже посещена
                if (targetCell != null && targetCell.abobus == null && !visited_cells.Contains(targetCell))
                {
                    skill_turns.Add(targetCell);
                }
            }
        }
/*
        foreach (HexCell cell in skill_turns)
        {
            // Подсвечиваем клетки
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);

            // Добавляем действия в клетку
            cell.actions.AddLast(new SimpleMovement(cell, abobus)); // Перемещение абобуса
        }
*/
        // Добавляем текущую клетку в список посещённых
//        visited_cells.Add(applied_to);
/*
        // Очищаем подсветку и действия после хода
        foreach (HexCell cell in skill_turns)
        {
            cell.actions.AddLast(new SimpleUnhighlight(cell, skill_turns));
            cell.actions.AddLast(new ClearActions<SohadSkill>(cell, skill_turns));
        }
*/



/*
    foreach (HexCell cell in skill_turns)
        {
            // Подсветка клеток
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);

            // Добавляем действия для клеток
            cell.actions.AddLast(new SimpleMovement(cell, game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates))); // Перемещение цели
            cell.actions.AddLast(new SimpleMovement(applied_to, abobus)); // Перемещение Sohad
            cell.actions.AddLast(new SimpleUnhighlight(applied_to, skill_turns)); // Снятие подсветки
            cell.actions.AddLast(new UnselectAbobus(cell, abobus)); // Снятие выделения
            cell.actions.AddLast(new EndTurn(cell)); // Завершение хода
        }

        // Добавляем действия для клетки, где находится Sohad
        abobus.cell.actions.AddLast(new SimpleUnhighlight(abobus.cell, skill_turns)); // Снятие подсветки
        abobus.cell.actions.AddLast(new ClearActions<IAction>(abobus.cell, skill_turns)); // Очистка действий
        abobus.cell.actions.AddLast(new ReturnHighlights(abobus.cell, abobus)); // Возвращение подсветки
    }
    }
*/




/*
    public void Invoke()
    {
        Debug.Log("Sohad invokes skill");

        // Убедимся, что выбран правильный персонаж
        if (game_manager.selected_abobus == null)
        {
            SelectAbobus select_abobus = new SelectAbobus(abobus.cell, abobus);
            select_abobus.Invoke();
        }

        // Получаем доступные клетки для применения навыка
        List<HexCell> skill_turns = abobus.GetPossibleSkillTurns(applied_to);

        foreach (HexCell cell in skill_turns)
        {
            // Подсветка клеток
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);

            // Добавляем действия для клеток
            cell.actions.AddLast(new SimpleMovement(cell, game_manager.GetAbobusByHexCoordinates(applied_to.hex_coordinates))); // Перемещение цели
            cell.actions.AddLast(new SimpleMovement(applied_to, abobus)); // Перемещение Sohad
            cell.actions.AddLast(new SimpleUnhighlight(applied_to, skill_turns)); // Снятие подсветки
            cell.actions.AddLast(new UnselectAbobus(cell, abobus)); // Снятие выделения
            cell.actions.AddLast(new EndTurn(cell)); // Завершение хода
        }

        // Добавляем действия для клетки, где находится Sohad
        abobus.cell.actions.AddLast(new SimpleUnhighlight(abobus.cell, skill_turns)); // Снятие подсветки
        abobus.cell.actions.AddLast(new ClearActions<IAction>(abobus.cell, skill_turns)); // Очистка действий
        abobus.cell.actions.AddLast(new ReturnHighlights(abobus.cell, abobus)); // Возвращение подсветки
    }
*/
