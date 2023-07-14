using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Abobus : MonoBehaviour
{
    private static Vector3[] turns;
    public HexCoordinates hex_coordinates;

    public abstract List<HexCoordinates> GetPossibleTurns(HexCoordinates? from = null);

}
