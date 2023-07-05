using System.Collections.Generic;

namespace NiUtils.Ui.Tooltips {
	public interface ITooltipExternalSource {
		bool displayTooltip { get; }
		IReadOnlyDictionary<string, string> GetTooltipParameters();
	}
}