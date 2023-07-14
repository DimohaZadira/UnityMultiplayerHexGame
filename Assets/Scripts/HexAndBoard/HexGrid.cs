using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections.Generic;
public class HexGrid : MonoBehaviour {

	Canvas grid_canvas;
		
	public int width;
	public int height;

	public HexCell cell_prefab;
    public TextMeshProUGUI cell_label_prefab;
	
	public Dictionary<HexCoordinates, int> hex_coords_to_index;
    HexCell[] cells;

	public void CreateGrid () 
	{
		hex_coords_to_index = new Dictionary<HexCoordinates, int>();
        grid_canvas = GetComponentInChildren<Canvas>();
		cells = new HexCell[height * width];
		int i = 0;
		for (int z = 0; z < height; z++) {
			if (z % 2 == 0) {
				for (int x = 0; x < width; x++) {
					CreateCell(x, z, i++);
				}
			} else {
				for (int x = 0; x < width - 1; x++) {
					CreateCell(x, z, i++);
				}
			}
		}
	}
	void CreateCell (int x, int z, int i)
	{
		Vector3 position = HexCoordinates.FromHexCoordinates(HexCoordinates.FromXZ(x, z));
		
		HexCell cell = cells[i] = Instantiate<HexCell>(cell_prefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		
		hex_coords_to_index.Add(cell.coordinates, i);

		TextMeshProUGUI label = Instantiate<TextMeshProUGUI>(cell_label_prefab);
		label.rectTransform.SetParent(grid_canvas.transform, false);
		label.rectTransform.anchoredPosition =
			new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}

	public HexCell GetCellByHexCoordinates(HexCoordinates hc) 
	{
		return cells[hex_coords_to_index[hc]];
	}

    public void ClearOutOfBoundsCells(List<HexCoordinates> list)
    {
        HexCoordinates[] arr = list.ToArray();
        // Debug.Log(hex_coords_to_index);
        foreach (HexCoordinates hex_coords in arr) {
            if (!hex_coords_to_index.ContainsKey(hex_coords)) {
                list.Remove(hex_coords);
            }
        }
    }


	
}