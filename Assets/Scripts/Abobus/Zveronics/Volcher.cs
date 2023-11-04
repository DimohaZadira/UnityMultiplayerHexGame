using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*ВЫБРАН ВОЛЧЕР: зелёным подсвечиваются все гексы в радиусе 2 за исключением тех, на которых стоят другие звероники. 
Если по прямой от ВОЛЧЕРА находится другой звероник, то предпоследний по направлению от ВОЛЧЕРА к этому зверонику 
гекс подсвечивается желтым. При клике на подсвеченный гекс ВОЛЧЕР перемещается на него. 
Если ВОЛЧЕР граничит с союзным звероником, то силуэт этого звероника и его гекс подсвечивается желтым. 
При клике на подсвеченный силуэт соседа тот становится обычным зверорником и получает право внеочередного хода. 
Применение обеих способностей за ход разрешено. Когда продолжение хода будет невозможно, 
на экран выводится символ «конец хода».*/
public class Volcher : Abobus
{
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
        return GetPossibleTurns(cell, RangeTwoComponent.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        return GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus);
    }

    
    override public List<HexCell> GetPossibleSkillTurns(HexCell from)
    {
        List<HexCell> ans = GetPossibleTurns(from, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
        ans.Add(cell);
        return ans;
    }

}