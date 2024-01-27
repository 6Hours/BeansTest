using AxGrid;
using AxGrid.Base;
using AxGrid.Model;
using System.Collections.Generic;
using System.Linq;
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
			Model.EventManager.AddAction($"On{listField}Changed", Changed);
		}

		protected virtual void Changed()
		{
			var dataList = Model.Get<List<Card>>(listField);

			float startPoint = (dataList.Count - 1) * prefab.RectTransform.sizeDelta.x * -0.4f;
			for (int i = 0; i < Mathf.Max(dataList.Count, cardItems.Count); i++)
            {
				if (i == cardItems.Count)
				{
					cardItems.Add(FindInPoolOrCreate(dataList[i]));
					cardItems[i].RectTransform.SetParent(transform);
				}

				if (dataList.Contains(cardItems[i].Card))
                {
					cardItems[i].MoveToPosition(
						Vector2.right * (startPoint + i * prefab.RectTransform.sizeDelta.x * 0.8f) +
						Vector2.down * 20f * (i % 2));
				}
				else
                {
					pool.Add(cardItems[i]);
					cardItems.RemoveAt(i);
					i--;
                }
			}
        }

		[OnDestroy]
		public void onDestroy()
		{
			Model.EventManager.RemoveAction("ModelChanged", Changed);
		}

		private static List<CardItem> pool = new List<CardItem>();

		private CardItem FindInPoolOrCreate(Card card)
        {
			var cardItem = pool.FirstOrDefault(item => item.Card == card);
			if(cardItem == null)
            {
				cardItem = Instantiate(prefab);
				cardItem.SetCardModel(card);
				return cardItem;
			}
			else
            {
				return cardItem;
            }
        }
	}
}