using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLightController : MonoBehaviour {

    void OnTriggerEnter2D (Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EventManager.TriggerEvent("EnteredLight");
        }
    }

    void OnTriggerExit2D (Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            EventManager.TriggerEvent("LeftLight");
        }
    }
}
