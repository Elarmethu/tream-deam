using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
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
}
