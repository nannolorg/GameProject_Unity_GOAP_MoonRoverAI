using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OverheadUI : MonoBehaviour
{
    GOAPBrain AIBrain;
    public TextMeshProUGUI OverheadText;

    // Start is called before the first frame update
    void Start()
    {
        AIBrain = GetComponent<GOAPBrain>(); 
    }

    // Update is called once per frame
    void Update()
    {
        OverheadText.text = AIBrain.GetActiveGoal();
    }
}
