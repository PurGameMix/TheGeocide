using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{

    public int PlatformLength;

    [SerializeField]
    private PlatfromPaletteSO _plaftformPalette;

    public bool IsAwakePlaying;
    public MovingPlatformType PathType;
    public float Speed;

    private static float _offsetFromOrigin = 0.5f;
    private static float _tileHeight = 0.5f;
    private static float _tileWidth = 1f;

    [SerializeField]
    private GameObject _plaftform;
    [SerializeField]
    private GameObject _pathContainer;
    private List<Transform> _pathList;
    [SerializeField]
    private GameObject _plaftformGfxPrefab;
    private int _nextDestination;
    private bool _isMoving;
    private bool _isReversePath = false;

    // Start is called before the first frame update
    void Start()
    {  
        _InitMovingPlateform();
        _InitPath();
        if (IsAwakePlaying)
        {
            Move();
        }
    }

    private void _InitPath()
    {

        if (_pathContainer.transform.childCount == 0)
        {
            Debug.LogWarning($"{GetType().FullName} : Platform has no path assigned");
            return;
        }
        _pathList = new List<Transform>();
        foreach (Transform child in _pathContainer.transform)
        {
            _pathList.Add(child);
        }

        _plaftform.transform.position = _pathList[0].position;
        _nextDestination = 1;
    }

    private void Move()
    {
        _isMoving = true;
    }

    private void Stop()
    {
        _isMoving = false;
    }

    private void _InitMovingPlateform()
    {

        for (var i = PlatformLength - 1; i >= 0; i--)
        {
            var gfx = Instantiate(_plaftformGfxPrefab, _plaftform.transform);
            gfx.transform.position = new Vector2(_plaftform.transform.position.x + i + _offsetFromOrigin, _plaftform.transform.position.y);

            gfx.GetComponent<SpriteRenderer>().sprite = _plaftformPalette.GetDisplaySprite(i, PlatformLength);
        }
        BoxCollider2D collider = _plaftform.GetComponent<BoxCollider2D>();
        collider.offset = GetCenter();
        collider.size = GetSize();
    }
    // Update is called once per frame
    void Update()
    {
        if (!_isMoving)
        {
            return;
        }

        if (Vector2.Distance(_plaftform.transform.position, _pathList[_nextDestination].position) < 0.02f)
        {
            _nextDestination = _isReversePath ? _nextDestination - 1 : _nextDestination + 1;
            if(_nextDestination == _pathList.Count)
            {
                switch (PathType)
                {
                    case MovingPlatformType.Simple: Stop(); break;
                    case MovingPlatformType.Looping: _nextDestination = 0; break;
                    case MovingPlatformType.Reverse: _isReversePath = true; _nextDestination = _pathList.Count - 2; break;
                }
            }

            if(_nextDestination == 0 && PathType == MovingPlatformType.Reverse)
            {
                _isReversePath = false;
            }
        }

        _plaftform.transform.position = Vector2.MoveTowards(_plaftform.transform.position, _pathList[_nextDestination].position, Speed * Time.deltaTime);
    }

    void OnDrawGizmos()
    {

        #if UNITY_EDITOR
        Gizmos.color = Color.green;
        Gizmos.DrawCube(GetCenter(transform), GetSize()); ;

        if (_pathContainer.transform.childCount > 0)
        {
            for(var i = 0; i < _pathContainer.transform.childCount; i++)
            {
                if(i != _pathContainer.transform.childCount-1)
                {
                    Gizmos.DrawLine(_pathContainer.transform.GetChild(i).transform.position, _pathContainer.transform.GetChild(i + 1).transform.position);
                }
            }
        }
        
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
