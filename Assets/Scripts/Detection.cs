using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detection : MonoBehaviour
{
    public Image crosshair;
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.attachedRigidbody)
            crosshair.color = Color.yellow;
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Player" && other.attachedRigidbody)
            crosshair.color = Color.white;
    }
}
