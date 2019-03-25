using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public int winCount;
    public int loseCount;

    public Text ControlText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateText() {
        ControlText.text = "Wins: " + winCount + "\nLoses: " + loseCount;
    }
}
