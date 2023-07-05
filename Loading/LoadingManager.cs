using System.Collections;
using UnityEngine;

namespace NiUtils.Loading {
	public class LoadingManager : MonoBehaviour {
		private static LoadingManager instance       { get; set; }
		private static LoadingProcess loadingProcess { get; set; }
		private static LoadingCanvas  loadingUi      => instance._loadingUi;
		public static  float          progress       => loadingProcess?.progress ?? 0;

		[SerializeField] protected LoadingCanvas _loadingUi;
		[SerializeField] protected bool          _visibleOnStart;

		private void Awake() {
			if (instance == null) instance = this;
			if (instance != this) Destroy(this);
		}

		private void Start() {
			LoadingCanvas.onFadeInComplete.AddListener(PlayLoadingProcess);
			if (_visibleOnStart) _loadingUi.SetVisible();
			else _loadingUi.SetHidden();
		}

		public static void Load(LoadingProcess process) {
			loadingProcess = process;
			loadingProcess.coroutineRunner = instance;
			loadingUi.Show();
		}

		private void PlayLoadingProcess() => StartCoroutine(DoPlayLoadingProcess());

		private static IEnumerator DoPlayLoadingProcess() {
			yield return loadingProcess.coroutineRunner.StartCoroutine(loadingProcess.routine);
			while (!loadingProcess.isDone) loadingProcess.ForceCompletion();
			yield return null;
			instance._loadingUi.Hide();
		}
	}
}