using System.Collections.Generic;
using NiUtils.Extensions;
using UnityEngine;

namespace NiUtils.Ui.Tooltips {
	public class TooltipOverlayUi : MonoBehaviour {
		private const  string           defaultTooltipName = "_DEFAULT";
		private static TooltipOverlayUi instance { get; set; }

		[SerializeField]                             protected TooltipUi _defaultTooltip;
		[Header("Fade properties")] [SerializeField] protected float     _fadeInDelay  = 1;
		[SerializeField]                             protected float     _fadeOutDelay = .5f;
		[SerializeField]                             protected float     _fadeSpeed    = 1.5f;

		private float opacityChangePerSecond => _fadeSpeed;
		private float minOpacity             => -_fadeInDelay * _fadeSpeed;
		private float maxOpacity             => 1 + _fadeOutDelay * _fadeSpeed;
		private float targetOpacity          { get; set; }
		private float currentOpacity         { get; set; }

		private List<(TooltipUi ui, TooltipData data)> tooltipStack   { get; } = new List<(TooltipUi, TooltipData)>();
		private Dictionary<string, TooltipUi>          tooltipModels  { get; } = new Dictionary<string, TooltipUi>();
		private TooltipUi                              currentTooltip { get; set; }

		private void Awake() {
			if (instance == null) instance = this;
			if (instance != this) Destroy(gameObject);
			else {
				tooltipModels.Add(_defaultTooltip.name, _defaultTooltip);
				tooltipModels.Add(defaultTooltipName, _defaultTooltip);
			}
		}

		private void Start() {
			RefreshContent(true);
		}

		private void OnDestroy() {
			if (instance == this) instance = null;
		}

		private TooltipUi GetTooltipUi(TooltipUi model) {
			if (!model) return tooltipModels[defaultTooltipName];
			if (tooltipModels.ContainsKey(model.name)) return tooltipModels[model.name];
			var newTooltipModelInstance = Instantiate(model, transform);
			newTooltipModelInstance.name = model.name;
			tooltipModels.Add(model.name, newTooltipModelInstance);
			return newTooltipModelInstance;
		}

		public static void Show(TooltipUi tooltipModel, TooltipData data) {
			if (data.isNullOrEmpty) return;
			instance.tooltipStack.Insert(0, (instance.GetTooltipUi(tooltipModel), data));
			instance.RefreshContent(false);
		}

		public static void Hide(TooltipData data) {
			instance.tooltipStack.RemoveWhere(t => Equals(t.data, data));
			instance.RefreshContent(false);
		}

		private void RefreshContent(bool instantaneous) {
			tooltipModels.Values.ForEach(t => t.RefreshGraphicsOpacity(0));
			if (tooltipStack.Count > 0) {
				currentTooltip = tooltipStack[0].ui;
				currentTooltip.Set(tooltipStack[0].data);
			}
			targetOpacity = tooltipStack.Count > 0 ? maxOpacity : minOpacity;
			if (instantaneous) currentOpacity = targetOpacity;
			currentTooltip?.RefreshGraphicsOpacity(currentOpacity);
		}

		private void Update() {
			if (!currentTooltip) return;
			if (currentOpacity == targetOpacity) return;
			if (currentOpacity < targetOpacity) currentOpacity = Mathf.Min(targetOpacity, currentOpacity + opacityChangePerSecond * Time.deltaTime);
			else if (currentOpacity > targetOpacity) currentOpacity = Mathf.Max(targetOpacity, currentOpacity - opacityChangePerSecond * Time.deltaTime);
			currentTooltip.RefreshGraphicsOpacity(currentOpacity);
		}
	}
}