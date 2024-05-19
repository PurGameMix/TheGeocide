using UnityEngine;

public class SecretWall : MonoBehaviour
{
    [SerializeField]
    private AudioController _audioController;

    [SerializeField]
    private GameObject _wall;

    public void Reveal()
    {
        _wall.SetActive(false);
        _audioController.Play("Enter");
        StartCoroutine(_audioController.FadeIn("Ambient",2));
    }

    public void Hide()
    {
        _wall.SetActive(true);
        _audioController.Play("Exit");
        _audioController.Stop("Ambient");
    }
}
