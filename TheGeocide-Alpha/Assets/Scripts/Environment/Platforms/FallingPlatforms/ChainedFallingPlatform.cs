using System.Collections;
using UnityEngine;

public class ChainedFallingPlatform : FallingPlatform
{
    [SerializeField]
    private SpriteRenderer _spriteGfx;

    private ChainedFallingPlatform _connectedPlatform;

    private float _connectBreakTime;

    public void SetSprite(Sprite sprite)
    {
        _spriteGfx.sprite = sprite;
    }

    public void SetConnection(ChainedFallingPlatform connectedPlatform, float connectBreakTime)
    {
        _connectedPlatform = connectedPlatform;
        _connectBreakTime = connectBreakTime;
    }

    private IEnumerator ConnectShaking()
    {
        yield return new WaitForSeconds(_connectBreakTime);
        _connectedPlatform.OnShakingTriggered();
    }

    public override void OnShakingTriggered()
    {
        base.OnShakingTriggered();

        if (_connectedPlatform != null && _connectedPlatform.isActiveAndEnabled)
        {
            StartCoroutine(ConnectShaking());
        }
    }
}
