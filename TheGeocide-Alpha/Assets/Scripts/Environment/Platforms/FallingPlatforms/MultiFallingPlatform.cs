using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiFallingPlatform : MonoBehaviour
{


    [Header("Common")]
    public bool IsChained;
    public float BreakTime;
    public int PlatformLength;

    [SerializeField]
    private PlatfromPaletteSO _plaftformPalette;

    private static float _offsetFromOrigin = 0.5f;
    private static float _tileHeight = 0.5f;
    private static float _tileWidth = 1f;

    [Header("Big platform")]
    [SerializeField]
    private FallingPlatform _plaftformPrefab;
    [SerializeField]
    private GameObject _plaftformGfxPrefab;

    [Header("Chained platforms")] 
    public float ConnectTime;
    [SerializeField]
    private ChainedFallingPlatform _chainedPlaftformPrefab;
    private Dictionary<int, ChainedFallingPlatform> _chainedPlatformDico;

    // Start is called before the first frame update
    void Start()
    {

        if (IsChained) {
            _InitChainedPlateform();
            return;
        }
                
        _InitBigPlateform();
    }

    private void _InitBigPlateform()
    {
        var platform = Instantiate(_plaftformPrefab, transform);

        var container = platform.transform.Find("GfxContainer");
        for (var i = PlatformLength - 1; i >= 0; i--)
        {
            var gfx = Instantiate(_plaftformGfxPrefab, container);
            gfx.transform.position = new Vector2(platform.transform.position.x + i + _offsetFromOrigin, platform.transform.position.y);

            gfx.GetComponent<SpriteRenderer>().sprite = _plaftformPalette.GetDisplaySprite(i, PlatformLength);
        }

        platform.SetBreakTime(BreakTime);

        BoxCollider2D collider = platform.GetComponent<BoxCollider2D>();
        collider.offset = GetCenter();
        collider.size = GetSize();
    }

    private void _InitChainedPlateform()
    {
        _chainedPlatformDico = new Dictionary<int, ChainedFallingPlatform>();
        for (var i = PlatformLength - 1; i >= 0; i--)
        {
            var plat = Instantiate(_chainedPlaftformPrefab, transform);
            plat.transform.position = new Vector2(transform.position.x + i + _offsetFromOrigin, transform.position.y);
            plat.SetBreakTime(BreakTime);

            _chainedPlatformDico.Add(i, plat);
            if (i != PlatformLength - 1)
            {
                plat.SetConnection(_chainedPlatformDico[i+1], ConnectTime);
            }

            plat.SetSprite(_plaftformPalette.GetDisplaySprite(i, PlatformLength));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        #if UNITY_EDITOR
                Gizmos.color = Color.blue;
                Gizmos.DrawCube(GetCenter(transform), GetSize()); ;
        #endif
    }

    private Vector3 GetCenter(Transform origin = null)
    {
        if (origin)
        {
            return new Vector3(origin.position.x + PlatformLength * _tileWidth / 2, origin.position.y + _tileHeight / 2);
        }

        return new Vector3(PlatformLength * _tileWidth / 2, _tileHeight / 2);
    }

    private Vector3 GetSize()
    {
        return new Vector3(PlatformLength, _tileHeight);
    }
}
