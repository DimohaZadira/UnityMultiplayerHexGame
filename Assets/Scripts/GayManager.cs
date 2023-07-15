using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GayManager : MonoBehaviour
{
    private GameObject abobus_go;
    private GameObject abobus_2_go;
    public GameObject hex_grid_go;
    private HexGrid hex_grid;
    private PlayerInput playerInput;
    private TouchControls touchControls;

    //
    private Abobus chosen_abobus;
    
    public int x, z;

    GameObject SpawnAbobus(Object original, Vector2 hex_coords_vec)
    {
        var hc = HexCoordinates.FromXZ((int)hex_coords_vec.x, (int)hex_coords_vec.y);
        GameObject abobus_go = Instantiate(original) as GameObject;
        abobus_go.AddComponent<Knight>();
        abobus_go.GetComponent<Knight>().MoveToHexCoordinates(hc);
        // abobus_go.GetComponent<Knight>().hex_coordinates = hc;
        // abobus_go.transform.SetParent(hex_grid_go.transform);
        // hc = HexCoordinates.ToOffsetCoordinates(hc.X, hc.Z);
        // abobus_go.transform.localPosition = HexCoordinates.FromHexCoordinates(hc);

        // abobus_go.transform.localRotation = Quaternion.Euler(0, 120, 0);

        return abobus_go;
    }
    // Start is called before the first frame update
    void Start()
    {
        chosen_abobus = null;

        hex_grid = hex_grid_go.GetComponent<HexGrid>();
        hex_grid.CreateGrid();
        
        abobus_go = SpawnAbobus(Resources.Load("Knight Variant"), new Vector2(0, 0));

        abobus_2_go = SpawnAbobus(Resources.Load("Knight Variant"), new Vector2(3, 3));
        

        playerInput = GetComponent<PlayerInput>();

        //touchControls.Player.Click.started += ctx => OnMouseClick(ctx);
        touchControls.Player.Click.performed += ctx => OnMouseClick(ctx);
        touchControls.Player.RightClick.performed += ctx => OnRightMouseClick(ctx);
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

    public void ListCellsChangeState (List<HexCoordinates> coords_list) 
    {
        hex_grid.ClearOutOfBoundsCells(coords_list);

        foreach (HexCoordinates hex_coords in coords_list) {
            HexCell hex_cell = hex_grid.GetCellByHexCoordinates(hex_coords);
            hex_cell.GetComponent<HighlightableCell>().SwitchHighlight();
        }
    }

    public void OnMouseClick(InputAction.CallbackContext value) {
        Ray inputRay = Camera.main.ScreenPointToRay(touchControls.Player.ClickPosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            GameObject hit_go = hit.collider.transform.gameObject;

            Abobus abobus = hit_go.GetComponent<Abobus>();
            if (abobus) {
                abobus.ChangeState();
                ListCellsChangeState(abobus.GetPossibleTurns());

                if (ReferenceEquals(abobus, chosen_abobus)) {
                    chosen_abobus = null;
                } else if (chosen_abobus) {
                    chosen_abobus.ChangeState();
                    ListCellsChangeState(chosen_abobus.GetPossibleTurns());
                    chosen_abobus = abobus;
                } else {
                    chosen_abobus = abobus;
                }
            }
            
        }
        
        
    }

    public void OnRightMouseClick(InputAction.CallbackContext value) {
        Ray inputRay = Camera.main.ScreenPointToRay(touchControls.Player.ClickPosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            GameObject hit_go = hit.collider.transform.gameObject;

            HexCell hex_cell = hit_go.GetComponent<HexCell>();
            if (hex_cell) {
                if (hex_cell.GetComponent<HighlightableCell>().highlighted) {
                    chosen_abobus.ChangeState();
                    ListCellsChangeState(chosen_abobus.GetPossibleTurns());
                    chosen_abobus.MoveToHexCoordinates(hex_cell.hex_coordinates);
                    chosen_abobus = null;
                }
            }
        }
    }

}
