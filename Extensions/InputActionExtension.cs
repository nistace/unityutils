using System;
using UnityEngine.InputSystem;

public static class InputActionExtension {
	public static void AddPerformListenerOnce(this InputAction action, Action<InputAction.CallbackContext> context) {
		action.performed -= context;
		action.performed += context;
	}

	public static void SetPerformListenerOnce(this InputAction action, Action<InputAction.CallbackContext> context, bool enable) {
		if (enable) action.AddPerformListenerOnce(context);
		else action.RemovePerformListener(context);
	}

	public static void SetCancelListenerOnce(this InputAction action, Action<InputAction.CallbackContext> context, bool enable) {
		if (enable) action.AddCancelListenerOnce(context);
		else action.RemoveCancelListener(context);
	}

	public static void SetAnyListenerOnce(this InputAction action, Action<InputAction.CallbackContext> context, bool enable) {
		if (enable) action.AddAnyListenerOnce(context);
		else action.RemoveAnyListener(context);
	}

	public static void AddCancelListenerOnce(this InputAction action, Action<InputAction.CallbackContext> context) {
		action.canceled -= context;
		action.canceled += context;
	}

	public static void AddAnyListenerOnce(this InputAction action, Action<InputAction.CallbackContext> context) {
		action.AddPerformListenerOnce(context);
		action.AddCancelListenerOnce(context);
	}

	public static void RemovePerformListener(this InputAction action, Action<InputAction.CallbackContext> context) {
		action.performed -= context;
	}

	public static void RemoveCancelListener(this InputAction action, Action<InputAction.CallbackContext> context) {
		action.canceled -= context;
	}

	public static void RemoveAnyListener(this InputAction action, Action<InputAction.CallbackContext> context) {
		action.RemovePerformListener(context);
		action.RemoveCancelListener(context);
	}

	public static void SetEnabled(this InputAction action, bool enabled) {
		if (enabled) action.Enable();
		else action.Disable();
	}

	public static void SetEnabled(this IInputActionCollection collection, bool enabled) {
		if (enabled) collection.Enable();
		else collection.Disable();
	}

	public static string LocalisedKeyName(this InputAction inputAction, int nonCompositeBindingIndex = 0) {
		var control = inputAction.GetControl(inputAction.GetNonCompositeBinding(nonCompositeBindingIndex));
		return Localisation.TryMap($"input.key.{control.name}", out var l10NUnique) ? l10NUnique : control.displayName;
	}

	public static InputControl GetControl(this InputAction inputAction, InputBinding binding) {
		return inputAction.controls[inputAction.IndexOfNonCompositeBinding(binding)];
	}

	public static InputBinding GetNonCompositeBinding(this InputAction inputAction, int index = 0) {
		foreach (var binding in inputAction.bindings) {
			if (binding.isComposite) continue;
			if (index == 0) return binding;
			index--;
		}
		throw new IndexOutOfRangeException($"inputAction {inputAction.name} does not have enough non-composite bindings. Index {index} requested.");
	}

	public static int IndexOfNonCompositeBinding(this InputAction inputAction, InputBinding binding) {
		var index = -1;
		foreach (var inputBinding in inputAction.bindings) {
			if (inputBinding.isComposite) continue;
			index++;
			if (binding == inputBinding) return index;
		}
		return -1;
	}

	public static int AbsoluteIndexOf(this InputAction inputAction, InputBinding binding) => inputAction.bindings.IndexOf(t => t == binding);

	public static int ToAbsoluteIndex(this InputAction inputAction, int nonCompositeIndex) => inputAction.AbsoluteIndexOf(inputAction.GetNonCompositeBinding(nonCompositeIndex));
}