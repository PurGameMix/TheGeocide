using UnityEngine;

public class PortalAnimatorEventHandler : MonoBehaviour
{
    private IPortal _portalScript;

    // Start is called before the first frame update
    void Start()
    {
        _portalScript = GetComponentInParent<IPortal>();
    }

    //Called by animator
    public void OnPortalOpeningCompleted()
    {
        _portalScript.OnPortalOpeningCompleted();
    }
}
