using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creator : MonoBehaviour
{
    public List<int> content;
    // Start is called before the first frame update
    void Start()
    {
        content = new List<int>();
        content.Add(2);
        content.Add(2);
        content.Add(8);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
