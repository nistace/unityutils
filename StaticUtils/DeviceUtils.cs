using System.Net.NetworkInformation;

namespace NiUtils.StaticUtils {
	public static class DeviceUtils {
		public static string GetPhysicalAddress() {
			var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			foreach (var adapter in networkInterfaces) {
				var address = adapter.GetPhysicalAddress();
				if (!string.IsNullOrEmpty(address.ToString())) {
					return address.ToString();
				}
			}
			return string.Empty;
		}
	}
}