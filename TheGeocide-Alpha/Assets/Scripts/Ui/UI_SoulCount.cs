using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SoulCount : MonoBehaviour
{
    [SerializeField]
    private GameStateChannel _gsc;

    [SerializeField]
    private TMP_Text _UStext;
    private void Awake()
    {
        _gsc.OnUnfortunateSoulAmoutUpdate += OnUnfortunateSoulAmoutUpdate;
    }

    private void OnDestroy()
    {
        _gsc.OnUnfortunateSoulAmoutUpdate -= OnUnfortunateSoulAmoutUpdate;
    }

    // Start is called before the first frame update
    void Start()
    {
        _gsc.OnUnfortunateSoulAmoutRequested();
    }


    private void OnUnfortunateSoulAmoutUpdate(int newAmount)
    {
        _UStext.text = $"{newAmount}";
    }

}
