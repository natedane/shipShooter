using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    public ParticleSystem spray;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();

    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    void Update()
    {
        if(transform.position.magnitude > 1000.0f)
        {
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        enemyController e = other.collider.GetComponent<enemyController>();
        if (e != null)
        {
            e.Hit();
        }

        SentryEnemy e2 = other.collider.GetComponent<SentryEnemy>();
        if (e2 != null)
        {
            e2.Hit();
        }

        carrierEnemy e3 = other.collider.GetComponent<carrierEnemy>();
        if (e3 != null)
        {
            e3.Hit();
        }

        shipController shot = other.gameObject.GetComponent<shipController >();
        if (shot != null)
        {
            shot.ChangeHealth(-1);
        }
        //SpriteRenderer spray = child.FindChild("big").GetComponent<SpriteRenderer>();
        playSystem();
        Destroy(gameObject);
    }

    void playSystem(){
        ParticleSystem s = Instantiate(spray, transform.position, Quaternion.identity);
        s.Play();
        Destroy(s, 1f);

    }
}
