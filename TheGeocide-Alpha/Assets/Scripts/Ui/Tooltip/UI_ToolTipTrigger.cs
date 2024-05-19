using Assets.Scripts.Localization;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ToolTipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private static LTDescr delay;
    public string content;
    public string header;
    public bool isLocalizationKey;
    public TradTable TradTable;

   private string _translatedContent;
   private string _translatedHeader;

    void Start()
    {
        if (isLocalizationKey)
        {
            _translatedContent = string.IsNullOrEmpty(header)? string.Empty: LocalizationService.GetLocalizedString(header, TradTable);
            _translatedHeader = string.IsNullOrEmpty(content) ? string.Empty : LocalizationService.GetLocalizedString(content, TradTable);
        }
    }

    private string GetHeader()
    {
        return isLocalizationKey ? _translatedHeader : header;
    }

    private string GetContent()
    {
        return isLocalizationKey ? _translatedContent : content;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        delay = LeanTween.delayedCall(0.2f, () => {
            UI_ToolTipSystem.Show(GetContent(), GetHeader());
            });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HideTooltip();
    }

    public void HideTooltip()
    {
        if(delay != null)
        {
            LeanTween.cancel(delay.id);
        }

        UI_ToolTipSystem.Hide();
    }
}
