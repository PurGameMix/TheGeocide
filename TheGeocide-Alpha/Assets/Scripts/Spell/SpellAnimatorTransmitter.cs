using UnityEngine;

public class SpellAnimatorTransmitter : MonoBehaviour
{

    [SerializeField]
    private InstantSpell _spell;

    public void SpellCompleted()
    {
        if (_spell == null)
        {
            Debug.LogWarning($"No effect attached {transform.parent.gameObject.name}");
            return;
        }

        _spell.DestroySpell();
    }
}
