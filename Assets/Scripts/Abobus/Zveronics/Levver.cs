using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*ВЫБРАН ЛЕВВЕР: зеленым подсвечиваются все гексы в радиусе 1 за исключением тех, на которых стоят другие звероники. 
Если по прямой от ЛЕВЕВРА находится другой звероник, то следующий по направлению от ЛЕВВЕРА к этому зверонику гекс 
подсвечивается желтым. При клике на подсвеченный гекс ЛЕВВЕР переместится туда.
Если ЛЕВВЕР граничит со звероником противника, то гекс такого соседа подсвечивается желтым. 
При клике на подсвеченный гекс желтым подсвечиваются все свободные гексы в радиусе 2 от кликнутого. 
При клике на один из них выбранный противник переместится на последний кликнутый гекс. 
САМ ЛЕВВЕР ПРИ ЭТОМ ОСТАЕТСЯ НА МЕСТЕ. Применение обеих способностей за ход разрешено. 
Когда продолжение хода будет невозможно, на экран выводится символ «конец хода».*/
public class Levver : Abobus
{
    public List<HexCoordinates> visited;
    private Abobus selected_target;

    public Levver()
    {
        visited = new List<HexCoordinates>();
        selected_target = null;
    }

    public override Type GetSkillType()
    {
        return typeof(LevverSkill);
    }

    override public List<HexCell> GetPossibleMovementTurns()
    {
        List<HexCell> ans = new List<HexCell>();
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        
        // 1. Проверяем все клетки в радиусе 1
        foreach (Vector3 turn in basis_turns)
        {
            HexCoordinates candidate = HexCoordinates.FromXY(
                cell.hex_coordinates.X + (int)turn[0],
                cell.hex_coordinates.Y + (int)turn[1]
            );

            if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate))
            {
                continue;
            }

            HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
            if (cell_candidate.state == HexCell.State.empty)
            {
                ans.Add(cell_candidate);
            }
        }
        
        // 2. Проверяем все клетки по прямой за каждым абобусом
        foreach (Vector3 turn in basis_turns)
        {
            HexCoordinates current = cell.hex_coordinates;
            
            // Идем по прямой, пока не найдем абобуса или не выйдем за пределы поля
            while (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(current))
            {
                current = HexCoordinates.FromXY(
                    current.X + (int)turn[0],
                    current.Y + (int)turn[1]
                );

                HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(current);
                
                if (cell_candidate.state == HexCell.State.abobus)
                {
                    // Проверяем клетку за абобусом
                    HexCoordinates behind = HexCoordinates.FromXY(
                        current.X + (int)turn[0],
                        current.Y + (int)turn[1]
                    );
                    
                    if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(behind))
                    {
                        HexCell behind_cell = game_manager.hex_grid.GetCellByHexCoordinates(behind);
                        if (behind_cell.state == HexCell.State.empty)
                        {
                            ans.Add(behind_cell);
                        }
                    }
                    break;
                }
            }
        }
        return ans;
    }

    public List<HexCell> GetPushTargetTurns(HexCell hc)
    {
        List<HexCell> ans = new List<HexCell>();
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        
        foreach (Vector3 turn in basis_turns)
        {
            HexCoordinates candidate = HexCoordinates.FromXY(
                hc.hex_coordinates.X + (int)turn[0],
                hc.hex_coordinates.Y + (int)turn[1]
            );

            if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate))
            {
                continue;
            }

            HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
            if (cell_candidate.state == HexCell.State.abobus && 
                cell_candidate.abobus.team != this.team)
            {
                ans.Add(cell_candidate);
            }
        }
        return ans;
    }

    public List<HexCell> GetPushDestinationTurns(HexCell target_cell)
    {
        List<HexCell> ans = new List<HexCell>();
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        
        // Проверяем клетки в радиусе 2 от цели
        for (int range = 1; range <= 2; range++)
        {
            foreach (Vector3 turn in basis_turns)
            {
                HexCoordinates candidate = HexCoordinates.FromXY(
                    target_cell.hex_coordinates.X + (int)turn[0] * range,
                    target_cell.hex_coordinates.Y + (int)turn[1] * range
                );

                if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate))
                {
                    continue;
                }

                HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if (cell_candidate.state == HexCell.State.empty)
                {
                    ans.Add(cell_candidate);
                }
            }
        }
        return ans;
    }

    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        List<HexCell> ans = new List<HexCell>();
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        
        // Проверяем соседние клетки на наличие звероников противника
        foreach (Vector3 turn in basis_turns)
        {
            HexCoordinates candidate = HexCoordinates.FromXY(
                cell.hex_coordinates.X + (int)turn[0],
                cell.hex_coordinates.Y + (int)turn[1]
            );

            if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate))
            {
                continue;
            }

            HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
            if (cell_candidate.state == HexCell.State.abobus && 
                cell_candidate.abobus.team != this.team)
            {
                ans.Add(cell_candidate);
            }
        }
        return ans;
    }

    public void SetSelectedTarget(Abobus target)
    {
        selected_target = target;
    }

    public Abobus GetSelectedTarget()
    {
        return selected_target;
    }

    public void ClearSelectedTarget()
    {
        selected_target = null;
    }
}
