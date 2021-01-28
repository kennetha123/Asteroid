using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Controller
{
    public Animator anim { get { return gameObject.GetComponent<Animator>(); } }
    public float defaultDrag;
    public PlayerData data;

    public Player(GameManager manager, GameObject gameObject, PlayerData data) : base(manager, gameObject)
    {
        this.data = data;

        // get default drag value
        defaultDrag = rigidBody.drag;

        for (int i = 0; i < data.laserInstance; i++)
        {
            GameObject laser = CreateLaser();
            laser.SetActive(false);

            availableLaser.Add(laser);
        }
    }

    public GameObject SpawnLaser()
    {
        int index = Random.Range(0, availableLaser.Count);
        PlayerLaser laser = availableLaser[index].GetComponent<PlayerLaser>();
        laser.Activate(data.firePoint, data.firePoint.position - transform.position);

        availableLaser.RemoveAt(index);

        return laser.gameObject;
    }

    public GameObject CreateLaser()
    {
        PlayerLaser laser = GameObject.Instantiate(data.laser).GetComponent<PlayerLaser>();
        laser.Init(this, manager);

        availableLaser.Add(laser.gameObject);

        return laser.gameObject;
    }

}