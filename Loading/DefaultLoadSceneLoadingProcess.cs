using System.Collections;
using UnityEngine.SceneManagement;

namespace NiUtils.Loading {
	public class DefaultLoadSceneLoadingProcess : LoadingProcess {
		public override IEnumerator routine => Do();

		private string sceneName { get; }

		public DefaultLoadSceneLoadingProcess(string sceneName) {
			this.sceneName = sceneName;
		}

		private IEnumerator Do() {
			var operation = SceneManager.LoadSceneAsync(sceneName);
			while (!operation.isDone) yield return Progress(operation.progress);
		}
	}
}