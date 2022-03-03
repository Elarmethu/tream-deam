using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using TMPro;

public class CardCell : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Header("Main")]
    public CardData data;
    [SerializeField] private Image backgroundCard;
    [SerializeField] private Sprite withoutAttackSprite;
    [SerializeField] private Sprite withAttackSprite;
    [SerializeField] private Sprite bemoleSprite;

    [Header("UI - Header")]
    [SerializeField] private Image levelCard;
    [SerializeField] private Text evredikaCost;
    [SerializeField] private TMP_Text nameCard;
    [SerializeField] private List<Sprite> levelsSprite;

    [Header("UI - Center")]
    [SerializeField] private Image logoCard;

    [Header("UI - Bottom")]
    [SerializeField] private Transform infoTransform;
    [SerializeField] private GameObject infoPrefab;
    [SerializeField] private List<Sprite> descriptionSprites;

    [Header("Inspection")]
    [SerializeField] private Transform ViewPos;
    [SerializeField] private Transform NullPos;
    public AudioClip ViewAudio;

    //Mobile Input System
    private float mb_timerInput;
    private bool isView;
    private bool isPressed;

    public void InitializeCard()
    {
        backgroundCard.sprite = data.Damage > 0 ? withAttackSprite : withoutAttackSprite;   
        evredikaCost.enabled = data.Name == "Bemol" ? false : true;
        if (data.Name == "Bemol") backgroundCard.sprite = bemoleSprite;

        levelCard.sprite = levelsSprite[data.Level - 1];
        nameCard.text = data.Name;
        evredikaCost.text = data.Evridika.ToString();
        logoCard.sprite = data.Icon;
        evredikaCost.color = backgroundCard.sprite != withAttackSprite ? new Color32(162, 162, 162, 255) : new Color32(24, 24, 24, 255);
        levelCard.color = backgroundCard.sprite != withAttackSprite ? new Color32(162, 162, 162, 255) : new Color32(24, 24, 24, 255);
        nameCard.color = backgroundCard.sprite != withAttackSprite ? new Color32(162, 162, 162, 255) : new Color32(24, 24, 24, 255);

        if (data.Evridika < 0)
        {
            var description = Instantiate(infoPrefab);
            description.transform.SetParent(infoTransform);
            description.GetComponent<Image>().sprite = descriptionSprites[2];
            description.transform.localScale = Vector3.one;

            description.GetComponentInChildren<Text>().text = Mathf.Abs(data.Evridika).ToString();
            description.GetComponentInChildren<Text>().color = backgroundCard.sprite == withAttackSprite ? new Color32(162, 162, 162, 255) : new Color32(24, 24, 24, 255);
        }

        if (data.Damage > 0)
        {
            var description = Instantiate(infoPrefab);
            description.transform.SetParent(infoTransform);
            description.GetComponent<Image>().sprite = descriptionSprites[0];
            description.transform.localScale = Vector3.one;

            Debug.Log(description.GetComponentInChildren<Text>().text);
            var text = description.GetComponentInChildren<Text>();
            text.text = data.Damage.ToString();
            Debug.Log(description.GetComponentInChildren<Text>().text);

            description.GetComponentInChildren<Text>().color = backgroundCard.sprite == withAttackSprite ? new Color32(162, 162, 162, 255) : new Color32(24, 24, 24, 255);
        }

        if(data.Shield > 0)
        {
            var description = Instantiate(infoPrefab);
            description.transform.SetParent(infoTransform);
            description.GetComponent<Image>().sprite = descriptionSprites[1];
            description.transform.localScale = Vector3.one;

            description.GetComponentInChildren<Text>().text = data.Shield.ToString();
            description.GetComponentInChildren<Text>().color = backgroundCard.sprite == withAttackSprite ? new Color32(162, 162, 162, 255) : new Color32(24, 24, 24, 255);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (mb_timerInput >= 0.5f && isView)
        {
            isView = false;
            mb_timerInput = 0.0f;
            transform.position = Vector3.Lerp(transform.position, NullPos.position, 0.5f);
        }
        else
        {
            if (PlayerLogic.Instance.canMotion)
            {
                CardLogic.Instance.CardUse(this);
            }
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

        if (mb_timerInput >= 0.5f && !isView)
        {
            transform.position = Vector3.Lerp(transform.position, ViewPos.position, 0.5f);
            isView = true;

            Game.Instance.source.clip = ViewAudio;
            Game.Instance.source.Play();
        }
    }
}
