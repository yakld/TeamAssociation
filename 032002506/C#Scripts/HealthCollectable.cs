using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    public int amount = 1;
    public ParticleSystem cureEffect;
    public AudioClip collectedClip;

    //添加触发器碰撞事件
    private void OnTriggerEnter2D(Collider2D collision)
    {
        RubyController rubyController = collision.GetComponent<RubyController>();
        if(rubyController != null)
        {
            if(rubyController.health < rubyController.maxHealth)
            {
                rubyController.ChangeHealth(amount);
                Instantiate(cureEffect, transform.position, Quaternion.identity);
                Destroy(gameObject);
                //播放吃草莓音效
                rubyController.PlaySound(collectedClip);
            }
            else
            {
                Debug.Log("当前玩家生命值是满的,不用加血");
            }
            
        }
        else
        {
            Debug.LogError("rubyController组件没有被获取到");
        }
        
    }
}
