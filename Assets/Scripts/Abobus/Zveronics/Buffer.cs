using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*ВЫБРАН БАФФЕР: зеленым подсвечиваются все гексы в радиусе 2 за исключением тех, на которых стоят другие звероники. 
Если БАФФЕР граничит с другим звероником (кроме КАЙМАНЧА), то гекс с соседом подсвечивается желтым. 
При нажатии на него желтым подсвечиваются все гексы в радиусе 1 от выбранного. 
Если на каком-то из них стоит другой звероник, то такой ход невозможен (желтым этот гекс не подсвечивается). 
После нажатия на один из подсвеченных желтым гексов противник будет перемещен на выбранный гекс, а БАФФЕР – на гекс, 
на котором стоял противник до применения способности. Поменять местами БАФФЕРА и любого его соседа возможно. 
Когда продолжение хода будет невозможно, на экран выводится символ «конец хода».*/
public class Buffer : Abobus
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