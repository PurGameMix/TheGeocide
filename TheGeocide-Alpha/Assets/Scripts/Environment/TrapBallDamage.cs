using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBallDamage : MonoBehaviour
{

    private int _trapBallDamage = 100;
    void OnTriggerEnter2D(Collider2D Hit)
    {
        var canbeDamaged = Hit.GetComponent<ICanBeDamaged>();
        if (canbeDamaged != null)
        {
            canbeDamaged.TakeDamage(_trapBallDamage, HealthEffectorType.trap);
            return;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
