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
        return "Slong skill";
    }


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

        foreach (HexCell cell in skill_turns)
        {
            // Подсвечиваем клетки
            cell.GetComponent<HighlightableCell>().SetState(HighlightableCell.State.highlighted_yellow);

            // Добавляем действия в клетку
            cell.actions.AddLast(new SimpleMovement(cell, abobus)); // Перемещение абобуса
        }

        // Добавляем текущую клетку в список посещённых
        visited_cells.Add(applied_to);

        // Очищаем подсветку и действия после хода
        foreach (HexCell cell in skill_turns)
        {
            cell.actions.AddLast(new SimpleUnhighlight(cell, skill_turns));
            cell.actions.AddLast(new ClearActions<SohadSkill>(cell, skill_turns));
        }
    }
}