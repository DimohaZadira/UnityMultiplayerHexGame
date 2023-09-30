using System.Collections;
using System.Collections.Generic;
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
    override public void RefreshSelf()
    {
    }
    override public bool PerformSkill(HexCell from, HexCell to)
    {
        Abobus to_move = gay_manager.GetAbobusByHexCoordinates(from.hex_coordinates);
        to_move.MoveToHexCoordinates(to.hex_coordinates);
        MoveToHexCoordinates(from.hex_coordinates);
        
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
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        List<HexCoordinates> ans = new List<HexCoordinates>();
        foreach (Vector3 turn in basis_turns) {
            // while () {
            //     i++;
            // }
        }
        return ans;
    }

    
    override public List<HexCoordinates> GetPossibleSkillTurns(HexCell from)
    {
        List<HexCoordinates> ans = GetPossibleTurns(from.hex_coordinates, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
        ans.Add(hex_coordinates);
        return ans;
    }

}
