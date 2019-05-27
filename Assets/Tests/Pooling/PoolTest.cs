using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FenrirPack.Pooling;

public class PoolTest : MonoBehaviour
{
	[SerializeField]
	PooledObject prefab;

	Pool pool;

	PooledObject instance;

	private void Start()
	{
		pool = Pool.CreatePool(prefab, 10, 5);

		instance = pool.Spawn(this.transform);

		Invoke("Despawn", 2);
	}

	void Despawn()
	{
		instance.Despawn();
	}
}