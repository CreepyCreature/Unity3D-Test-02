using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    public delegate void CheckpointEnter(Vector3 position);
    public static event CheckpointEnter OnCheckpointEnter;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            GameManager.Instance.SavePlayerPosition(other.transform.position);

            if (OnCheckpointEnter != null)
                OnCheckpointEnter(other.transform.position);
        }
    }
}
