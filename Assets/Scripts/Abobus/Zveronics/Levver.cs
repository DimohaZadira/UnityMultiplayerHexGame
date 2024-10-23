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
    public Levver ()
    {
        action_type = typeof(LevverSkill);
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
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        List<HexCell> ans = new List<HexCell>();
        foreach (Vector3 turn in basis_turns) {
            // while () {
            //     i++;
            // }
        }
        return ans;
    }

    
    override public List<HexCell> GetPossibleSkillTurns(HexCell from)
    {
        List<HexCell> ans = GetPossibleTurns(from, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
        ans.Add(cell);
        return ans;
    }

}
