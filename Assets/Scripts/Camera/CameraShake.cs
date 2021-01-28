using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private float shake = 0;
    [SerializeField] private float shakeAmount = 0.7f;

    // Update is called once per frame
    void Update()
    {
        if(shake > 0)
        {
            shake -= Time.deltaTime;
            this.transform.position = new Vector3(Random.insideUnitSphere.x * shakeAmount, Random.insideUnitSphere.y * shakeAmount, transform.position.z);

            if(shake <= 0)
            {
                shake = 0;
                this.transform.position = new Vector3(0, 0, transform.position.z);
            }
        }
    }

    public void Shake(float time)
    {
        shake = time;
    }
}
