using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaser : MonoBehaviour
{
    #region Public/Serializeable Variable

    public LaserData data;
    public Laser<Player> instance;

    #endregion

    #region Private/Hidden Variable

    private float currentAliveTime = 0;         // automatically destroy this laser after currentAliveTime = 0

    #endregion

    // Update is called once per frame
    void Update()
    {
        // automatically destroy after certain amount of time
        if (currentAliveTime > 0)
        {
            currentAliveTime -= Time.deltaTime;
            instance.rigidBody.velocity = instance.direction * instance.data.speed;

            if (currentAliveTime <= 0)
            {
                instance.Destroy();
            }
        }

        instance.EucledianTorus();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        /* destroying asteroid that hit this laser            *
         * if this asteroid size > 1, then split the asteroid */
        if(collision.gameObject.CompareTag(instance.destructableTag))
        {
            if(collision.gameObject.GetComponent<AsteroidController>() != null)
            {
                AsteroidController asteroid = collision.gameObject.GetComponent<AsteroidController>();
                instance.manager.currentScore += asteroid.score * asteroid.size;

                asteroid.instance.Hit();
                Camera.main.GetComponent<CameraShake>().Shake(0.2f);
                instance.Destroy();
            }

            instance.DestroyTarget<BigUFOController, BigUFO>(collision.gameObject);
            instance.DestroyTarget<SmallUFOController, SmallUFO>(collision.gameObject);
        }
    }

    /// <summary>
    /// Get Rocket reference from the scene
    /// </summary>
    /// <param name="player"></param>
    public void Init(Player player, GameManager manager)
    {
        instance = new Laser<Player>(manager, gameObject, player, data);
    }

    /// <summary>
    /// activate this laser from pos, and firing into dir
    /// </summary>
    /// <param name="pos">fire point position</param>
    /// <param name="dir">fire point direction</param>
    public void Activate(Transform pos, Vector2 dir)
    {
        transform.position = pos.position;
        transform.rotation = pos.rotation;

        instance.direction = dir;
        currentAliveTime = instance.data.aliveTime;

        gameObject.SetActive(true);
    }
}
