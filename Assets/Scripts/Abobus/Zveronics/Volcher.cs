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
    override public void RefreshSelf()
    {
    }
    override public bool PerformSkill(HexCell from, HexCell to)
    {
        Abobus to_move = game_manager.GetAbobusByHexCoordinates(from.hex_coordinates);
        to_move.MoveToHexCoordinates(to.hex_coordinates);
        MoveToHexCoordinates(from.hex_coordinates);
        
        return true;
    }
    private List<HexCoordinates> GetPossibleTurns(HexCoordinates from,  Vector3[] basis_turns, HexCell.State check)
    {
        List<HexCoordinates> ans = new List<HexCoordinates>();

        foreach (Vector3 turn in basis_turns) {
            HexCoordinates candidate = HexCoordinates.FromXY(from.X + (int)turn[0], from.Y + (int)turn[1]);
            
            if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate)) {
                HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if (cell_candidate.state == check) {
                    ans.Add(candidate);
                }
            }
        }
        return ans;
    }

    override public List<HexCoordinates> GetPossibleMovementTurns()
    {
        return GetPossibleTurns(hex_coordinates, RangeTwoComponent.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCoordinates> GetPossibleSkillTriggerTurns()
    {
        return GetPossibleTurns(hex_coordinates, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus);
    }

    
    override public List<HexCoordinates> GetPossibleSkillTurns(HexCell from)
    {
        List<HexCoordinates> ans = GetPossibleTurns(from.hex_coordinates, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
        ans.Add(hex_coordinates);
        return ans;
    }

}