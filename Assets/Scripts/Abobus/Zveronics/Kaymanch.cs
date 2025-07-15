using System;
using System.Collections.Generic;
using UnityEngine;
/*ВЫБРАН КАЙМАНЧ: зеленым подсвечиваются все гексы в радиусе 1 за исключением тех, на которых стоят другие звероники. 
Если КАЙМАНЧ граничит с союзным звероником, то гекс с этим союзником подсвечивается желтым. 
При клике на этот гекс желтым подсвечиваются свободные гексы, находящиеся в радиусе 2 от КАЙМАНЧА. 
После клика на один из подсвеченных гексов КАЙМАНЧ останется на изначальном месте, 
а кликнутый союзник переместится на выбранный гекс. Когда продолжение хода будет невозможно, 
на экран выводится символ «конец хода».*/
public class Kaymanch : Abobus
{
    public override Type GetSkillType()
    {
        return typeof(KaymanchSkill);
    }

    override public List<HexCell> GetPossibleMovementTurns()
    {
        return GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        List<HexCell> ans = new List<HexCell>();

        foreach (Vector3 turn in RangeOneComponent.GetBasisTurns())
        {
            HexCoordinates candidate = HexCoordinates.FromXY(cell.hex_coordinates.X + (int)turn[0], cell.hex_coordinates.Y + (int)turn[1]);

            if (!game_manager.hex_grid.CheckHexCoordsOutOfBounds(candidate))
            {
                HexCell cell_candidate = game_manager.hex_grid.GetCellByHexCoordinates(candidate);
                if ((cell_candidate.state == HexCell.State.abobus) && (game_manager.GetAbobusByHexCoordinates(candidate).team == team))
                {
                    ans.Add(cell_candidate);
                }
            }
        }
        return ans;
    }

    public List<HexCell> GetPossibleSkillTurns(HexCell from)
    {
        return GetPossibleTurns(from, RadiusTwoComponent.GetBasisTurns(), HexCell.State.empty);
    }

}