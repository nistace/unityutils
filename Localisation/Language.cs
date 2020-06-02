using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Constants/Language")]
public class Language : DataScriptableObject {
	[SerializeField] protected string           _iso;
	[SerializeField] protected TextAsset        _textAsset;
	[SerializeField] protected bool             _alwaysLoad;
	[SerializeField] protected bool             _defaultLanguage;
	[SerializeField] protected SystemLanguage[] _languages;

	public TextAsset                   textAsset       => _textAsset;
	public bool                        alwaysLoad      => _alwaysLoad;
	public string                      iso             => _iso;
	public IEnumerable<SystemLanguage> languages       => _languages;
	public bool                        defaultLanguage => _defaultLanguage;
}