using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Utils.Debugging;
using Utils.Events;
using Utils.Extensions;
using Utils.Types.Ui;
using Utils.Ui;

[RequireComponent(typeof(Canvas))]
public class DebugCanvas : MonoBehaviourUi {
	private static DebugCanvas instance { get; set; }

	[SerializeField] protected DebugLineUi            _linePrefab;
	[SerializeField] protected Transform              _linesContainer;
	[SerializeField] protected ScrollRectAutoScroller _autoScroller;
	[SerializeField] protected TMP_InputField         _commandInput;
	[SerializeField] protected bool                   _refocusOnSubmit = true;

	public static StringEvent   onCommandSubmitted { get; } = new StringEvent();
	public static BoolEvent     onDisplayedChanged { get; } = new BoolEvent();
	public static UnityEvent    onPreviousPressed  { get; } = new UnityEvent();
	public static UnityEvent    onNextPressed      { get; } = new UnityEvent();
	private       DebugControls controls           { get; set; }

	private void Awake() {
		if (instance) Destroy(gameObject);
		else instance = this;
		DontDestroyOnLoad(transform.root.gameObject);
		GetComponent<Canvas>().enabled = true;
		controls = new DebugControls();
	}

	private void Start() => gameObject.SetActive(false);

	private void OnEnable() => SetListeners(true);

	private void OnDisable() => SetListeners(false);

	private void SetListeners(bool enabled) {
		if (!_commandInput) return;

		_commandInput.onSubmit.SetListenerActive(HandleSubmitCommand, enabled);
		_commandInput.onSelect.SetListenerActive(s => SetCommandInputListenersEnabled(true), enabled);
		_commandInput.onDeselect.SetListenerActive(s => SetCommandInputListenersEnabled(false), enabled);
	}

	private void SetCommandInputListenersEnabled(bool enabled) {
		controls.Common.Next.AddPerformListenerOnce(HandleSelectNextCommand);
		controls.Common.Previous.AddPerformListenerOnce(HandleSelectPreviousCommand);
		controls.Common.Next.SetEnabled(enabled);
		controls.Common.Previous.SetEnabled(enabled);
	}

	private static void HandleSelectNextCommand(InputAction.CallbackContext obj) => onNextPressed.Invoke();
	private static void HandleSelectPreviousCommand(InputAction.CallbackContext obj) => onPreviousPressed.Invoke();

	private void HandleSubmitCommand(string command) {
		if (!_commandInput.wasCanceled && command.Trim().Length > 0) {
			onCommandSubmitted.Invoke(command.Trim());
		}
		_commandInput.SetTextWithoutNotify(string.Empty);
		if (_refocusOnSubmit) _commandInput.ActivateInputField();
	}

	public static void Print(string info, string type, Color color) {
		if (!instance) return;
		if (instance && instance._autoScroller && instance._autoScroller.atBottom) instance._autoScroller.ScrollToBottom();
		var line = Instantiate(instance._linePrefab, instance._linesContainer);
		line.Set(info, type);
		line.color = color;
	}

	public static void Toggle() {
		instance.gameObject.SetActive(!instance.gameObject.activeSelf);
		if (instance.gameObject.activeSelf && instance._commandInput) instance._commandInput.ActivateInputField();
		onDisplayedChanged.Invoke(instance.gameObject.activeSelf);
	}

	public static void SetCommand(string cmd) {
		instance._commandInput.text = cmd;
		instance._commandInput.caretPosition = cmd.Length;
	}
}