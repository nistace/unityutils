using System.Collections.Generic;

namespace Utils.Ui.Tooltips {
	public interface ITooltipExternalSource {
		bool displayTooltip { get; }
		IReadOnlyDictionary<string, string> GetTooltipParameters();
	}
}