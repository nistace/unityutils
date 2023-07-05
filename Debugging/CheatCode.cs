using System;
using System.Text.RegularExpressions;
using NiUtils.Libraries;

namespace NiUtils.Debugging {
	public readonly struct CheatCode {
		public Func<Match, string> solve              { get; }
		public int                 protectionLevel    { get; }
		public string              name               { get; }
		public string              displayName        => Localisation.Map($"cheat.func.{name}");
		public string              displayDescription => Localisation.Map($"cheat.func.{name}.description");

		public CheatCode(Func<Match, string> solve, int protectionLevel) {
			this.solve = solve;
			name = solve.Method.Name.Replace("Handle", "").Replace("CheatCode", "");
			this.protectionLevel = protectionLevel;
		}
	}
}