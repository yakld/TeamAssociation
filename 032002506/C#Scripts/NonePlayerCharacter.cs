using TMPro;
using UnityEngine;

public class NonePlayerCharacter : MonoBehaviour
{
    //对话框显示时长
    public float displayTime = 4.0f;
    //用来获取对话框
    public GameObject dialogBox;
    //计时器
    float timerDisplay;
    //创建游戏对象获取TMP控件
    public GameObject TMPGameObject;
    TextMeshProUGUI _tmTexBox;
    //存储页数
    int _currentPage = 1;
    int _totalPages;

    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1.0f;
        _tmTexBox = TMPGameObject.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _totalPages = _tmTexBox.textInfo.pageCount;
        if (timerDisplay >= 0.0f)
        {
            //翻页
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(_currentPage < _totalPages)
                {
                    _currentPage += 1;
                }
                else
                {
                    _currentPage = 1;
                }
            }
            _tmTexBox.pageToDisplay = _currentPage;
            timerDisplay -= Time.deltaTime;
        }
        else
        {
            dialogBox.SetActive(false);
        }

    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
    }
}

