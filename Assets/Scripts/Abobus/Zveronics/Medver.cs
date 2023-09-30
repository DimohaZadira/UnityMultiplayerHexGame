using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*ВЫБРАН МЕДВЕР: зелёным подсвечиваются все гексы в радиусе 1 за исключением тех, на которых стоят другие звероники. 
Гексы и силуэты всех звероников, граничащих с МЕДВЕРОМ, и гекс, на котором стоит сам МЕДВЕРР, подсвечиваются жёлтым, 
и между всеми соседними стоят обоюдные стрелочки. После клика на любую пару подсвеченных гексов звероники, 
стоящие на них, поменяются местами (менять местами себя с любым соседом не запрещено). 
Когда продолжение хода будет невозможно, на экран выводится символ «конец хода».*/
public class Medver : Abobus
{
    override public void RefreshSelf()
    {
    }
    override public bool PerformSkill(HexCell from, HexCell to)
    {
        Abobus from_move = gay_manager.GetAbobusByHexCoordinates(from.hex_coordinates);
        Abobus to_move = gay_manager.GetAbobusByHexCoordinates(to.hex_coordinates);
        to_move.MoveToHexCoordinates(from.hex_coordinates);
        from_move.MoveToHexCoordinates(to.hex_coordinates);
        
        return true;
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
        List<HexCoordinates> ans = GetPossibleTurns(hex_coordinates, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus);
        ans.Add(hex_coordinates);
        return ans;
    }

}