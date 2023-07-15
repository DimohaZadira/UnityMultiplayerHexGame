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

    public enum Teams {
        blue, 
        yellow
    };
    private Dictionary<Teams, List<GameObject>> abobi;

    public Abobus chosen_abobus;
    
    public int x, z;

    GameObject SpawnAbobus(Object original, Vector2 hex_coords_vec, Teams team)
    {
        var hc = HexCoordinates.FromXZ((int)hex_coords_vec.x, (int)hex_coords_vec.y);
        GameObject abobus_go = Instantiate(original) as GameObject;
        abobus_go.AddComponent<Knight>();
        abobus_go.GetComponent<Knight>().MoveToHexCoordinates(hc);
        abobus_go.GetComponent<Knight>().team = team;

        return abobus_go;
    }
    // Start is called before the first frame update
    void Start()
    {
        chosen_abobus = null;
        abobi = new Dictionary<Teams, List<GameObject>>();
        foreach (Teams team in System.Enum.GetValues(typeof(Teams))) {
            abobi.Add(team, new List<GameObject>());
        }

        hex_grid = hex_grid_go.GetComponent<HexGrid>();
        hex_grid.CreateGrid();
        
        abobus_go = SpawnAbobus(Resources.Load("Knight Variant"), new Vector2(0, 0), Teams.blue);
        abobi[Teams.blue].Add(abobus_go);

        abobus_2_go = SpawnAbobus(Resources.Load("Knight Variant"), new Vector2(2, 1), Teams.yellow);
        abobi[Teams.yellow].Add(abobus_2_go);

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
            hex_cell.GetComponent<HighlightableCell>().SwitchHighlight(this);
        }
    }

    public List<Abobus> GetListAbobiByHexCoordinates (HexCoordinates hex_coordinates)
    {
        List<Abobus> ans = new List<Abobus>();
        foreach(List<GameObject> abobi_in_team in abobi.Values) {
            foreach(GameObject abobus_go in abobi_in_team) {
                if (abobus_go.GetComponent<Abobus>().hex_coordinates == hex_coordinates) {
                    ans.Add(abobus_go.GetComponent<Abobus>());
                }
            }
        }
        // Debug.Log("On cell " + hex_coordinates.ToString() + " detected " + ans.Count + " abobi");
        return ans;
    } 

    public void OnMouseClick(InputAction.CallbackContext value) {
        Ray inputRay = Camera.main.ScreenPointToRay(touchControls.Player.ClickPosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            GameObject hit_go = hit.collider.transform.gameObject;

            Abobus abobus = hit_go.GetComponent<Abobus>();
            if (abobus) {
                if (ReferenceEquals(abobus, chosen_abobus)) {
                    chosen_abobus = null;
                } else if (chosen_abobus) {
                    chosen_abobus.ChangeState();
                    ListCellsChangeState(chosen_abobus.GetPossibleTurns());
                    chosen_abobus = abobus;
                } else {
                    chosen_abobus = abobus;
                }
                
                abobus.ChangeState();
                ListCellsChangeState(abobus.GetPossibleTurns());
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
