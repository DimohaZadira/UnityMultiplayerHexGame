using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public enum GayState {
    Idle, AbobusChosen, CellChosen
}

public class GayManager : MonoBehaviour
{
    private GameObject abobus_go;
    public GameObject hex_grid_go;
    private HexGrid hex_grid;
    private PlayerInput playerInput;
    private TouchControls touchControls;
    private GayState gay_state;
    
    public int x, z;
    // Start is called before the first frame update
    void Start()
    {
        hex_grid = hex_grid_go.GetComponent<HexGrid>();
        hex_grid.CreateGrid();
        HexCoordinates hc = HexCoordinates.FromXY(0,0);
        
        abobus_go = Instantiate(Resources.Load("Knight Variant")) as GameObject;
        abobus_go.AddComponent<Knight>();
        abobus_go.GetComponent<Knight>().hex_coordinates = hc;
        abobus_go.transform.SetParent(hex_grid_go.transform);
        abobus_go.transform.localPosition = HexCoordinates.FromHexCoordinates(hc);
        abobus_go.transform.localRotation = Quaternion.Euler(0, 120, 0);
        
        playerInput = GetComponent<PlayerInput>();

        //touchControls.Player.Click.started += ctx => OnMouseClick(ctx);
        touchControls.Player.Click.performed += ctx => OnMouseClick(ctx);
    }


    void Awake() {
        touchControls = new TouchControls();
    }
    void OnEnable() {
        touchControls.Enable();
    }
    void OnDisable() {
        touchControls.Disable();
    }

    void Update() {
        // HexCoordinates hc = HexCoordinates.ToOffsetCoordinates(x,z);
        // abobus_go.transform.position = HexCoordinates.FromHexCoordinates(hc);  
    }
    public void OnMouseClick(InputAction.CallbackContext value) {
        Ray inputRay = Camera.main.ScreenPointToRay(touchControls.Player.ClickPosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            GameObject hit_go = hit.collider.transform.gameObject;
            if (hit_go.GetComponent<Abobus>()) {
                HexCoordinates abobus_grid_coords = hit_go.GetComponent<Abobus>().hex_coordinates;
                Debug.Log("Hit abobus at " + abobus_grid_coords.ToString());


                // HexCell hit_cell = hex_grid.GetComponent<HexGrid>().GetCellByHexCoordinates(abobus_grid_coords);
                // to_highlight.Add(hit_cell.GetComponent<HexCell>());
                List<HexCoordinates> to_highlight = hit_go.GetComponent<Abobus>().GetPossibleTurns();
                hex_grid.ClearOutOfBoundsCells(to_highlight);

                foreach (HexCoordinates hex_coords in to_highlight) {
                    HexCell hex_cell = hex_grid.GetCellByHexCoordinates(hex_coords);
                    hex_cell.GetComponent<HighlightableCell>().SwitchHighlight();
                }

            } else if (hit_go.GetComponent<HexCell>()) {

            }
            
            }
        
        
        
    }




    



}
