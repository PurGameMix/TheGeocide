using UnityEngine;

public class UI_ButtonNavigation : MonoBehaviour
{
    [SerializeField]
    private GuiNavigationChannel _navchannel;
    [SerializeField]
    private GameObject _viewToNavigate;

    public void OnClick() {
        _navchannel.RaiseViewRequest(_viewToNavigate.name);
    }
}
