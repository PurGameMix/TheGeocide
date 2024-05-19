using UnityEngine;
using System.Collections;
using Assets.Scripts.Utils;
using System.Collections.Generic;

public class WaterDetector : MonoBehaviour
{

    public LayerMask CanTriggerLayer;

    internal int index;
    internal WaterPhysics parent;

    void OnTriggerEnter2D(Collider2D Hit)
    {
        if (Hit.isTrigger)
        {
            return;
        }

        if (!CanTriggerLayer.Contains(Hit.gameObject.layer))
        {
            return;
        }

        parent.RegisterObjectInWater(index, Hit.gameObject.name);

        if (parent.IsObjectAlreadyInWater(index, Hit.gameObject.name))
        {
            return;
        }

        var rb2D = Hit.GetComponent<Rigidbody2D>();
        if (rb2D != null)
        {
            parent.Splash(transform.position.x, rb2D.velocity.y * rb2D.mass / 40f);
        }
    }

    void OnTriggerExit2D(Collider2D Hit)
    {
        parent.UnRegisterObjectInWater(index, Hit.gameObject.name);
    }



    /*void OnTriggerStay2D(Collider2D Hit)
    {
        //print(Hit.name);
        if (Hit.rigidbody2D != null)
        {
            int points = Mathf.RoundToInt(Hit.transform.localScale.x * 15f);
            for (int i = 0; i < points; i++)
            {
                transform.parent.GetComponent<Water>().Splish(Hit.transform.position.x - Hit.transform.localScale.x + i * 2 * Hit.transform.localScale.x / points, Hit.rigidbody2D.mass * Hit.rigidbody2D.velocity.x / 10f / points * 2f);
            }
        }
    }*/

}