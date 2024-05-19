using UnityEngine;

[ExecuteInEditMode]
public class LightColorController : MonoBehaviour
{
    [SerializeField] [Range(0,1)] float time;

    private IColorSetter[] setters;
    private float currentTime = 0;

    public float timeValue => currentTime;

    public void GetSetters()
    {
        setters = GetComponentsInChildren<IColorSetter>();
        foreach (var setter in setters)
            setter.Refresh();
    }

    private void OnEnable()
    {
        GetSetters();
        UpdateSetters();
    }

    private void OnDisable()
    {
        UpdateSetters();
    }

    private void Update()
    {
        if (currentTime != time)
            UpdateSetters();
    }

    public void UpdateSetters()
    {
        currentTime = time;

        foreach (var setter in setters)
            setter.SetColor(time);
    }
}
