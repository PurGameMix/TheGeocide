using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

//Todo use Sprite resolver
public class PlayerEquipmentSkinner : MonoBehaviour
{
    [SerializeField]
    private PlayerInventoryChannel _playerInventoryChannel;

    [SerializeField]
    private GameObject _playerGfx;

    private Dictionary<string, SpriteResolver> _skeletonMap = new Dictionary<string, SpriteResolver>();

    void Awake()
    {
        var rootElement = _playerGfx.transform.Find("Iskaa");

        if(rootElement == null)
        {
            Debug.LogError($"{GetType().FullName}: Probl�me getting sprite resolvers root element");
        }

        var resolvers = rootElement.GetComponentsInChildren<SpriteResolver>();
        if(resolvers.Length == 0)
        {
            return;
        }


        _skeletonMap.Add("Torso", rootElement.transform.Find("Iskaa torso").GetComponent<SpriteResolver>());
        _skeletonMap.Add("Head", rootElement.transform.Find("Iskaa head").GetComponent<SpriteResolver>());
        _skeletonMap.Add("BackArm", rootElement.transform.Find("Iskaa back arm").GetComponent<SpriteResolver>());
        _skeletonMap.Add("FrontArm", rootElement.transform.Find("Iskaa front arm").GetComponent<SpriteResolver>());
        _skeletonMap.Add("BackLeg", rootElement.transform.Find("Iskaa back leg").GetComponent<SpriteResolver>());
        _skeletonMap.Add("FrontLeg", rootElement.transform.Find("Iskaa front leg").GetComponent<SpriteResolver>());

        _playerInventoryChannel.OnInventoryChanged += OnInventoryChanged;
    }

    private void OnDestroy()
    {
        _playerInventoryChannel.OnInventoryChanged -= OnInventoryChanged;
    }

    private void OnInventoryChanged(PlayerItemSO so)
    {

        if(_skeletonMap.Count > 0)
        {
            Debug.Log($"{GetType().FullName}: No resolver found on player skin");
            return;
        }

        var changes = so.GetPlayerBonesChanges();

        foreach(var change in changes)
        {
            if (!_skeletonMap.ContainsKey(change))
            {
                Debug.Log($"{GetType().FullName}: Change {change} is not found in skeleton resolver");
                continue;
            }

            var resolver = _skeletonMap[change];
            resolver.SetCategoryAndLabel(change, so.GetName());
        }
    }
}
