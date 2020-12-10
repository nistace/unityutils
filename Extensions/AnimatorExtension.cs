using System.Linq;
using UnityEngine;

namespace Utils.Extensions {
	public static class AnimatorExtension {
		public static bool HasParameter(this Animator anim, int parameterId) => anim.parameters.Any(t => t.nameHash == parameterId);
		public static bool HasParameter(this Animator anim, string parameterName) => anim.parameters.Any(t => t.name == parameterName);
	}
}