using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Highlightable: MonoBehaviour
{
    public bool highlighted;
    public void SwitchHighlight(GayManager gm)
	{
		highlighted = !highlighted;
		if (highlighted) {
            SetHighlighted(gm);
		} else {
            SetDefault();
		}
	}

    public abstract void SetHighlighted(GayManager gm);
    public abstract void SetDefault();

}
