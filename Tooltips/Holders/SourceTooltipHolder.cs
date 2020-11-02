using UnityEngine;

public class SourceTooltipHolder : TooltipHolder {
	[SerializeField] protected TooltipUi     _tooltipModel;
	[SerializeField] protected MonoBehaviour _dataSource;

	private            ITooltipExternalSource dataSource { get; set; }
	protected override TooltipUi              uiModel    => _tooltipModel;

	protected override void Awake() {
		base.Awake();
		dataSource = _dataSource as ITooltipExternalSource ?? _dataSource?.GetComponent<ITooltipExternalSource>();
	}

	protected override TooltipData GetShowData() {
		if (dataSource == null) {
			Debug.LogWarning($"Tooltip holder with no source: {name}");
			return null;
		}
		if (!dataSource.displayTooltip) return null;
		return new TooltipData(rectTransform, dataSource.GetTooltipParameters());
	}
}