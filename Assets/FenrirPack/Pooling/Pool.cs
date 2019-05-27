using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FenrirPack.Pooling
{
	public class Pool
	{
		#region Static Implementation
		const string poolsRootName = "PoolsParent";


		private static Dictionary<PooledObject, Pool> allPools;

		private static GameObject poolsRootObject;

		/// <summary>
		/// Initializes pooling system. Called automatically the first time a pool is created.
		/// </summary>
		private static void Initialize()
		{
			allPools = new Dictionary<PooledObject, Pool>();
			poolsRootObject = new GameObject(poolsRootName);
		}

		/// <summary>
		/// Creates a new pool, with the desired parameters.
		/// </summary>
		/// <param name="prefab">Prefab the pool is based on.</param>
		/// <param name="size">Starting pool size.</param>
		/// <param name="increaseStep">If pool reached limit, increase by this amount (if zero, will not increase and return null).</param>
		/// <returns>Pooling system for this specific prefab.</returns>
		public static Pool CreatePool(PooledObject prefab, int size = 1, int increaseStep = 0)
		{
			if (poolsRootObject == null)
			{
				Initialize();
			}

			Pool newPool = new Pool(prefab, size, increaseStep);

			allPools.Add(prefab, newPool);

			return newPool;

		}

		/// <summary>
		/// Gets an existing pool.
		/// </summary>
		/// <param name="prefab">Prefab to look for.</param>
		/// <param name="createIfNonExistent">If true, creates a new pooling system with size 1 and step 0.</param>
		/// <returns></returns>
		public static Pool GetPool(PooledObject prefab, bool createIfNonExistent = false)
		{
			Pool item;
			allPools.TryGetValue(prefab, out item);

			if (allPools == null)
			{
				if (createIfNonExistent)
				{
					item = CreatePool(prefab);
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
		bool IsFullyUsed
		{
			get
			{
				return pointer == currentPoolSize;
			}
		}

		private GameObject poolParent;

		private PooledObject prefab;
		private int increaseStep;

		private PooledObject[] poolItems;
		private int pointer;

		private int currentPoolSize;

		/// <summary>
		/// Constructor method. Initializes all references and creates the first items in the pool.
		/// </summary>
		/// <param name="prefab">Prefab to base the pool on.</param>
		/// <param name="startSize">Start pool size.</param>
		/// <param name="increaseStep">The amount of objects that will be created if the pool increases size.</param>
		public Pool(PooledObject prefab, int startSize, int increaseStep)
		{
			this.prefab = prefab;
			this.increaseStep = increaseStep;

			poolItems = new PooledObject[startSize];
			pointer = 0;
			currentPoolSize = 0;

			poolParent = new GameObject(prefab.name);
			poolParent.transform.SetParent(poolsRootObject.transform);
			CreateItems(startSize);
		}

		/// <summary>
		/// Creates a number of objects depending on the size.
		/// </summary>
		/// <param name="amount">Amount of items to be created.</param>
		private void CreateItems(int amount)
		{
			for (int i = 0; i < amount; i++)
			{
				CreateItem();
			}
		}

		/// <summary>
		/// Creates a new item, and places it on the end of the pool array.
		/// </summary>
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

		/// <summary>
		/// Spawn a new item.
		/// </summary>
		/// <typeparam name="T">Type to return.</typeparam>
		/// <returns>Instantiated item.</returns>
		public T Spawn<T>()
		{
			this.IncreasePoolIfNeeded();

				if (IsFullyUsed)
					return default;


			PooledObject obj = poolItems[pointer];
			pointer++;

			obj.gameObject.SetActive(true);

			return obj.GetComponent<T>();
		}

		/// <summary>
		/// Despawns object.
		/// </summary>
		/// <param name="obj">Object to be despawned.</param>
		public void Despawn(PooledObject obj)
		{
			obj.gameObject.SetActive(false);

			pointer--;
			int oldObjectIndex = obj.poolIndex;
			obj.poolIndex = pointer;
			poolItems[pointer].poolIndex = oldObjectIndex;

			poolItems[oldObjectIndex] = poolItems[pointer];
			poolItems[pointer] = obj;
		}

		/// <summary>
		/// Checks if pool is full, increaases its size by increaseStep.
		/// </summary>
		void IncreasePoolIfNeeded()
		{
			if (IsFullyUsed)
			{
				if (increaseStep > 0)
				{
					System.Array.Resize(ref poolItems, currentPoolSize + increaseStep);
					this.CreateItems(increaseStep);
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