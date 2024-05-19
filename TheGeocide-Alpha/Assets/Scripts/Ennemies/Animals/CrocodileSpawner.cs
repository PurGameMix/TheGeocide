using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrocodileSpawner : MonoBehaviour
{
    [SerializeField]
    private Collider2D _spawnArea;

    [SerializeField]
    private Transform _limitInferior;

    [SerializeField]
    private Transform _limitSuperior;

    [SerializeField]
    private Crocodile _crocodilePrefab;

    [SerializeField]
    private List<Transform> _forbiddenSpawn;

    private Crocodile _currentCrocodile;
    private float _verticalSpawnDistance = -1f;
    private float _horizontalSpawnDistance = 9f;
    private float _threshold = 1;

    private float respawnCd = 3f;
    private float _lastPlayerTime = 0;
    private bool _isPlayerOut;
    // Start is called before the first frame update
    void Start()
    {
        _lastPlayerTime = respawnCd;
        _isPlayerOut = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlayerOut)
        {
            _lastPlayerTime += Time.deltaTime;
        }
        else
        {
            _lastPlayerTime = 0;
        }

        if (_currentCrocodile != null && _lastPlayerTime > respawnCd)
        {
            _currentCrocodile.Despawn();
            _currentCrocodile = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var detectable = other.GetComponent<Player>();
    
        if (detectable != null)
        {         
            _isPlayerOut = false;

            if (_currentCrocodile == null)
            {
                _currentCrocodile = Instantiate(_crocodilePrefab, transform);
            }

            if(_lastPlayerTime > respawnCd)
            {
                var spawnPoint = GetSpawnPoint(detectable.transform.position);
                _currentCrocodile.gameObject.transform.position = spawnPoint;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        var detectable = other.GetComponent<Player>();
        if (detectable != null)
        {
            _isPlayerOut = true;

            if (_currentCrocodile != null && _lastPlayerTime > respawnCd)
            {
                _currentCrocodile.Despawn();
                _currentCrocodile = null;
            }
        }
    }


    private Vector2 GetSpawnPoint(Vector2 playerPos)
    {
        float wantedPos;
        if (playerPos.x < _limitInferior.position.x)
        {
            wantedPos = playerPos.x + _horizontalSpawnDistance;
        }
        else if (playerPos.x > _limitInferior.position.x)
        {
            wantedPos = playerPos.x - _horizontalSpawnDistance;
        }
        else
        {
            var rng = Random.Range(1, 3);
            var sign = rng == 2 ? -1 : 1;
            wantedPos = (playerPos.x + _horizontalSpawnDistance) * sign;
        }

        foreach (var obstacle in _forbiddenSpawn)
        {
            if(obstacle == null || obstacle.position == null)
            {
                continue;
            }

            if (obstacle.position.x - _threshold <= wantedPos && wantedPos <= obstacle.position.x + _threshold)
            {
                wantedPos += _threshold * 2;
            }
        }

        return new Vector2(wantedPos, playerPos.y + _verticalSpawnDistance);
    }
}
