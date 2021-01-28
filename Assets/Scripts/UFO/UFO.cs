using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UFOData
{
    public GameObject laser;

    public int laserInstance;

    public float speed;
}

public class UFO : Controller
{
    public UFOData data;

    public Vector3 target;
    public Vector3 direction;

    public UFO(GameManager manager, GameObject gameObject, Vector3 target, UFOData data) : base(manager, gameObject)
    {
        this.target = target;
        this.data = data;

        for (int i = 0; i < data.laserInstance; i++)
        {
            GameObject laser = CreateLaser();
            laser.SetActive(false);

            availableLaser.Add(laser);
        }

        SetDirection();
    }

    public void SetDirection()
    {
        direction = target - transform.position;
    }

    public GameObject CreateLaser()
    {
        UFOLaser laser = GameObject.Instantiate(data.laser).GetComponent<UFOLaser>();
        laser.Init(this, manager);

        availableLaser.Add(laser.gameObject);

        return laser.gameObject;
    }

}

public class SmallUFO : UFO
{
    private float fireRadius;
    private Transform player;

    public SmallUFO(GameManager manager, GameObject gameObject, Vector3 target, UFOData data, Transform player, float fireRadius) : base(manager, gameObject, target, data)
    {
        this.player = player;
        this.fireRadius = fireRadius;

        SetTarget();
    }

    public void SpawnLaser()
    {
        Vector2 target = Random.insideUnitSphere * fireRadius + player.position - transform.position;
        int index = Random.Range(0, availableLaser.Count);
        availableLaser[index].GetComponent<UFOLaser>().Activate(transform.position, target);
        availableLaser.RemoveAt(index);
    }

    public void SetTarget()
    {
        target = Random.insideUnitSphere * 3 + player.position;

        SetDirection();
    }
}

public class BigUFO : UFO
{
    private float radius;

    public BigUFO(GameManager manager, GameObject gameObject, Vector3 target, UFOData data, float radius) : base(manager, gameObject, target, data)
    {
        this.radius = radius;
    }

    public GameObject SpawnLaser()
    {
        int random = Random.Range(0, availableLaser.Count);
        Vector3 dir = Random.insideUnitCircle;
        UFOLaser laser = availableLaser[random].GetComponent<UFOLaser>();

        laser.Activate(transform.position, dir);
        availableLaser.RemoveAt(random);

        return laser.gameObject;
    }

    public void SetTarget()
    {
        target = Random.insideUnitSphere * radius;
        target = new Vector3(
            target.x + Random.Range(-5,5),
            target.y
        );

        SetDirection();
    }
}
