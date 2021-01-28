using UnityEngine.UI;
using Utils.Extensions;

namespace Utils.Ui.Tooltips {
	public class ButtonWithTooltip : Button {
		private FixedTooltipHolder sTooltip { get; set; }
		public  FixedTooltipHolder tooltip  => sTooltip ? sTooltip : (sTooltip = gameObject.GetOrAddComponent<FixedTooltipHolder>());
	}
}