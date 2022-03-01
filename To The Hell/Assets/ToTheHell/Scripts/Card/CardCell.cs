using UnityEngine;
using UnityEngine.UI;

public class CardCell : MonoBehaviour
{
    [Header("Main")]
    public CardData data;

    [Header("UI")]
    [SerializeField] private Image iconCell;
    [SerializeField] private Text nameCell;
    [SerializeField] private Text descriptionCell;
    [SerializeField] private Text costCell;
    [SerializeField] private Button buttonCell;
    
    [Header("Inspection")]
    [SerializeField] private Transform ViewPos;
    [SerializeField] private Transform NullPos;
    public AudioClip ViewAudio;

    public bool Initilized;
    public bool Choosed;

    // Mobile input system
    private float mb_timerInput;
    private bool isView;


    public void InitializeCard()
    {
        iconCell.sprite = data.Icon;
        nameCell.text = data.Name;
        descriptionCell.text = data.Desription;
        
    }
    
    public void CardUse()
    {
        if (GameObject.Find("EventSystem").GetComponent<PlayerLogic>().canMotion)
            GameObject.Find("EventSystem").GetComponent<CardLogic>().CardUse(this);
    }

#if UNITY_EDITOR || UNITY_STANDALONE
    private void OnMouseEnter()
    {
        transform.position = Vector3.Lerp(transform.position, ViewPos.position, 0.5f);

        Game.Instance.source.clip = ViewAudio;
        Game.Instance.source.Play();
    }

    private void OnMouseExit()
    {
        transform.position = Vector3.Lerp(transform.position, NullPos.position, 0.5f);
    }
#elif UNITY_ANDROID

    private void Update()
    {
        if(Input.touchCount > 0)
        {
            mb_timerInput += Time.deltaTime;
            
            if(mb_timerInput >= 0.5f && !isView)
            {
                transform.position = Vector3.Lerp(transform.position, ViewPos.position, 0.5f);   
                Game.Instance.source.clip = ViewAudio;
                Game.Instance.source.Play();
                isView = true;
            }
        }
        else
        {
            if (isView)
            {
                mb_timerInput = 0.0f;
                transform.position = Vector3.Lerp(transform.position, NullPos.position, 0.5f);
                isView = false;
            }
        }
    }

#endif
}
