using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FenrirPack.Pooling
{
	public class Pool
	{
		#region Static Implementation
		const string poolsRootName = "PoolsScene";


		private static Dictionary<PooledObject, Pool> allPools;

		private static Scene poolsRootScene;

		private static bool isInitialized;

		private static void Initialize()
		{
			allPools = new Dictionary<PooledObject, Pool>();
			poolsRootScene = SceneManager.CreateScene(poolsRootName);

			isInitialized = true;
		}

		public static Pool Create(PooledObject prefab, int size = 1, int increaseStep = 0, bool scenePersistent = false)
		{
			if (!isInitialized)
			{
				Initialize();
			}

			Pool newPool = new Pool(prefab, size, increaseStep, scenePersistent);

			allPools.Add(prefab, newPool);

			return newPool;
		}

		public static Pool GetPool(PooledObject prefab, bool createIfNonExistent = false)
		{
			Pool item;
			allPools.TryGetValue(prefab, out item);

			if (allPools == null)
			{
				if (createIfNonExistent)
				{
					item = Create(prefab);
				}
				else
				{
					Debug.LogWarning("Non existent pool for prefab " + prefab);
				}
			}
			return item;
		}
		#endregion



		#region Instance implementation

		private GameObject poolParent;

		private PooledObject prefab;
		private int increaseStep;

		private PooledObject[] poolItems;
		private int pointer;

		private int currentPoolSize;
		bool IsFullyUsed
		{
			get
			{
				return pointer == currentPoolSize;
			}
		}

		public Pool(PooledObject prefab, int startSize, int increaseStep, bool scenePersistent)
		{
			this.prefab = prefab;
			this.increaseStep = increaseStep;

			poolItems = new PooledObject[startSize];
			pointer = 0;
			currentPoolSize = 0;

			poolParent = new GameObject(prefab.name);

			SceneManager.MoveGameObjectToScene(poolParent, poolsRootScene);
			poolParent.SetActive(false);

			if(scenePersistent)
			{
				GameObject.DontDestroyOnLoad(poolParent);
			}

			CreatePoolItems(startSize);
		}

		private void CreatePoolItems(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				CreateItem();
			}
		}

		private void CreateItem()
		{
			if(currentPoolSize == poolItems.Length)
			{
				Debug.LogError("Can't create a new item for pool " + prefab + ". This is probably because this method is called somewhere it shouldn't.");
			}

			PooledObject item = GameObject.Instantiate(prefab, poolParent.transform);
			item.myPool = this;
			item.gameObject.SetActive(false);

			poolItems[currentPoolSize] = item;
			currentPoolSize++;
		}

		public T Spawn<T>(Transform parent)
		{
			return Spawn(parent).GetComponent<T>();
		}

		public PooledObject Spawn(Transform parent)
		{
			this.ChheckForSizeChange();

			if (IsFullyUsed)
				return default;


			PooledObject obj = poolItems[pointer];
			pointer++;

			obj.transform.SetParent(parent);

			obj.OnSpawn();
			obj.gameObject.SetActive(true);

			return obj;
		}

		public void Despawn(PooledObject obj)
		{
			obj.OnDespawn();
			obj.gameObject.SetActive(false);
			obj.transform.SetParent(poolParent.transform);

			pointer--;
			int oldObjectIndex = obj.poolIndex;
			obj.poolIndex = pointer;
			poolItems[pointer].poolIndex = oldObjectIndex;

			poolItems[oldObjectIndex] = poolItems[pointer];
			poolItems[pointer] = obj;
		}

		private void ChheckForSizeChange()
		{
			if (IsFullyUsed)
			{
				if (increaseStep > 0)
				{
					System.Array.Resize(ref poolItems, currentPoolSize + increaseStep);
					this.CreatePoolItems(increaseStep);
				}
				else
				{
					Debug.LogWarning("Pool " + poolParent + " reached its maximum limit, and can't grow. No item will be returned.");
				}
			}
		}

		#endregion
	}
}