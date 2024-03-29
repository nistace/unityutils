﻿using UnityEngine;

namespace NiUtils.Extensions {
	public static class ParticleSystemExtension {
		public static void SetPlaying(this ParticleSystem particleSystem, bool play) {
			if (play) particleSystem.Play();
			else particleSystem.Stop();
		}
	}
}