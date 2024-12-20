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

    override public List<HexCell> GetPossibleMovementTurns()
    {
        return GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.empty);
    }

    override public List<HexCell> GetPossibleSkillTriggerTurns()
    {
        return GetPossibleTurns(cell, RangeOneComponent.GetBasisTurns(), HexCell.State.abobus);
    }

}