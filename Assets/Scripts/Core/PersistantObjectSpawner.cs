using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGJ.Core
{
	public class PersistantObjectSpawner : MonoBehaviour
	{
		[SerializeField] GameObject persistantObjectPrefab = null;

		static bool hasSpawned = false;

		void Awake()
		{
            if (hasSpawned) { return; }

            SpawnPersistantObject();
            hasSpawned = true;
		}

		void SpawnPersistantObject()
		{
			GameObject persistantObject = Instantiate(persistantObjectPrefab);
			DontDestroyOnLoad(persistantObject);
		}
	}
}