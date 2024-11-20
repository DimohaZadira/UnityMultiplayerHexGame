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

    override public List<HexCell> GetPossibleMovementTurns()
    {
        return GetPossibleTurns(cell, RangeTwoComponent.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        return GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus);
    }

}