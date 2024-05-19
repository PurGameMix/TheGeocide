using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Boat : MonoBehaviour, ICanBeDamaged
{
    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private Interactable _SailInteractable;

    [SerializeField]
    private SpriteRenderer _SailUpSprite;

    [SerializeField]
    private SpriteRenderer _SailDownSprite;

    [SerializeField]
    private AudioController _audioController;

    [Range(0, .3f)]
    [SerializeField]
    private float m_MovementSmoothing = .05f;  // How much to smooth out the movement

    private bool isSailing = false;

    public float speed = 10f;

    private Vector2 m_Velocity = Vector3.zero;

    private bool _playerVisible = false;
    private Player _playerNear;

    private static string _sailDownTradKey = "GUI_INTERACT_BOAT_DOWN";
    private static string _sailUpTradKey = "GUI_INTERACT_BOAT_UP";

    private UnityEvent _sailDownEvent;
    private UnityEvent _sailUpEvent;
    void Awake()
    {
        _SailDownSprite.enabled = false;
        _sailDownEvent = new UnityEvent();
        _sailUpEvent = new UnityEvent();
        _sailDownEvent.AddListener(HandleSailDown);
        _sailUpEvent.AddListener(HandleSailUp);
    }

    //Called be Interaction component
    public void HandleSailUp()
    {
        Debug.Log("SailUp!");


        _SailUpSprite.enabled = true;
        _SailDownSprite.enabled = false;

        isSailing = false;

        _SailInteractable.SetInteraction(_sailDownEvent, _sailDownTradKey);
    }

    //Called be Interaction component
    public void HandleSailDown()
    {
        Debug.Log("SailDown!");

        _SailUpSprite.enabled = false;
        _SailDownSprite.enabled = true;

        isSailing = true;

        _SailInteractable.SetInteraction(_sailUpEvent, _sailUpTradKey);
    }

    private void FixedUpdate()
    {
        if (isSailing)
        {
            Vector2 force = Vector2.right * speed * Time.deltaTime;
            Vector2 targetVelocity = new Vector2(force.x * 10f, _rb.velocity.y);
            // And then smoothing it out and applying it to the character
            _rb.AddForce(Vector2.SmoothDamp(_rb.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing));
        }

        //Debug.Log($"PlayerVisible {_playerVisible} && PlayerNEar {_playerNear != null}");
    }

    public void TakeDamage(int damage, HealthEffectorType type)
    {
        //Debug.Log("Play JungleBlocHit");
        _audioController.Play("Hit");
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        var entity = other.GetComponent<Player>();
        if (entity!= null)
        {
            if (_playerVisible && !_playerNear)
            {
                _playerNear = entity;
            }
            else
            {
                _playerVisible = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Player>())
        {
            if (_playerVisible && _playerNear)
            {
                _playerNear = null;
            }
            else
            {
                _playerVisible = false;
            }
        }
    }
}
