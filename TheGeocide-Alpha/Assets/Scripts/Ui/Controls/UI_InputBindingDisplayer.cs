using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_InputBindingDisplayer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _tmp;

    [SerializeField]
    private string _actionName;

    // Start is called before the first frame update
    void Start()
    {
        _tmp.text = InputHandler.GetBindingDisplayString(_actionName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
