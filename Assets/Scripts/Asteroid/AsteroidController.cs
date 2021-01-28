using UnityEngine;
using System;

public class AsteroidController : Entity<Asteroid>
{
    #region Public/Serializeable Variable

    public int size = 3;

    #endregion

    public void Init(GameManager manager)
    {
        instance = new Asteroid(manager, this.gameObject, size);
    }

    // Update is called once per frame
    void Update()
    {
        /* while this gameobject still used or activated *
         * set velocity into direction                   */
        if (gameObject.activeSelf)
            instance.rigidBody.velocity = instance.direction;

        instance.EucledianTorus();
    }

    /// <summary>
    /// Note: The Editor Camera is count as camera, so it will run if this gameobject seen by editor camera
    /// </summary>
    private void OnBecameVisible()
    {
        if (!instance.hasSeen)
            instance.hasSeen = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Rocket player = collision.gameObject.GetComponent<Rocket>();
            player.Hit();

            instance.Hit();
        }
    }
}
