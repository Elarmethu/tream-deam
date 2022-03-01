using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardCell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Main")]
    public CardData data;

    [Header("UI")]
    [SerializeField] private Image iconCell;
    [SerializeField] private Text nameCell;
    [SerializeField] private Text descriptionCell;
    
    [Header("Inspection")]
    [SerializeField] private Transform ViewPos;
    [SerializeField] private Transform NullPos;
    public AudioClip ViewAudio;

    // Mobile input system
    private float mb_timerInput;
    private bool isView;
    private bool isPressed;


    public void InitializeCard()
    {
        iconCell.sprite = data.Icon;
        nameCell.text = data.Name;
        descriptionCell.text = data.Desription.Replace("<br>", "\n");        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if(mb_timerInput >= 0.5f && isView)
        {
            isView = false;
            mb_timerInput = 0.0f;
            transform.position = Vector3.Lerp(transform.position, NullPos.position, 0.5f);
        } else 
        {
            if (PlayerLogic.Instance.canMotion)
                CardLogic.Instance.CardUse(this);
        }

        isPressed = false;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (mb_timerInput >= 0.5f && isView)
        {
            isView = false;
            transform.position = Vector3.Lerp(transform.position, NullPos.position, 0.5f);
        }
    }

    private void Update()
    {
        mb_timerInput = isPressed ? mb_timerInput += Time.deltaTime : 0.0f;
        
        if(mb_timerInput >= 0.5f && !isView)
        {
            transform.position = Vector3.Lerp(transform.position, ViewPos.position, 0.5f);
            isView = true;
            
            Game.Instance.source.clip = ViewAudio;
            Game.Instance.source.Play();
        }
    }
}
