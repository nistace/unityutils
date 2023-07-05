using NiUtils.Extensions;
using UnityEngine.UI;

namespace NiUtils.Ui.Tooltips {
	public class ButtonWithTooltip : Button {
		private FixedTooltipHolder sTooltip { get; set; }
		public  FixedTooltipHolder tooltip  => sTooltip ? sTooltip : (sTooltip = gameObject.GetOrAddComponent<FixedTooltipHolder>());
	}
}