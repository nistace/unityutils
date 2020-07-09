﻿public struct LocalisationMessage {
	private string   key        { get; }
	private object[] parameters { get; }
	public  string   display    => Localisation.Map(key, parameters);
	public  bool     exists     => !string.IsNullOrEmpty(key);

	public LocalisationMessage(string key, params object[] parameters) {
		this.key = key;
		this.parameters = parameters;
	}

	public static implicit operator LocalisationMessage(string key) => new LocalisationMessage(key);
	public static implicit operator LocalisationMessage((string key, object parameter) group) => new LocalisationMessage(group.key, group.parameter);
	public static implicit operator LocalisationMessage((string key, object[] parameters) group) => new LocalisationMessage(group.key, group.parameters);

	public LocalisationMessage With(string key) => new LocalisationMessage(key, parameters);

	public override string ToString() => display;
}