﻿using System;
using System.Text.RegularExpressions;

namespace Utils.Debugging {
	public struct CheatCode {
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