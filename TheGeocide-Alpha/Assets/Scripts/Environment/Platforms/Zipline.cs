using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zipline : MonoBehaviour
{

    [SerializeField]
    private Transform StartPoint;

    [SerializeField]
    private Transform EndPoint;

    [SerializeField]
    private SpriteRenderer _zipSprite;

    [SerializeField]
    private Rigidbody2D _localZip;

    [SerializeField]
    private AudioController _audioController;


    public float zipSpeed;

    private float zipScale = 0.2f;
    private float _arrivalThreshold = 1f;
    private Vector2 _zipVector;
    private bool _isZipping;
    private Vector2 _playerAnchor;
    private float playerGravityScale = 1;
    private float _zipHeight;
    private float _zipMinHeight = 1;
    // Start is called before the first frame update
    void Start()
    {
        _zipHeight = StartPoint.position.y - EndPoint.position.y;
        if (_zipHeight <= _zipMinHeight)
        {
            Debug.LogError($"{GetType().FullName} :  Zipline dont have enough height for zipline {name}");
            return;
        }

        _zipVector = (EndPoint.position - StartPoint.position).normalized;
        StretchLine();
    }

    private float GetZiplineAngle(Vector2 origin, Vector2 target)
    {
        return VectorUtils.GetAngle(origin, target);
    }

    // Update is called once per frame
    void Update()
    {

        if (!_isZipping || _localZip == null)
        {
            return;
        }

        _localZip.GetComponent<Rigidbody2D>().AddForce(_zipVector * zipSpeed * Time.deltaTime * _zipHeight * 10);

        if(NotEnoughLine())
        {
            EndZipline();
        }
    }

    private bool NotEnoughLine()
    {
        return Vector2.Distance(_localZip.position, EndPoint.position) <= _arrivalThreshold;
    }

    internal bool IsSlidingRight()
    {
        return StartPoint.position.x < EndPoint.position.x;
    }

    public void StartZipline(PlayerStateMachine player)
    {

        if (_isZipping || NotEnoughLine())
        {
            return;
        }

        _localZip.transform.position = _playerAnchor;



        var playerRb = player.GetComponent<Rigidbody2D>();
        _localZip.velocity = Vector2.zero;

        //reset player mobility
        playerGravityScale = playerRb.gravityScale;
        playerRb.gravityScale = 0;
        playerRb.isKinematic = true;
        playerRb.velocity = Vector2.zero;

        //Handle player direction
        player.CheckDirectionToFace(IsSlidingRight());
        var currentZipCheckPoint = player.GetZipCheckPoint();
        if (_playerAnchor != currentZipCheckPoint)
        {
            var diff = _playerAnchor - currentZipCheckPoint;
            player.transform.position = (Vector2)player.transform.position + diff;
        }

        //Attach
        player.transform.parent = _localZip.transform;

        _isZipping = true;
        _audioController.Play("Zipline");
    }

    internal bool ZipOver()
    {
        return !_isZipping;
    }

    public void EndZipline()
    {

        if (ZipOver())
        {
            return;
        }

        GameObject player = _localZip.transform.GetChild(0).gameObject;
        var playerRb = player.GetComponent<Rigidbody2D>();
        playerRb.gravityScale = playerGravityScale;
        playerRb.isKinematic = false;
        //playerRb.velocity = Vector2.zero;
        player.transform.parent = null;

        _isZipping = false;
        _audioController.Stop("Zipline");
    }

    internal void SetAnchor(Vector2 position)
    {
        _playerAnchor = position;
    }

    private void StretchLine()
    {
        //position zip between 2 point
        _zipSprite.transform.position = (StartPoint.position + EndPoint.position) / 2f;

        var zipSize = new Vector2(_zipSprite.size.x, Vector2.Distance(StartPoint.position, EndPoint.position) / zipScale);
        //Stretching zip size
        _zipSprite.size = zipSize;

        //Setting angle of the zipline from vertical to horitontal - angle
        var angle = GetZiplineAngle(StartPoint.position, EndPoint.position);
        _zipSprite.transform.Rotate(0, 0, 90 - angle);

        _zipSprite.GetComponent<BoxCollider2D>().size = zipSize;
    }
}
