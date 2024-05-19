using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotemAnimatorEventHandler : MonoBehaviour
{
    private Totem _TotemScript;

    // Start is called before the first frame update
    void Start()
    {
        _TotemScript = GetComponentInParent<Totem>();
    }

    //Called by animator
    public void OnTotemOpeningCompleted()
    {
        _TotemScript.OnTotemRaiseCompleted();
    }
}
