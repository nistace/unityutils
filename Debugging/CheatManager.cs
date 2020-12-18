using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Utils.Extensions;

namespace Utils.Debugging {
	public static class CheatManager {
		public static  int          userAccessLevel     { get; set; }
		private static List<string> previousCommands    { get; } = new List<string>();
		private static int          repeatPreviousIndex { get; set; }

		private static Dictionary<Regex, CheatCode> cheatCodes { get; } = new Dictionary<Regex, CheatCode> {
			{new Regex("^ *help *(\\w+)? *$"), new CheatCode(HandleHelpCheatCode, 0)}, {new Regex("^ *exit-debug *(\\w+)? *$"), new CheatCode(HandleExitDebug, 0)}
		};

		public static void DefineCheatCode(string regex, Func<Match, string> cheatCodeAction, int protectionLevel) => cheatCodes.Set(new Regex(regex), new CheatCode(cheatCodeAction, protectionLevel));

		public static void SetListenToDebugCommand(bool enable) {
			DebugCanvas.onCommandSubmitted.SetListenerActive(HandleCheatCode, enable);
			DebugCanvas.onDownPressedInCommandInput.SetListenerActive(RepeatNextCommand, enable);
			DebugCanvas.onUpPressedInCommandInput.SetListenerActive(RepeatPreviousCommand, enable);
		}

		private static void RepeatPreviousCommand() {
			repeatPreviousIndex = (repeatPreviousIndex - 1).Clamp(0, previousCommands.Count);
			DebugCanvas.SetCommand(previousCommands.GetSafe(repeatPreviousIndex, string.Empty));
		}

		private static void RepeatNextCommand() {
			repeatPreviousIndex = (repeatPreviousIndex + 1).Clamp(0, previousCommands.Count);
			DebugCanvas.SetCommand(previousCommands.GetSafe(repeatPreviousIndex, string.Empty));
		}

		private static void HandleCheatCode(string cmd) {
			cmd = cmd.ToLower().Trim();
			previousCommands.Add(cmd);
			repeatPreviousIndex = previousCommands.Count;
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
			DebugCanvas.Toggle();
			return string.Empty;
		}
	}
}