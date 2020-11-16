using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Utils.Debugging {
	public static class CheatManager {
		public static int userAccessLevel { get; set; }

		private static Dictionary<Regex, CheatCode> cheatCodes { get; } = new Dictionary<Regex, CheatCode> {
			{new Regex("^ *help *(\\w+)? *$"), new CheatCode(HandleHelpCheatCode, 0)}, {new Regex("^ *exit-debug *(\\w+)? *$"), new CheatCode(HandleExitDebug, 0)}
		};

		public static void DefineCheatCode(string regex, Func<Match, string> cheatCodeAction, int protectionLevel) => cheatCodes.Set(new Regex(regex), new CheatCode(cheatCodeAction, protectionLevel));
		public static void SetListenToDebugCommand(bool enable) => Debug.onCommandSubmitted.SetListenerActive(HandleCheatCode, enable);

		public static void HandleCheatCode(string cmd) {
			cmd = cmd.ToLower().Trim();
			var matchingRegex = cheatCodes.FirstOrDefault(t => t.Key.IsMatch(cmd)).Key;
			if (matchingRegex == null) Debug.LogCheat($"{cmd}: Command not found");
			else if (!IsCheatCodeAllowed(cheatCodes[matchingRegex])) Debug.LogCheat($"{cmd}: You don't have the rights to use that command.");
			else Debug.LogCheat($"{cmd}: {cheatCodes[matchingRegex].solve(matchingRegex.Match(cmd))}");
		}

		private static bool IsCheatCodeAllowed(CheatCode cheatCode) => cheatCode.protectionLevel <= userAccessLevel;

		private static string HandleHelpCheatCode(Match regexMatch) {
			if (regexMatch.Groups.Count > 2) return "Error: Regex parameters not understood";
			if (regexMatch.Groups.Count == 1 || string.IsNullOrEmpty(regexMatch.Groups[1].Value))
				return $"\n{cheatCodes.Where(t => IsCheatCodeAllowed(t.Value)).Select(t => t.Value.name.WithLength(30, '.') + t.Key).OrderBy(t => t).Join("\n")}";
			var cheatCodeToExplain = cheatCodes.SingleOrDefault(t => IsCheatCodeAllowed(t.Value) && string.Equals(t.Value.name, regexMatch.Groups[1].Value, StringComparison.CurrentCultureIgnoreCase));
			if (cheatCodeToExplain.Key == null) return "Error: parameter is not a valid cheat-code name";
			return $"\nHelp \"{cheatCodeToExplain.Value.displayName}\" ({cheatCodeToExplain.Value.name}):\n{cheatCodeToExplain.Value.displayDescription}";
		}

		private static string HandleExitDebug(Match arg) {
			DebugUi.Toggle();
			return string.Empty;
		}
	}
}