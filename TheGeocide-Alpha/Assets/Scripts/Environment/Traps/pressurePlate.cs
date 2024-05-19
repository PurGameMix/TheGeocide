using Assets.Data.Common.Definition;
using Assets.Data.GameEvent.Definition;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pressurePlate : MonoBehaviour
{
    [SerializeField]
    private BoxCollider2D _boxCollider;

    [SerializeField]
    private Transform _gfx;

    [SerializeField]
    private GameEventChannel _geChannel;

    [SerializeField]
    private AudioController _audioController;

    [SerializeField]
    private LayerMask _canTriggerLayer;

    private bool _isPressed = false;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isInLayerMask(collision.gameObject.layer) && !_isPressed)
        {
            PlatePressIn();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isInLayerMask(collision.gameObject.layer) && _isPressed)
        {
            PlatePressOut();
        }
    }

    private bool isInLayerMask(int layerValue)
    {
        return _canTriggerLayer.Contains(layerValue);
    }
    private void PlatePressIn()
    {
       
        _gfx.Translate(new Vector2(0, -0.02f));

        _geChannel.RaiseEvent(new GameEvent()
        {
            Origin = gameObject,
            Type = GameEventType.PlatePressuredIn
        });
        _isPressed = true;
        _audioController.Play("PressureIn");
    }

    private void PlatePressOut()
    {

        _gfx.Translate(new Vector2(0, 0.02f));

        _geChannel.RaiseEvent(new GameEvent()
        {
            Origin = gameObject,
            Type = GameEventType.PlatePressuredOut
        });
        _isPressed = false;
        _audioController.Play("PressureOut");
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
