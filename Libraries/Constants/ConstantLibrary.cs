﻿using UnityEngine;

namespace NiUtils.Libraries {
	[CreateAssetMenu(menuName = "Constants/Libraries/Constants")]
	public class ConstantLibrary : Library<string> {
		protected override string GetNonExistingWarningMessage(string key) => $"No constant named {key}";
	}
}