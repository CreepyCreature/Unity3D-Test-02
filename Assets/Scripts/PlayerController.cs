using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Range(1f, 100f)]
    public float speed_;

    public GameObject follower_;

    [Range(0f, 20f)]
    public float follower_distance_ = 2.0f;
    
    private Rigidbody rigidbody_;

    // Use this for initialization
    void Start ()
    {
        rigidbody_ = GetComponent<Rigidbody>();
	}
		
	void FixedUpdate ()
    {
        float horizontal_axis = Input.GetAxis("Horizontal");
        float vertical_axis = Input.GetAxis("Vertical");

        Vector3 move_force = new Vector3(horizontal_axis, 0, vertical_axis);

        rigidbody_.AddForce(move_force * speed_ * Time.deltaTime * 100);

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Instantiate(follower_,
                transform.position + new Vector3(0, 0, follower_distance_), 
                new Quaternion(0, 0, 0, 0)
            );
        }
	}

    private void OnTriggerEnter (Collider other)
    {
        if (other.CompareTag("PickUp"))
        {
            PlayerResources.CollectCoin();
            //other.gameObject.SetActive(false);
            Destroy(other.gameObject);
            AudioManager.Instance.PlaySound("PickUp");
        }
    }

    private void OnCollisionEnter (Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }

        if (collision.relativeVelocity.magnitude > 100f)
            AudioManager.Instance.PlaySound("Crash");
    }
}
