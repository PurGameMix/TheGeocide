using UnityEngine;

public class TempleColumn : MonoBehaviour
{
    public int Speed = 2;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private AudioController  _audioController;
    [SerializeField]
    private Transform _closePoint;
    [SerializeField]
    private Transform _openPoint;
    [SerializeField]
    private BoxCollider2D boxCollider;

    private bool _isClosing = false;
    private bool _isOpening = false;
    private float _pillarSizeOffest;
    private Vector3 _relativeTop; 
    // Start is called before the first frame update
    void Start()
    {
        _pillarSizeOffest = boxCollider.bounds.size.y / 2;

        _relativeTop = transform.TransformDirection(Vector2.up);
    }

    // Update is called once per frame
    void Update()
    {
        if (_isClosing)
        {
            if(transform.position.y+ _pillarSizeOffest >= _closePoint.position.y)
            {
                StopCloseDoor();
                return;
            }
            transform.position = transform.position + _relativeTop * Time.deltaTime * Speed;
        }

        if (_isOpening)
        {
            if (transform.position.y + _pillarSizeOffest <= _openPoint.position.y)
            {
                StopOpenDoor();
                return;
            }
            transform.position = transform.position - _relativeTop * Time.deltaTime * Speed;
        }
    }

    public void CloseDoor()
    {
        _isClosing = true;
        _isOpening = false;
        _animator.Play("Pillar_Shaking");
        _audioController.Play("Pillar_Move");
    }

    private void StopCloseDoor()
    {
        _isClosing = false;
        _animator.Play("Pillar_Idle");
        _audioController.Stop("Pillar_Move");
        _audioController.Play("Pillar_Stop");

    }

    public void OpenDoor()
    {
        _isOpening = true;
        _isClosing = false;
        _animator.Play("Pillar_Shaking");
        _audioController.Play("Pillar_Move");
    }

    private void StopOpenDoor()
    {
        _isOpening = false;
        _animator.Play("Pillar_Idle");
        _audioController.Stop("Pillar_Move");
        _audioController.Play("Pillar_Stop");

    }
}
