using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : Entity<Player>, IHealth
{
    #region Public/Serializeable Variable

    public int healthLoss = 1;
    public PlayerData data;

    #endregion

    #region Private/Hidden Variable

    [HideInInspector]
    public int currentHealth { get; set; }

    private float invulnerableTime;

    #endregion

    public void Init(GameManager manager)
    {
        instance = new Player(manager, gameObject, data);

        // start game with starting health
        currentHealth = manager.startingHealth;
        manager.currentHealth = currentHealth;
        invulnerableTime = 5;
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * instance.data.angularSpeed * Time.deltaTime);
        instance.rigidBody.AddForce(transform.up * instance.data.thrust * Input.GetAxis("Vertical"));
        instance.rigidBody.velocity = new Vector2(
            Mathf.Clamp(instance.rigidBody.velocity.x, -instance.data.maxSpeed, instance.data.maxSpeed), 
            Mathf.Clamp(instance.rigidBody.velocity.y, -instance.data.maxSpeed, instance.data.maxSpeed)
        );

        // if this gameobject out of camera sight
        instance.EucledianTorus();

        // fire!
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            try
            {
                instance.SpawnLaser();
            }
            catch
            {
                GameObject laser = instance.CreateLaser();
                laser.GetComponent<PlayerLaser>().Activate(transform, data.firePoint.position - transform.position);
            }
        }

        // 'stopping' rocket when there's no vertical input
        instance.rigidBody.drag = Input.GetAxisRaw("Vertical") == 0 ? instance.data.friction : instance.defaultDrag;

        if(invulnerableTime > 0)
        {
            invulnerableTime -= Time.deltaTime;
            GetComponent<Collider2D>().enabled = false;

            if(invulnerableTime <= 0)
            {
                GetComponent<Collider2D>().enabled = true;

                invulnerableTime = 0;
            }
        }

        instance.anim.SetBool("Invulnerable", !GetComponent<Collider2D>().enabled);
    }

    public void Hit()
    {
        instance.Explode();
        Camera.main.GetComponent<CameraShake>().Shake(0.3f);

        if (currentHealth > 1)
        {
            DecreaseHealth(healthLoss);
            instance.manager.currentHealth = currentHealth;

            invulnerableTime = 5;
        }
        else
        {
            instance.Destroy();
            instance.manager.GameOver();
        }
    }

    public int DecreaseHealth(int value)
    {
        return currentHealth -= value;
    }

    public int IncreaseHealth(int value)
    {
        return currentHealth += value;
    }
}
