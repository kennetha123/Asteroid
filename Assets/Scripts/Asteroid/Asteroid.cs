using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Asteroid : Instance
{
    private int size;

    public bool hasSeen;
    public Vector2 direction;

    public Asteroid(GameManager manager, GameObject gameObject, int size) : base(manager, gameObject)
    {
        this.size = size;

        hasSeen = false;
    }

    /// <summary>
    /// Launch this asteroid from a position
    /// to a direction
    /// </summary>
    /// <param name="pos"></param>
    public void Activate(Vector2 pos)
    {
        transform.position = pos;

        Vector3 dir = (UnityEngine.Random.insideUnitCircle * manager.camArea.y / 2);
        direction = (dir - transform.position) / 2;

        hasSeen = false;
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Destroy / split this asteroid based on size
    /// and spawn explosion particle
    /// </summary>
    public void Hit()
    {
        Explode();

        if (size > 1)
        {
            Split();
            return;
        }

        Destroy();
    }

    /// <summary>
    /// Split this asteroid into small asteroid
    /// based on size
    /// </summary>
    public void Split()
    {
        for (int i = 0; i < size; i++)
        {
            AsteroidController asteroid = manager.asteroidManager.GetSmallAsteroids().GetComponent<AsteroidController>();
            asteroid.instance.Activate(transform.position);
        }

        manager.asteroidCounter += size;
        Destroy();
    }

    /// <summary>
    /// destroy this asteroid from view
    /// </summary>
    public void Destroy()
    {
        hasSeen = false;

        gameObject.SetActive(false);
        rigidBody.velocity = Vector2.zero;

        manager.asteroidCounter--;
        manager.asteroidManager.Add(gameObject);
    }
}