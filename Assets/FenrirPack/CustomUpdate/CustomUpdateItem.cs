namespace FenrirPack.CustomTime
{
	public class CustomUpdateItem : System.IEquatable<CustomUpdateItem>, System.IEquatable<float>
	{
		public float TimeElapsedCounter;
		public float TimeInterval;

		public System.Action OnUpdate;

		public bool Equals(CustomUpdateItem other)
		{
			return other.TimeInterval == TimeInterval;
		}

		public bool Equals(float other)
		{
			return TimeInterval == other;
		}
	} 
}