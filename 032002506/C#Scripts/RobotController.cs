using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class RobotController : MonoBehaviour
{
    public float speed = 1f;
    public bool vertical;//竖着走还是横着走
    public float changeTime = 3.0f;//反转敌人方向时间
    Rigidbody2D rigidbody2D;
    Vector2 position;
    int direction = 1;//方向
    float timer;//计时器

    public AudioClip fixClip;//修复音频
    public AudioSource audioSource;

    Animator animator;

    bool broken = true;//机器人是否坏掉
    public ParticleSystem smokeEffect;//烟雾特效

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); 
        audioSource =  GetComponent<AudioSource>();
        timer = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (!broken)
        {
            return;
        }

        if(timer >= 0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = changeTime;
            direction = -direction;
        }
    }

    private void FixedUpdate()
    {
        if (!broken)
        {
            return;
        }
        MovePosition();
    }

    private void MovePosition()
    {
        position = transform.position;
        if (vertical)
        {
            position.y += speed * direction * Time.deltaTime;
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else
        {
            position.x += speed * direction * Time.deltaTime;
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
        
        rigidbody2D.position = position;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController player = collision.gameObject.GetComponent<RubyController>();
        if(player != null)
        {
            player.ChangeHealth(-1);
        }
    }
    //修复机器人
    public void Fix()
    {
        broken = false;//修复
        audioSource.PlayOneShot(fixClip);//播放修复音频
        //让机器人不能碰撞
        rigidbody2D.simulated = false;
        
        //播放修理动画
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();//停止烟雾特效
        audioSource.clip = null;//停止走路音效
    }
}
