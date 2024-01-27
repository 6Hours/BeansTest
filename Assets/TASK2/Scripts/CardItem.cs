using AxGrid;
using AxGrid.Base;
using AxGrid.Path;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Task2
{
    public class CardItem : MonoBehaviourExt, IPointerClickHandler
    {
        [SerializeField] private TMP_Text nameLable;

        public Card Card { get; private set; }

        private Image image;

        private Image Image
        {
            get
            {
                if (image == null)
                {
                    image = GetComponent<Image>();
                }

                return image;
            }
        }

        public RectTransform RectTransform => Image.rectTransform;

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

        public void SetCardInteractable(bool interactable)
        {
            image.raycastTarget = interactable;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            Settings.Fsm?.Invoke("OnCardBtn", Card);
        }
    }
}
