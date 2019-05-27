using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FenrirPack;

namespace FenrirPack.CustomTime
{
	public class CustomUpdateManager : Singleton<CustomUpdateManager>
	{
		List<CustomUpdateItem> intervalItems;
		int intervalItemsCount;

		public CustomUpdateManager()
		{
			intervalItems = new List<CustomUpdateItem>();
			intervalItemsCount = 0;
		}

		public void AddListenerForInterval(float interval, System.Action OnUpdate)
		{
			CustomUpdateItem item = intervalItems.FindInList(interval);
			if (item == null)
			{
				item = new CustomUpdateItem() { TimeInterval = interval };
				intervalItems.Add(item);
				intervalItemsCount++;
			}
			item.OnUpdate += OnUpdate;
		}

		public void RemoveListenerForInterval(float interval, System.Action OnUpdate)
		{
			CustomUpdateItem item = intervalItems.FindInList(interval);
			if (item != null)
			{
				item.OnUpdate -= OnUpdate;
				if (item.OnUpdate.GetInvocationList().Length == 0)
				{
					intervalItems.Remove(item);
					intervalItemsCount--;
				}
			}
		}

		private void FixedUpdate()
		{
			for (int i = 0; i < intervalItemsCount; i++)
			{
				CustomUpdateItem update = intervalItems[i];

				update.TimeElapsedCounter += Time.fixedDeltaTime;
				if (update.TimeElapsedCounter > update.TimeInterval)
				{
					update.TimeElapsedCounter = 0;
					update.OnUpdate?.Invoke();
				}
			}
		}
	} 
}