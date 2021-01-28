using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LaserData
{
    public float speed = 10;
    public float aliveTime = 2f;
}

public class Laser<T> : Instance 
{
    public LaserData data;
    public T parent;

    public Vector2 direction;

    public Laser(GameManager manager, GameObject gameObject, T parent, LaserData data) : base(manager, gameObject)
    {
        this.parent = parent;
        this.data = data;
    }

    public void Destroy()
    {
        gameObject.SetActive(false);
        (parent as Controller).availableLaser.Add(gameObject);
    }

    public void DestroyTarget<Target, Type>(GameObject obj) where Type : Controller where Target : Entity<Type>
    {
        if (obj.GetComponent<Target>() == null)
            return;

        Target target = obj.GetComponent<Target>();

        target.instance.Destroy();
        manager.currentScore += target.score;

        Camera.main.GetComponent<CameraShake>().Shake(0.2f);

        Destroy();
    }
}
