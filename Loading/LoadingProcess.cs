using System;
using System.Collections;
using NiUtils.Types;
using UnityEngine;

namespace NiUtils.Loading {
	public abstract class LoadingProcess {
		public abstract IEnumerator   routine            { get; }
		public          Ratio         progress           { get; private set; }
		public          string        progressStatusText { get; private set; }
		public          bool          isDone             => progress == 1;
		public          MonoBehaviour coroutineRunner    { get; set; }

		protected object Progress(float progress, Action action = null, string progressStatusText = null) {
			action?.Invoke();
			this.progress = progress;
			if (progressStatusText != null) this.progressStatusText = progressStatusText;
			return null;
		}

		public void ForceCompletion() => progress = 1;
	}
}