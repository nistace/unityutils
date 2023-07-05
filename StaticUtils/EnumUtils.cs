using System;

namespace NiUtils.StaticUtils {
	public static class EnumUtils {
		public static E[] Values<E>() where E : Enum => (E[])Enum.GetValues(typeof(E));

		public static void ForEach<E>(Action<E> action) where E : Enum {
			foreach (var e in Values<E>()) action(e);
		}

		public static int SizeOf<E>() where E : Enum => Values<E>().Length;
	}
}