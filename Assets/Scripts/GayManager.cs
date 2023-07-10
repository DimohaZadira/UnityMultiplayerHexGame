using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GayManager : MonoBehaviour
{
    private GameObject abobus_go;
    public GameObject hex_grid;
    private PlayerInput playerInput;
    private TouchControls touchControls;
    
    public int x, z;
    // Start is called before the first frame update
    void Start()
    {
        hex_grid.GetComponent<HexGrid>().CreateGrid();
        HexCoordinates hc = new HexCoordinates(0,0);
        
        abobus_go = Instantiate(Resources.Load("Knight Variant")) as GameObject;
        abobus_go.AddComponent<Abobus>();
        abobus_go.GetComponent<Abobus>().hex_coordinates = hc;
        abobus_go.transform.SetParent(hex_grid.transform);
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
        HexCoordinates hc = HexCoordinates.ToOffsetCoordinates(x,z);
        abobus_go.transform.position = HexCoordinates.FromHexCoordinates(hc);  
    }
    public void OnMouseClick(InputAction.CallbackContext value) {
        Ray inputRay = Camera.main.ScreenPointToRay(touchControls.Player.ClickPosition.ReadValue<Vector2>());
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            GameObject hit_abobus = hit.collider.transform.gameObject;
            if (hit_abobus.GetComponent<Abobus>()) {
                Debug.Log("Hit abobus");
                List<HexCell> to_highlight = new List<HexCell>();

                HexCoordinates abobus_grid_coords = hit_abobus.GetComponent<Abobus>().hex_coordinates;
                HexCell hit_cell = hex_grid.GetComponent<HexGrid>().GetCellByHexCoordinates(abobus_grid_coords);
                to_highlight.Add(hit_cell.GetComponent<HexCell>());

                foreach (HexCell cell in to_highlight) {
                    cell.GetComponent<HighlightableCell>().SwitchHighlight();
                }
                
                Debug.Log(hit_cell.GetComponent<HexCell>().coordinates.ToString());

            }
            
            }
        
        
        
    }




    



}
