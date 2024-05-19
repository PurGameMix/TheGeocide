using Assets.Data.PlayerMouvement.Definition;
using System.Collections.Generic;
using UnityEngine;
internal enum CombatType
{
    Melee,
    Ranged
}
/// <summary>
/// Cette class est un gros workaround tentant de résoudre le problème suivant :
/// Problème: Lorsqu'une animation est cancel lors de la visée, les effets utilisés apparaissent dans d'autres animations
/// Solution: Quand on quitte le cast ou l'envoie d'un sort (Player_Ranged_Cast, Player_Ranged_Release etc...) on désactive l'image de cette effet
/// D'un point de vue performance les positions de l'objet seront toujours calculés
/// </summary>
public class PlayerAnimationEffectsCleaner : MonoBehaviour
{
    [SerializeField]
    private PlayerStateChannel _playerMvtChannel;

    [SerializeField]
    private Transform _rangedEffectsFolder;
    [SerializeField]
    private Transform _meleeEffectsFolder;

    private Dictionary<string, Sprite> _rangedEffectsSprites;
    private Dictionary<string, Sprite> _meleeEffectsSprites;

    void Awake()
    {
        _playerMvtChannel.OnMouvementStateEnter += OnMouvementStateEnter;
        _playerMvtChannel.OnMouvementStateExit += OnMouvementStateExit;
    }

    void OnDestroy()
    {
        _playerMvtChannel.OnMouvementStateEnter -= OnMouvementStateEnter;
        _playerMvtChannel.OnMouvementStateExit += OnMouvementStateExit;
    }

    private void OnMouvementStateEnter(PlayerStateEvent mvtEvt)
    {
        if (mvtEvt.Type.IsMeleeCombatState())
        {
            AddEffectsSprites(CombatType.Melee);
        }

        if (mvtEvt.Type.IsRangedCombatState())
        {
            AddEffectsSprites(CombatType.Ranged);
        }
    }

    private void OnMouvementStateExit(PlayerStateEvent mvtEvt)
    {

        if (mvtEvt.Type.IsMeleeCombatState())
        {
            CleanEffectsSprites(CombatType.Melee);
        }

        if (mvtEvt.Type.IsRangedCombatState())
        {
            //Debug.Log($"Effects cleans for {mvtEvt.Type}");
            CleanEffectsSprites(CombatType.Ranged);
        }
    }

    private void AddEffectsSprites(CombatType combatType)
    {
        var effectsFolder = GetEffectsGameObjects(combatType);
        for (int i = 0; i < effectsFolder.transform.childCount; i++)
        {

            GameObject go = effectsFolder.transform.GetChild(i).gameObject;
            //go.GetComponent<SpriteRenderer>().enabled = false;

            var sr = go.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                go.GetComponent<SpriteRenderer>().sprite = GetSprite(go.name);
            }
        }
    }

    private void CleanEffectsSprites(CombatType combatType)
    {
        var effectsFolder = GetEffectsGameObjects(combatType);

        for (int i = 0; i < effectsFolder.transform.childCount; i++)
        {

            GameObject go = effectsFolder.transform.GetChild(i).gameObject;
            //go.GetComponent<SpriteRenderer>().enabled = true;
            var sr = go.GetComponent<SpriteRenderer>();
            if(sr != null)
            {
                go.GetComponent<SpriteRenderer>().sprite = null;
            }        
        }
    }

    private Sprite GetSprite(string name)
    {
        if (_rangedEffectsSprites.ContainsKey(name))
        {
            return _rangedEffectsSprites[name];
        }

        return null;
    }

    private Transform GetEffectsGameObjects(CombatType combatType)
    {
        if(combatType == CombatType.Ranged)
        {
            _rangedEffectsSprites = GetSprites(_rangedEffectsFolder, _rangedEffectsSprites);
            return _rangedEffectsFolder;
        }

        _meleeEffectsSprites = GetSprites(_meleeEffectsFolder, _meleeEffectsSprites);
        return _meleeEffectsFolder;
    }

    private Dictionary<string, Sprite> GetSprites(Transform effectsFolder, Dictionary<string, Sprite> spriteDico)
    {

        if(spriteDico != null)
        {
            return spriteDico;
        }

        spriteDico = new Dictionary<string, Sprite>();
        for (int i = 0; i < effectsFolder.transform.childCount; i++)
        {

            GameObject go = effectsFolder.transform.GetChild(i).gameObject;

            var spriteRenderer = go.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteDico.Add(go.name, go.GetComponent<SpriteRenderer>().sprite); 
            }
        }

        return spriteDico;
    }
}
