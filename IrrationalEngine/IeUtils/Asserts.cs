using System;

namespace IeUtils
{
	public static class Asserts
	{
		public static void AssertNotNull(this object currentObject, string name)
		{
			if (currentObject == null)
			{
				throw new ArgumentNullException($"Element {name} is null.");
			}
		}

		public static void AssertNotNullorEmpty(this string currentObject, string name)
		{
			if (string.IsNullOrEmpty(currentObject))
			{
				throw new ArgumentNullException($"String {name} is null or empty.");
			}
		}
	}
}
