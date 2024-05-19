using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// allows to share ids especially for agents cummucating through sdo channels
/// </summary>
public class EntityInfo : MonoBehaviour
{

    internal string EntityName;
    // Start is called before the first frame update
    void Awake()
    {
        EntityName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
