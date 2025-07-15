using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
/*Выбран ПРИМАР: зеленым подсвечиваются все гексы в радиусе 1 за исключением тех, на которых стоят другие звероники. 
Если ПРИМАР граничит с любым другим звероником, то гекс соседа подсвечивается желтым. 
После клика на этот гекс желтым подсвечиваются все граничащие с выбранным гексы, на которых стоят другие звероники. 
Зеленым подсвечиваются все остальные гексы в радиусе 1 от последнего выбранного. 
Таким образом, для применения способности нужно по очереди кликать на цепочку звероников, стоящих рядом, 
и «спрыгнуть» на свободный граничащий гекс в любую сторону с каждого из них. 
После клика на подсвеченную зеленым клетку ход заканчивается.*/
public class Primar : Abobus
{
    public override Type GetSkillType()
    {
        return typeof(PrimarSkill);
    }
    public List<HexCoordinates> visited;
    public Primar()
    {
        visited = new List<HexCoordinates>();
    }

    override public List<HexCell> GetPossibleMovementTurns()
    {
        return GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        return GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus);
    }

    public List<HexCell> GetPossibleSkillTurns(HexCell from)
    {
        List<HexCell> ans = new List<HexCell>();
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        
        foreach (Vector3 turn in basis_turns)
        {
            HexCoordinates candidate = HexCoordinates.FromXY(
                from.hex_coordinates.X + (int)turn[0],
                from.hex_coordinates.Y + (int)turn[1]
            );

            if (game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate))
            {
                continue;
            }

            HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
            if (cell_candidate.state == HexCell.State.abobus && !visited.Contains(cell_candidate.hex_coordinates))
            {
                ans.Add(cell_candidate);
            }
        }
        return ans;
    }
}
