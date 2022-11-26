using UnityEngine;

public class RubyController : MonoBehaviour
{
    public int maxHealth = 5;//最大生命值
    int currentHealth;//当前生命值
    public float timeOfInvincible = 2.0f;//无敌时间
    bool isInvincible;//是否无敌标志
    float invincibleTime = 2f;//无敌时间计时器
    public int health
    {
        get { return currentHealth; }
        //set { currentHealth = value; }
    }
    

    float horizontal;
    float vertical;
    public float speed = 3.0f;
    Rigidbody2D rigidbody2D;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);//静止时矢量
    Vector2 move;//移动矢量

    public GameObject projectilePrefab;//子弹预制件

    //音频源对象
    AudioSource audioSource;
    public AudioClip hitClip;//受伤音频
    public AudioClip throwClip;//发射齿轮声音剪辑

    // Start is called before the first frame update
    void Start()
    {
        /*
        QualitySettings.vSyncCount = 0;//垂直同步技术设置为0才能锁帧
        Application.targetFrameRate = 10;//目标帧数=10
        */
        //获取当前游戏对象的刚体组件
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();//动画组件
        audioSource = GetComponent<AudioSource>();  

        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //创建二维矢量来存储ruby数据
        move = new Vector2(horizontal, vertical);
        //正在移动中
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();//向量长度设为1
        }
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);//magnitue属性返回矢量长度

        //处于无敌状态
        if (isInvincible)
        {
            invincibleTime -= Time.deltaTime;
            if(invincibleTime < 0)
            {
                isInvincible = false;
            }
        }

        //添加发射子弹逻辑
        if (Input.GetKeyDown(KeyCode.K) || Input.GetAxis("Fire1")!=0)
        {
            Launch();
        }

        //NPC交互逻辑
        if (Input.GetKeyDown(KeyCode.E))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC")); 
            if(hit.collider != null)
            {
                Debug.Log($"射线碰到了{hit.collider.gameObject}");
            }
            NonePlayerCharacter npc = hit.collider.GetComponent<NonePlayerCharacter>();
            if(npc != null)
            {
                npc.DisplayDialog();
            }

        }
    }

    private void FixedUpdate()
    {
        Vector2 position = transform.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        rigidbody2D.position = position;
    }

    internal void ChangeHealth(int amount)
    {
        if(amount < 0)
        {
            if (isInvincible)
            {
                return;
            }
            isInvincible = true;
            invincibleTime = timeOfInvincible;
        }
        //生命值介于0到maxHealth之间
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        
        if(amount < 0)
        {
            //播放受伤动画
            animator.SetTrigger("Hit");
            //播放受伤音频
            PlaySound(hitClip);
        }
        
        Debug.LogFormat("当前生命值：{0}/{1}", currentHealth, maxHealth);
        //设置血条
        UIHealthBar.instance.SetValue(currentHealth/(float)maxHealth);
    }
    //发射子弹
    private void Launch()
    {
        //实例化子弹
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);//通过脚本对象调用子弹移动方法
        animator.SetTrigger("Launch");//发射子弹动画
        PlaySound(throwClip);//播放音频
    }

    //播放音频剪辑
    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }
}
