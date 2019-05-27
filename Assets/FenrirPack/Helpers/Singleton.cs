using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FenrirPack
{
	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		private static T instance = null;

		/// <summary>This singleton instance. There can be only one script of this type in the whole scene.</summary>
		public static T Instance
		{
			get { if (instance == null) instance = CreateInstance(); return instance; }
		}

		private static T CreateInstance()
		{
			T instance;
			System.Type type = typeof(T);
			instance = Object.FindObjectOfType(type) as T; // It's not that fast, but it's supposed to run once per singleton only.

			if (!instance)
			{
				GameObject obj = new GameObject(type.Name);
				instance = obj.AddComponent<T>();
			}

			return instance;
		}

		public static bool IsInstanced { get { return instance != null; } }
	} 
}