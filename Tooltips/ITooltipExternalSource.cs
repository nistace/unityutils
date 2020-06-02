using System.Collections.Generic;

public interface ITooltipExternalSource {
	bool displayTooltip { get; }
	IReadOnlyDictionary<string, string> GetTooltipParameters();
}