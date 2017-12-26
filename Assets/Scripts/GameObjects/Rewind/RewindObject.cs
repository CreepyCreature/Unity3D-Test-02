using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindObject : MonoBehaviour
{
    [Range(0, 60)]
    public float max_recorded_time = 5f;

    private bool is_rewinding_ = false;
    private List<PointInTime> points_in_time_;

    private Rigidbody rigidbody_;

	// Use this for initialization
	void Start ()
    {
        points_in_time_ = new List<PointInTime>();
        rigidbody_ = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
            EngageRewind();
        if (Input.GetKeyUp(KeyCode.KeypadEnter))
            DisengageRewind();
	}

    void FixedUpdate ()
    {
        if (is_rewinding_)
            Rewind();
        else
            Record();
    }

    void EngageRewind ()
    {
        is_rewinding_ = true;
        rigidbody_.isKinematic = true;
        AudioManager.Instance.PlaySound("Rewinding");
    }

    void DisengageRewind ()
    {
        is_rewinding_ = false;
        rigidbody_.isKinematic = false;
        AudioManager.Instance.StopSound("Rewinding");
    }

    void Rewind()
    {
        if (points_in_time_.Count > 0)
        {
            transform.position = points_in_time_[0].position_;
            transform.rotation = points_in_time_[0].rotation_;
            points_in_time_.RemoveAt(0);
        }
        else
            DisengageRewind();
    }

    void Record ()
    {
        points_in_time_.Insert(0, new PointInTime(transform.position, transform.rotation));

        if (points_in_time_.Count > Mathf.RoundToInt(max_recorded_time * 1f / Time.fixedDeltaTime))
            points_in_time_.RemoveAt(points_in_time_.Count - 1);
    }
}
