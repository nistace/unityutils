using NiUtils.Pun.Random;
using NiUtils.Types;

namespace NiUtils.Pun.Extensions {
	public static class RatioExtension {
		public static bool NetworkRoll(this Ratio ratio, bool winUnder = true) => Ratio.Roll(ratio.value, PunRandom.GetNextValue(), winUnder);
	}
}