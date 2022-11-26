using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2D;
    public ParticleSystem hitEffect;

    // Start is called before the first frame update
    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();

    }
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2D.AddForce(direction * force);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RobotController robotController = collision.collider.GetComponent<RobotController>();
        if(robotController != null)
        {
            robotController.Fix();
        }
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        Debug.Log($"子弹碰撞到了{collision.gameObject}");
        Destroy(gameObject);//销毁子弹
    }

    private void Update()
    {
        //如果没有碰到任何碰撞体，在飞行了100米后就销毁
        if (transform.position.magnitude > 100)
        {
            Destroy(gameObject);
        }
    }

}
