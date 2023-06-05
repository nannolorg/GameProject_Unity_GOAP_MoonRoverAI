using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityInfo : MonoBehaviour
{
    public enum ItemName
    {
        Samples
    }
    public HomeBase home { get; private set; } = null;
    public Dictionary<ItemName, int> Inventory;

    public void SetHome(HomeBase _home)
    {
        home = _home;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
