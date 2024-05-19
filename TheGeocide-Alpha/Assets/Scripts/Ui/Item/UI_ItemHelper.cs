using UnityEngine;
using UnityEngine.UI;

public class UI_ItemHelper
{
    public GameObject _this;
    public Image ImageComponent;
    public UI_ToolTipTrigger TooltipTrigger;
    public GameObject Slot;

    public UI_ItemHelper()
    {

    }

    public UI_ItemHelper(GameObject itemHelper)
    {
        _this = itemHelper;
        Slot = _this.transform.Find("ItemSlot").gameObject;
        //Debug.Log($"Slot : {Slot}");
        ImageComponent = Slot.transform.GetChild(0).gameObject.GetComponent<Image>();
        //Debug.Log($"_imageComponent : {ImageComponent}");
        TooltipTrigger = Slot.GetComponent<UI_ToolTipTrigger>();
    }

    internal void SetHelperData(PlayerItemSO data)
    {
        ImageComponent.enabled = true;
        ImageComponent.sprite = data.GetSetSprite();

        TooltipTrigger.header = data.GetDetailHeader();
        TooltipTrigger.content = data.GetDetailContent();
        TooltipTrigger.enabled = true;
    }
}
