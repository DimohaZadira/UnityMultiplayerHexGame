using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*ВЫБРАН МЕДВЕР: зелёным подсвечиваются все гексы в радиусе 1 за исключением тех, на которых стоят другие звероники. 
Гексы и силуэты всех звероников, граничащих с МЕДВЕРОМ, и гекс, на котором стоит сам МЕДВЕР, подсвечиваются жёлтым, 
и между всеми соседними стоят обоюдные стрелочки. После клика на любую пару подсвеченных гексов звероники, 
стоящие на них, поменяются местами (менять местами себя с любым соседом не запрещено). 
Когда продолжение хода будет невозможно, на экран выводится символ «конец хода».*/
public class Medver : Abobus
{
    public Medver()
    {
        action_type = typeof(MedverSkill);
    }
    private List<HexCell> GetPossibleTurns(HexCell from,  Vector3[] basis_turns, HexCell.State check)
    {
        List<HexCell> ans = new List<HexCell>();

        foreach (Vector3 turn in basis_turns) {
            HexCoordinates candidate = HexCoordinates.FromXY(from.hex_coordinates.X + (int)turn[0], from.hex_coordinates.Y + (int)turn[1]);
            
            if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if (cell_candidate.state == check) {
                    ans.Add(cell_candidate);
                }
            }
        }
        return ans;
    }

    override public List<HexCell> GetPossibleMovementTurns()
    {
        return GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        return GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus);
    }

    
    override public List<HexCell> GetPossibleSkillTurns(HexCell from)
    {
        List<HexCell> ans = GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus);
        ans.Add(cell);
        return ans;
    }

}