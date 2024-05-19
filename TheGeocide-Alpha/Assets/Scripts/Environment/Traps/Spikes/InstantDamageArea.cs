using UnityEngine;

public class InstantDamageArea : MonoBehaviour
{

    [SerializeField]
    private AudioSource _audioSource;
    public int Damage = 1000;

    void OnTriggerEnter2D(Collider2D Hit)
    {
        var player = Hit.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(Damage, HealthEffectorType.trap);
            if(_audioSource != null)
            {
                _audioSource.Play();
            }
        }
    }
}
