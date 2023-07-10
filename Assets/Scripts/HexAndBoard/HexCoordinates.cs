using UnityEngine;

[System.Serializable]
public struct HexCoordinates {
    [SerializeField]
	private int x, z;

	public int X {
		get {
			return x;
		}
	}

	public int Z {
		get {
			return z;
		}
	}

	public HexCoordinates (int x, int z) {
		this.x = x;
		this.z = z;
	}
    public int Y {
		get {
			return -X - Z;
		}
	}

	public override string ToString () {
		return "(" +
			X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
	}

	public string ToStringOnSeparateLines () {
		return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
	}

    public static HexCoordinates FromOffsetCoordinates (int x, int z) {
		return new HexCoordinates(x - z / 2, z);
	}

	public static HexCoordinates ToOffsetCoordinates (int x, int z) {
		return new HexCoordinates(x + z / 2, z);
	}
	
	public static Vector3 FromHexCoordinates (HexCoordinates hc) {
		return new Vector3((hc.X + hc.Z * 0.5f - hc.Z / 2) * (HexMetrics.innerRadius * 2f)
		                 , 0
						 , hc.Z * (HexMetrics.outerRadius * 1.5f));
	}

	public static HexCoordinates FromWorldOffsetPosition (Vector3 offset_position) {
		int z = (int)(offset_position.z / (HexMetrics.outerRadius * 1.5f));
		int x = -1;
		if (z % 2 == 0) {
			x = (int)Mathf.Round(offset_position.x / (2 * HexMetrics.innerRadius));
		} else {
			x = (int)Mathf.Round((offset_position.x - (HexMetrics.outerRadius / 2)) / (2 * HexMetrics.innerRadius));
		}
		
		return new HexCoordinates(x, z);
	}



}