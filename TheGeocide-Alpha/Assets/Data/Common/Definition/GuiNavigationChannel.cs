using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/GuiNavigationChannel")]
public class GuiNavigationChannel : ScriptableObject
{
    public delegate void MenuCallback(string viewName);
    
    public MenuCallback OnViewRequested;
    public void RaiseViewRequest(string viewName)
    {
        OnViewRequested?.Invoke(viewName);
    }
}