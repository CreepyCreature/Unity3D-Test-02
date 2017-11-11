using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") { return; }

        GetComponent<MeshRenderer>().enabled = false;
        PlayerResources.CollectCoin();
        Destroy(gameObject);
    }
}
