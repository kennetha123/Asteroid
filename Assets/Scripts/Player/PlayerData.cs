using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public GameObject laser;
    public Transform firePoint;

    public int laserInstance;

    public float angularSpeed;
    public float thrust;
    public float maxSpeed;
    public float friction;
}
