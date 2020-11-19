using TMPro;
using UnityEngine;
using UnityEngine.Events;
using Utils.Events;
using Utils.Types.Ui;

public class DebugUi : MonoBehaviour {
	private static DebugUi instance { get; set; }

	[SerializeField] protected DebugLineUi            _linePrefab;
	[SerializeField] protected Transform              _linesContainer;
	[SerializeField] protected ScrollRectAutoScroller _autoScroller;
	[SerializeField] protected int                    _autoScrollerUpdateFrames = 5;
	[SerializeField] protected TMP_InputField         _commandInput;
	[SerializeField] protected bool                   _refocusOnSubmit = true;

	public static StringEvent onCommandSubmitted          { get; } = new StringEvent();
	public static BoolEvent   onDisplayedChanged          { get; } = new BoolEvent();
	public static UnityEvent  onUpPressedInCommandInput   { get; } = new UnityEvent();
	public static UnityEvent  onDownPressedInCommandInput { get; } = new UnityEvent();

	private void Awake() {
		if (instance) Destroy(gameObject);
		else instance = this;
		DontDestroyOnLoad(transform.root.gameObject);
	}

	private void Start() => gameObject.SetActive(false);

	private void OnEnable() => SetListeners(true);

	private void OnDisable() => SetListeners(false);

	private void SetListeners(bool enabled) {
		if (!_commandInput) return;

		_commandInput.onSubmit.SetListenerActive(HandleSubmitCommand, enabled);
	}

	private void HandleSubmitCommand(string command) {
		if (!_commandInput.wasCanceled && command.Trim().Length > 0) {
			onCommandSubmitted.Invoke(command.Trim());
		}
		_commandInput.SetTextWithoutNotify(string.Empty);
		if (_refocusOnSubmit) _commandInput.ActivateInputField();
	}

	public static void Print(string info, string type, Color color) {
		if (instance && instance._autoScroller && instance._autoScroller.atBottom) instance._autoScroller.ScrollToBottom(instance._autoScrollerUpdateFrames);
		var line = Instantiate(instance._linePrefab, instance._linesContainer);
		line.Set(info, type);
		line.color = color;
	}

	public static void Toggle() {
		instance.gameObject.SetActive(!instance.gameObject.activeSelf);
		if (instance.gameObject.activeSelf && instance._commandInput) instance._commandInput.ActivateInputField();
		onDisplayedChanged.Invoke(instance.gameObject.activeSelf);
	}

	private void Update() {
		if (!_commandInput.isActiveAndEnabled) return;
		if (Input.GetKeyDown(KeyCode.UpArrow)) onUpPressedInCommandInput.Invoke();
		if (Input.GetKeyDown(KeyCode.DownArrow)) onDownPressedInCommandInput.Invoke();
	}

	public static void SetCommand(string cmd) {
		instance._commandInput.text = cmd;
		instance._commandInput.caretPosition = cmd.Length;
	}
}