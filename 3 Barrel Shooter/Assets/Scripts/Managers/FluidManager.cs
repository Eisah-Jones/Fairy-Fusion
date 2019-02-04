using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class FluidManager
{

    private List<GameObject> elementFluids;

    public FluidManager()
    {
        elementFluids = new List<GameObject>();
        LoadFluids(); 
    }

    public void LoadFluids()
    {
        TextAsset txt = (TextAsset)Resources.Load("Fluids/Fluid Systems/loadFluids", typeof(TextAsset));
        string[] lines = Regex.Split(txt.text, "\n|\r|\r\n");

        foreach (string line in lines)
        {
            elementFluids.Add(Resources.Load<GameObject>("Fluids/Fluid Systems/" + line));
        }
    }

    public GameObject GetFluidByID(int i)
    {
        return elementFluids[i-1];
    }
}
