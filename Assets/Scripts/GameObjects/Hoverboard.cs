using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hoverboard : MonoBehaviour
{
    [Range(0, 50)]public float hoverforce_ = 2.0f;

    private void OnTriggerStay(Collider other)
    {
        if (other.attachedRigidbody != null)
        {
            other.attachedRigidbody.AddForce(Vector3.up * hoverforce_, ForceMode.Acceleration);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        AudioManager.Instance.PlaySound("Hovering");
    }

    private void OnTriggerExit(Collider other)
    {
        //AudioManager.instance.StopSound("Hovering");
    }
}
