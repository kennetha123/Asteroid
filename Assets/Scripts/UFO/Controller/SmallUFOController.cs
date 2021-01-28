using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallUFOController : Entity<SmallUFO>
{
    public UFOData data;

    public float setTargetDelay;
    public float fireRadius;

    private float fireTime;
    private float setTargetTime;

    private Transform player;

    public void Init(GameManager manager)
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        instance = new SmallUFO(manager, gameObject, Random.insideUnitSphere * 3 + player.position, data, player, fireRadius);

        setTargetTime = Mathf.Clamp(setTargetDelay - (float)manager.currentScore / 10000f, 0.1f, 10);
        fireTime = Mathf.Clamp(2 - (float)manager.currentScore / 20000f, 1, 2);
    }

    // Update is called once per frame
    void Update()
    {
        instance.rigidBody.velocity = instance.direction.normalized * instance.data.speed;

        instance.EucledianTorus();

        if (player == null)
            return;

        if(setTargetTime > 0 )
        {
            setTargetTime -= Time.deltaTime;

            if(setTargetTime <= 0)
            {
                instance.SetTarget();

                setTargetTime = Mathf.Clamp(setTargetDelay - (float)instance.manager.currentScore / 10000f, 0.1f, 10);
            }
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
            player.instance.Explode();

            if (player.currentHealth > 1)
            {
                player.DecreaseHealth(player.healthLoss);
                instance.manager.currentHealth = player.currentHealth;
            }
            else
            {
                player.instance.Destroy();
                instance.manager.GameOver();
            }

            instance.Destroy();
        }
    }

    private void OnDestroy()
    {
        instance.manager.ufoCounter--;
    }
}
