using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Debugger : MonoBehaviour
{
    public static Debugger Instance { get; private set; } = null;
    public TextMeshProUGUI DebugText;
    private string OutputQuery = string.Empty;
    public List<GOAPBrain> TrackedAI { get; private set; } = new List<GOAPBrain>();

    private void Awake()
    {
        Instance = this;
    }


    private void Update()
    {
        //Build the Query


        DebugText.text = OutputQuery;
    }

    public void SetDebugInfo(string _query)
    {
        OutputQuery = _query; 
    }


}
