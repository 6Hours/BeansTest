using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Task2
{
	public class UICardCollectionDataBind : MonoBehaviourExtBind
	{
		[SerializeField] private CardItem prefab;

		private List<CardItem> cardItems = new List<CardItem>();

		public string listField = "";

		/// <summary>
		/// Отправляет события во вспомогательную UI fsm
		/// </summary>
		public bool isFsmUI = false;

		[OnAwake]
		public void awake()
		{
			listField = listField == "" ? $"List{name}Cards" : listField;
		}

		[OnStart]
		public void start()
		{
			Model.EventManager.AddAction("ModelChanged", Changed);
			Model.EventManager.AddAction<CardItem>("MoveCard", MoveCard);
		}

		public void Changed()
        {
			var dataList = Model.Get<List<Card>>(listField);

			if (dataList == null) return;

			float startPoint = (dataList.Count - 1) * prefab.RectTransform.sizeDelta.x * 0.4f;
			for (int i = 0; i < dataList.Count; i++)
            {
				if (i == cardItems.Count)
				{
					cardItems.Add(Instantiate(prefab, transform));
					cardItems[i].SetCardModel(dataList[i]);
				}

				cardItems[i].MoveToPosition(Vector2.left * (startPoint + i * prefab.RectTransform.sizeDelta.x * 0.8f));
            }
        }

		public void MoveCard(CardItem cardItem)
        {
			if(cardItems.Contains(cardItem))
            {
				cardItems.Remove(cardItem);
            }
			else
            {
				cardItems.Add(cardItem);

				cardItem.RectTransform.parent = transform;
				cardItem.RectTransform.SetAsLastSibling();
			}
        }

		[OnDestroy]
		public void onDestroy()
		{
			Model.EventManager.RemoveAction("ModelChanged", Changed);
			Model.EventManager.RemoveAction<CardItem>("MoveCard", MoveCard);
		}


	}
}