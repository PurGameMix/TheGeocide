using Assets.Scripts.Utils;
using UnityEngine;

public class LedgeDetector : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private LayerMask edgeClimbLayer;
    [SerializeField] private PlayerStateChannel _psc;
    [SerializeField] private Transform  _playerTransform;
    private bool canDetect;
    private bool _isEdgeDetected;
    private bool _isReset;
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    private void Awake()
    {
        _isReset = true;
    }

    private void Update()
    {

        //Debug.Log($"LedgeDetection : {_isEdgeDetected}, canDetect : {canDetect}");

        if (!canDetect)
        {
            return;
        }
       var lastDetection = _isEdgeDetected;
        _isEdgeDetected = Physics2D.OverlapCircle(transform.position,radius, edgeClimbLayer);

        if(lastDetection != _isEdgeDetected)
        {
            _isReset = true;
        }

        if (_isEdgeDetected && _isReset)
        {
            _psc.RaiseEdgeDetection(new PlayerEdgeEvent(_playerTransform.position, true));
            _isReset = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (edgeClimbLayer.Contains(collision.gameObject.layer))
        {
            canDetect = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (edgeClimbLayer.Contains(collision.gameObject.layer))
        {
            canDetect = true;
        }
    }
}
