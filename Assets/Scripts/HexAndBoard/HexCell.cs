using UnityEngine;

public class HexCell : MonoBehaviour {
    public State state;
    public enum State {
        out_of_bounds, abobus, empty
    };
	public HexCoordinates hex_coordinates;

}