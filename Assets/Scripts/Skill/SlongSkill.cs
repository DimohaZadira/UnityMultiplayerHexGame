using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlongSkill : IAction
{
    private HexCell applied_to;
    public HexCell AppliedTo {
        get {
            return applied_to;
        }
        set {
            applied_to = value;
        }
    }

    public string DebugMessage()
    {
        throw new System.NotImplementedException();
    }

    public void Invoke()
    {
        Debug.Log("Псаси");
    }
}
