using AxGrid;
using AxGrid.Base;
using AxGrid.Path;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Task2
{
    public class CardItem : MonoBehaviourExt
    {
        [SerializeField] private TMP_Text nameLable;

        public Card Card { get; private set; }

        private Button button;

        private RectTransform rectTransform;

        public RectTransform RectTransform
        {
            get
            {
                if(rectTransform == null)
                    rectTransform = GetComponent<RectTransform>();

                return rectTransform;
            }
        }

        [OnAwake]
        public void awake()
        {
            button = GetComponent<Button>();

            button.onClick.AddListener(OnClick);
        }

        public void SetCardModel(Card _card)
        {
            Card = _card;

            //update sprite, show name in TMP_Text and etc
            nameLable.text = Card.cardName;
        }

        public void MoveToPosition(Vector2 _position)
        {
            Vector2 startPosition = RectTransform.anchoredPosition;
            Path.Action(() => { }).EasingLinear(
                0.2f, 0f, 1f, (f) => RectTransform.anchoredPosition = Vector2.Lerp(startPosition, _position, f));
        }

        public void SetButtonInteractable(bool interactable)
        {
            button.interactable = interactable;
        }

        private void OnClick()
        {
            Settings.Fsm?.Invoke("OnCardBtn", Card);
        }
    }
}
