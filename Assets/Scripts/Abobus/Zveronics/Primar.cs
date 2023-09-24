using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*Выбран ПРИМАР: зеленым подсвечиваются все гексы в радиусе 1 за исключением тех, на которых стоят другие звероники. 
Если ПРИМАР граничит с любым другим звероником, то гекс соседа подсвечивается желтым. 
После клика на этот гекс желтым подсвечиваются все граничащие с выбранным гексы, на которых стоят другие звероники. 
Зеленым подсвечиваются все остальные гексы в радиусе 1 от последнего выбранного. 
Таким образом, для применения способности нужно по очереди кликать на цепочку звероников, стоящих рядом, 
и «спрыгнуть» на свободный граничащий гекс в любую сторону с каждого из них. 
После клика на подсвеченную зеленым клетку ход заканчивается.*/
public class Primar : Abobus
{
    override public bool PerformSkill(HexCell from, HexCell to)
    {
        if (gay_manager.GetAbobusByHexCoordinates(to.hex_coordinates)) {
            MoveToHexCoordinates(to.hex_coordinates);
            return false;
        }
        MoveToHexCoordinates(to.hex_coordinates);
        return true;
    }
    override public void PrePerformSkill(HexCell to) {
        MoveToHexCoordinates(to.hex_coordinates);
    }
    private List<HexCoordinates> GetPossibleTurns(HexCoordinates from,  Vector3[] basis_turns, HexCell.State check)
    {
        List<HexCoordinates> ans = new List<HexCoordinates>();

        foreach (Vector3 turn in basis_turns) {
            HexCoordinates candidate = HexCoordinates.FromXY(from.X + (int)turn[0], from.Y + (int)turn[1]);
            
            if (!gay_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                HexCell cell_candidate = gay_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if (cell_candidate.state == check) {
                    ans.Add(candidate);
                }
            }
        }
        return ans;
    }

    override public List<HexCoordinates> GetPossibleMovementTurns()
    {
        return GetPossibleTurns(hex_coordinates, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCoordinates> GetPossibleSkillTriggerTurns()
    {
        return GetPossibleTurns(hex_coordinates, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus);
    }

    
    override public List<HexCoordinates> GetPossibleSkillTurns(HexCell from)
    {
        List<HexCoordinates> ans = GetPossibleTurns(from.hex_coordinates, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
        ans.AddRange(GetPossibleTurns(from.hex_coordinates, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus));
        return ans;
    }

}
