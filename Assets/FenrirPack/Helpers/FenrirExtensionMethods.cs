using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FenrirPack
{
	public static partial class ExtensionMethods
	{
		public static TList FindInList<TList, TItem>(this List<TList> collection, TItem parameter) where TList : System.IEquatable<TItem>
		{
			for (int i = 0; i < collection.Count; i++)
			{
				if (collection[i].Equals(parameter))
				{
					return collection[i];
				}
			}
			return default;
		}

		public static TList FindInArray<TList, TItem>(this TList[] collection, TItem parameter) where TList : System.IEquatable<TItem>
		{
			for (int i = 0; i < collection.Length; i++)
			{
				if (collection[i].Equals(parameter))
				{
					return collection[i];
				}
			}
			return default;
		}
	} 
}