using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FenrirPack.Pooling
{
	public class PooledObject : MonoBehaviour
	{
		[HideInInspector]
		public int poolIndex;
		[HideInInspector]
		public Pool myPool;

		public void Despawn()
		{
			myPool.Despawn(this);
		}
	}

}