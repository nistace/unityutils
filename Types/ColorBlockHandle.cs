using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ColorBlockHandle {
	[SerializeField] protected ColorBlock _colorBlock;

	public static implicit operator ColorBlock(ColorBlockHandle handle) => handle._colorBlock;
	public static implicit operator ColorBlockHandle(ColorBlock colorBlock) => new ColorBlockHandle {_colorBlock = colorBlock};
}