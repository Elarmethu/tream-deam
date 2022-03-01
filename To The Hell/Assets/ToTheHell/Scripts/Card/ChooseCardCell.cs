using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseCardCell : MonoBehaviour
{
    [Header("Main")]
    public CardData data;

    [Header("UI")]
    [SerializeField] private Image iconCell;
    [SerializeField] private Text nameCell;
    [SerializeField] private Text descriptionCell;
    [SerializeField] private Text costCell;
    [SerializeField] private Button buttonCell;

    public void InitializeCard()
    {
        iconCell.sprite = data.Icon;
        nameCell.text = data.Name;
        descriptionCell.text = data.Desription;
    }

    public void CardChoose()
    {
        var logic = GameObject.Find("EventSystem").GetComponent<CardLogic>();
       // logic.AddData(data, transform.parent.gameObject);
    }
}
