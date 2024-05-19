using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDot : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D Hit)
    {
        var player = Hit.GetComponent<Player>();
        if (player != null)
        {
            player.Drown(true);
        }
    }

    void OnTriggerExit2D(Collider2D Hit)
    {
        var player = Hit.GetComponent<Player>();
        if (player != null)
        {
            player.Drown(false);
        }
    }
}
