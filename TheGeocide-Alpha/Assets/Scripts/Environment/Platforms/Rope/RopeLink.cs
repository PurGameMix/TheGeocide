using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeLink : MonoBehaviour, ICanBeDamaged
{

    public SpriteRenderer spriteComponent;

    [SerializeField]
    public List<Sprite> spriteList;

    [SerializeField]
    private AudioChannel _ropeAudioChannel;

    private int Health = 100;

    private string AudioManagerId;

    internal void SetAudioManagerId(string parentId)
    {
        AudioManagerId = parentId;
    }

    public void TakeDamage(int damage, HealthEffectorType type)
    {
        if(Health <= 0)
        {
            return;
        }

        Health -= damage;
        if(Health > 0)
        {
            _ropeAudioChannel.RaiseAudioRequest(new AudioEvent("Rope_Squeak", AudioManagerId));
            return;
        }

        _ropeAudioChannel.RaiseAudioRequest(new AudioEvent("Rope_Break", AudioManagerId));
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        var rng = UnityEngine.Random.Range(0, spriteList.Count * 2);

        var index = rng >= spriteList.Count -1? spriteList.Count - 1 : rng;
        spriteComponent.sprite = spriteList[index];
    }

    private void Update()
    {
        if(Health <= 0)
        {
            _ropeAudioChannel.RaiseAudioRequest(new AudioEvent("Rope_Break"));

        }
    }
}
