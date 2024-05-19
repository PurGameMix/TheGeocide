using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopePlatform : MonoBehaviour
{
    [Header("Rope")]
    public Rope RopePrefab;
    public RopeLink LinkPrefab;
    public int NbLinks;
    [SerializeField]
    private GameObject _ropeContainer;

    [Header("Carabiner")]
    [SerializeField]
    private GameObject LeftCarabiner;

    [SerializeField]
    private GameObject RightCarabiner;

    [Header("Platform")]
    public int PlatformLength;
    [SerializeField]
    private PlatfromPaletteSO _plaftformPalette;
    [SerializeField]
    private MovingRopePlateform _plaftformContainer;
    [SerializeField]
    private GameObject _plaftformGfxPrefab;

    private static float _offsetFromOrigin = 0.5f;
    private static float _tileHeight = 0.5f;
    private static float _tileWidth = 1f;

    private Rope _leftRope;
    private Rope _rightRope;
    // Start is called before the first frame update
    void Start()
    {

        //Setup ropes
        var ropeLinks = _InitRopes();

        //Setup carabiners
        _InitCarabiners(ropeLinks.Item1, ropeLinks.Item2);

        //Setup hanging platform
        _InitHangingPlateform();
    }

    private Tuple<Rigidbody2D, Rigidbody2D> _InitRopes()
    {
        //Generate left rope
        _leftRope = Instantiate(RopePrefab, _ropeContainer.transform);
        var leftLink = _leftRope.GenerateRope(LinkPrefab, NbLinks);

        //Generate right rope and set position
        _rightRope = Instantiate(RopePrefab, _ropeContainer.transform);
        var rightX = transform.position.x + PlatformLength * _tileWidth;
        _rightRope.transform.position = new Vector2(rightX, transform.position.y);
        var rightLink = _rightRope.GenerateRope(LinkPrefab, NbLinks);

        //Return last link of each rope
        return new Tuple<Rigidbody2D, Rigidbody2D>(leftLink, rightLink);
    }

    private void _InitCarabiners(Rigidbody2D leftLink, Rigidbody2D rightLink)
    {
        //Setup both links of left carabiner
        var leftRopeJoint = LeftCarabiner.GetComponent<HingeJoint2D>();
        leftRopeJoint.connectedBody = leftLink;
        leftRopeJoint.connectedAnchor = new Vector2(0, -_tileHeight);

        var leftPlatformJoint = LeftCarabiner.GetComponent<DistanceJoint2D>();
        leftPlatformJoint.connectedAnchor = new Vector2(0, _tileHeight);

        //Setup both links of right carabiner
        var rightRopeJoint = RightCarabiner.GetComponent<HingeJoint2D>();
        rightRopeJoint.connectedBody = rightLink;
        rightRopeJoint.connectedAnchor = new Vector2(0, -_tileHeight);

        var rightPlatformJoint = RightCarabiner.GetComponent<DistanceJoint2D>();
        rightPlatformJoint.connectedAnchor = new Vector2(PlatformLength * _tileWidth, _tileHeight);
    }

    private void _InitHangingPlateform()
    {
        //Set platform position
        var oneLinkHeight = LinkPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        var bottomY = transform.position.y - NbLinks * oneLinkHeight;
        _plaftformContainer.transform.position = new Vector2(transform.position.x, bottomY);

        //Setup platform tile
        for (var i = PlatformLength - 1; i >= 0; i--)
        {
            var gfx = Instantiate(_plaftformGfxPrefab, _plaftformContainer.transform);
            gfx.transform.position = new Vector2(_plaftformContainer.transform.position.x + i + _offsetFromOrigin, _plaftformContainer.transform.position.y);

            gfx.GetComponent<SpriteRenderer>().sprite = _plaftformPalette.GetDisplaySprite(i, PlatformLength);
        }
        BoxCollider2D collider = _plaftformContainer.GetComponent<BoxCollider2D>();
        collider.offset = GetCenter(Vector2.zero);
        collider.size = GetSize();
    }

    // Update is called once per frame
    void Update()
    {
        if (_plaftformContainer.IsBreak)
        {
            return;
        }

        if(_rightRope == null || _leftRope == null)
        {
            return;
        }

        _plaftformContainer.IsBreak = _rightRope.IsBreak || _leftRope.IsBreak;
    }


    private Vector3 GetCenter(Vector3 origin)
    {
        if (origin != Vector3.zero)
        {
            return new Vector3(origin.x + PlatformLength * _tileWidth / 2, origin.y + _tileHeight / 2);
        }

        return new Vector3(PlatformLength * _tileWidth / 2, _tileHeight / 2);
    }

    private Vector3 GetSize()
    {
        return new Vector3(PlatformLength, _tileHeight);
    }

    void OnDrawGizmos()
    {

        #if UNITY_EDITOR
                Gizmos.color = Color.yellow;
        var rightX = transform.position.x + PlatformLength * _tileWidth;
        var oneLinkHeight = LinkPrefab.GetComponent<SpriteRenderer>().bounds.size.y;
        var bottomY = transform.position.y - NbLinks * oneLinkHeight;
        var leftCorner = new Vector2(transform.position.x, bottomY);
        //LeftRope
        Gizmos.DrawLine(transform.position, leftCorner);

        //RightRope
        Gizmos.DrawLine(new Vector2(rightX, transform.position.y), new Vector2(rightX, bottomY));

        //Plateform
        Gizmos.DrawCube(GetCenter(leftCorner), GetSize()); ;
#endif
    }
}
