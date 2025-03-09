using System;
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
    public override Type GetSkillType()
    {
        throw new NotImplementedException();
    }
    override public List<HexCell> GetPossibleMovementTurns()
    {
        return GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        Vector3[] basis_turns = RangeOneComponent.GetBasisTurns();
        List<HexCell> ans = new List<HexCell>();
        foreach (Vector3 turn in basis_turns)
        {
            // while () {
            //     i++;
            // }
        }
        return ans;
    }

    
}
