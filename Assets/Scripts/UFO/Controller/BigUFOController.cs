using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigUFOController : Entity<BigUFO>
{
    public UFOData data;

    private float fireTime;

    public void Init(GameManager manager)
    {
        instance = new BigUFO(manager, gameObject, Random.insideUnitSphere * manager.camArea.y, data, manager.camArea.y);
        fireTime = Mathf.Clamp(2 - (float)manager.currentScore / 20000f, 1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        instance.rigidBody.velocity = instance.direction.normalized * instance.data.speed;

        if (instance.EucledianTorus())
        {
            instance.SetTarget();
        }

        if(fireTime > 0)
        {
            fireTime -= Time.deltaTime;

            if(fireTime <= 0)
            {
                instance.SpawnLaser();

                fireTime = Mathf.Clamp(2 - (float)instance.manager.currentScore / 20000f, 1, 2);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rocket player = collision.gameObject.GetComponent<Rocket>();
            player.Hit();

            instance.Destroy();
        }
    }

    private void OnDestroy()
    {
        instance.manager.ufoCounter--;
    }
}
