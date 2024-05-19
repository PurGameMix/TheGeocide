using Assets.Data.Items.Definition;
using TMPro;
using UnityEngine;

public class UI_ItemInputBindingDisplayer : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _tmp;

    [SerializeField]
    private ItemType _itemType;

    // Start is called before the first frame update
    void Start()
    {
        _tmp.text = InputHandler.GetBindingDisplayString(_itemType.GetInputActionName());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
