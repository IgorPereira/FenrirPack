using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FenrirPack.Pooling;

public class PoolTest : MonoBehaviour
{
	[SerializeField]
	PooledObject prefabPersistent;
	[SerializeField]
	PooledObject prefabScene;

	Pool persistentPool;
	Pool scenePool;

	PooledObject persistentPoolObj;
	PooledObject scenePoolObj;

	private void Start()
	{
		persistentPool = Pool.Create(prefabPersistent, 10, 5, true);
		scenePool = Pool.Create(prefabScene, 10, 5, false);

		persistentPoolObj = persistentPool.Spawn(this.transform);
		scenePoolObj = scenePool.Spawn(this.transform);

		Invoke("Despawn", 2);
	}

	void Despawn()
	{
		persistentPoolObj.Despawn();
		scenePoolObj.Despawn();
	}
}