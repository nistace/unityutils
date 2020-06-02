using System;

public static class DateTimeExtension {
	public static string ToLongDateTimeString(this DateTime dateTime) => dateTime.ToLongDateString() + " " + dateTime.ToLongTimeString();
}