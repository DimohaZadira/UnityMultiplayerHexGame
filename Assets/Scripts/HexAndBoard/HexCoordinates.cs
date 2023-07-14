using UnityEngine;
using System.Collections.Generic;
using System;

[System.Serializable]
public struct HexCoordinates : IEquatable<HexCoordinates>{
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

	private HexCoordinates (int x, int z) {
		this.x = x;
		this.z = z;
	}

    public static HexCoordinates FromXY (int x, int y)
    {
        return new HexCoordinates(x, -y - x);
    }
    public static HexCoordinates FromXZ (int x, int z)
    {
        return new HexCoordinates(x, z);

    }
    public static HexCoordinates FromYZ (int y, int z)
    {
        return new HexCoordinates(-y - z, z);
    }

    public int Y {
		get {
			return -X - Z;
		}
	}

    public static bool operator ==(HexCoordinates hc1, HexCoordinates hc2)
    {
        if (ReferenceEquals(hc1, hc2)) 
            return true;
        if (ReferenceEquals(hc1, null)) 
            return false;
        if (ReferenceEquals(hc2, null))
            return false;
        return hc1.Equals(hc2);
    }
    public static bool operator !=(HexCoordinates hc1, HexCoordinates hc2)
    {
        return !(hc1 == hc2);
    }
    public bool Equals(HexCoordinates hc) {
        if (ReferenceEquals(hc, null)) {
            return false;
        }
        if (ReferenceEquals(this, hc)) {
            return true;
        }
        return x.Equals(hc.X) && z.Equals(hc.Z);
    }

    #nullable enable
    public override bool Equals(object? obj) => Equals(obj as HexCoordinates?);
    #nullable disable

    public override int GetHashCode()
    {
        unchecked
        {
            int hashCode = x.GetHashCode();
            hashCode = (hashCode * 397) ^ z.GetHashCode();
            return hashCode;
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