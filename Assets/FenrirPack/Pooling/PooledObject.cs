using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FenrirPack.Pooling
{
	public class PooledObject : MonoBehaviour
	{
		[HideInInspector]
		internal int poolIndex;
		[HideInInspector]
		internal Pool myPool;

		public virtual void OnSpawn()
		{

		}

		public virtual void OnDespawn()
		{

		}

		public void Despawn()
		{
			myPool.Despawn(this);
		}
	}

}