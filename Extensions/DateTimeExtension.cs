using System;

namespace NiUtils.Extensions {
	public static class DateTimeExtension {
		public static string ToLongDateTimeString(this DateTime dateTime) => dateTime.ToLongDateString() + " " + dateTime.ToLongTimeString();
	}
}