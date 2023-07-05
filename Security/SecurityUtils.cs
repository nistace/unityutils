using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace NiUtils.Security {
	public static class SecurityUtils {
		public static string SecureMd5(string input) {
			using (var md5 = MD5.Create()) {
				return md5.ComputeHash(Encoding.ASCII.GetBytes(input)).Select(t => t.ToString("X2")).Aggregate((t, u) => $"{t}{u}");
			}
		}
	}
}