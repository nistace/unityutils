using System;

public static class EnumUtils {
	public static E[] Values<E>() where E : Enum {
		return (E[]) Enum.GetValues(typeof(E));
	}

	public static int SizeOf<E>() where E : Enum {
		return Values<E>().Length;
	}
}