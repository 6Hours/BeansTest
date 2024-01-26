using AxGrid;
using AxGrid.Base;
using AxGrid.Path;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Task2
{
    public class CardItem : MonoBehaviourExt, IPointerClickHandler
    {
        [SerializeField] private TMP_Text nameLable;

        public Card Card { get; private set; }

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
                1f, 0f, 1f, (f) => RectTransform.anchoredPosition = Vector2.Lerp(startPosition, _position, f));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Settings.Fsm?.Invoke("OnCardButton", this);
        }
    }
}
