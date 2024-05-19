using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElecUltimate : InstantSpell
{

    public List<ElecBall> BlitzBalls;

    // Start is called before the first frame update
    void Start()
    {

        var ballsDamage = _damage / 2;
        foreach (var ball in BlitzBalls)
        {
            ball.Init(ballsDamage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(BlitzBalls.Count == 0)
        {
            Destroy(gameObject);
        }
    }

    public override void DestroySpell()
    {
        //Destroy(gameObject);
    }
}
