﻿using UnityEngine;
using UnityEngine.UI;

namespace Utils.Ui {
	public class ImageAnimator : MonoBehaviour {
		[SerializeField] protected Image    _image;
		[SerializeField] protected float[]  _delays;
		[SerializeField] protected Sprite[] _sprites;
		[SerializeField] protected int      _currentFrameIndex;
		[SerializeField] protected float    _currentFrameTime;

		public Image image => _image;

		private void Reset() {
			_image = GetComponent<Image>();
			_delays = new[] {1f};
			_sprites = new[] {_image?.sprite};
		}

		private void Update() {
			if (!_image) return;
			if (_delays.Length < 2) return;
			if (_sprites.Length < 2) return;

			_currentFrameTime += Time.deltaTime;
			if (_currentFrameTime < _delays.GetSafe(_currentFrameIndex)) return;
			_currentFrameIndex++;
			_currentFrameIndex %= _sprites.Length;
			_image.sprite = _sprites[_currentFrameIndex];
			_currentFrameTime = 0;
		}

		private void OnEnable() {
			_currentFrameIndex = 0;
			_currentFrameTime = 0;
			if (_sprites.Length > 0) _image.sprite = _sprites[_currentFrameIndex];
		}
	}
}