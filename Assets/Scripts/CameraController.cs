using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    GameObject player_;

    private Vector3 offset_;
    
	void Awake ()
    {
        offset_ = transform.position - player_.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate ()
    {
        transform.position = player_.transform.position + offset_;
	}
}
