using System;
using System.Collections.Generic;
using UnityEngine;

class PointInTime
{
    public Vector3 position_;
    public Quaternion rotation_;

    public PointInTime (Vector3 position, Quaternion rotation)
    {
        position_ = position;
        rotation_ = rotation;
    }
}
