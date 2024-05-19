using System.Collections.Generic;
using UnityEngine;

public class EffectsController : MonoBehaviour
{
    private Dictionary<string, Effect> _effectDico = new Dictionary<string, Effect>();

    private Dictionary<string, EntityFX> _effectInstancesDico = new Dictionary<string, EntityFX>();
    public Effect[] effects;



    // Start is called before the first frame update
    void Awake()
    {

        foreach (var effect in effects)
        {
            if (_effectDico.ContainsKey(effect.Name))
            {
                Debug.LogError($"{gameObject.name}: Audio clip with name '{effect.Name}' already exist");
                continue;
            }

            _effectDico.Add(effect.Name, effect);
        }
    }

    public EntityFX Play(string effectName,bool isLookingLeft, bool noValidation = false)
    {
        if (!IsValid(effectName, noValidation))
        {
            return null;
        }
        
        //Debug.Log($"Playing {effectName}");
        var go = InstantiateEffect(_effectDico[effectName]);
        StoreInstance(effectName,go);

        return go;
    }

    private void StoreInstance(string effectName, EntityFX go)
    {

        //Fo now only 1 instance. Todo : need more?
        if (_effectInstancesDico.ContainsKey(effectName))
        {
            _effectInstancesDico[effectName] = go;
            return;
        }

        _effectInstancesDico.Add(effectName, go);
    }

    public EntityFX InstantiateEffect(Effect effect)
    {

        if (effect.IsFollowingOrigin)
        {
            return Instantiate(effect.Prefab, effect.OriginPosition);
        }
        

        return Instantiate(effect.Prefab, effect.OriginPosition.position, effect.OriginPosition.rotation);
    }

    private bool IsValid(string effectName, bool noValidation = false)
    {
        if (!_effectDico.ContainsKey(effectName))
        {
            if (!noValidation)
            {
                Debug.LogWarning($"{gameObject.name}: Effect with name '{effectName}' not found");
            }

            return false;
        }

        return true;
    }
}
