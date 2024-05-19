using UnityEngine;


[CreateAssetMenu(menuName = "ScriptableObjects/EventChannel/PlayerSpellStateChannel")]
public class PlayerSpellStateChannel : ScriptableObject
{
    public delegate void PlayerCastSpellCallback(PlayerSpellStateEvent psse);
    public delegate void PlayerBoolActionCallback(bool isDashing);
    public delegate void PlayerMovementCallback();


    public PlayerCastSpellCallback OnSpellStateChanged;

    public void RaiseSpellChanged(PlayerSpellStateEvent psse)
    {
        OnSpellStateChanged?.Invoke(psse);
    }
}

