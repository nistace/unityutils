using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.Aseprite {
	[CreateAssetMenu]
	public class AsepriteSheet : ScriptableObject {
		public enum Operation {
			SplitTexture,
			CreateRuleTiles
		}

		[SerializeField] protected Operation    _operation;
		[SerializeField] protected string       _pattern = "layer.tag.frame";
		[SerializeField] protected Texture2D    _texture;
		[SerializeField] protected TextAsset    _json;
		[SerializeField] protected float        _pixelsPerSprite = 16;
		[SerializeField] protected Vector2      _defaultPivot    = new Vector2(.5f, .5f);
		[SerializeField] protected LayerPivot[] _layerPivots;
		[SerializeField] protected Sprite[]     _sprites;

		public Operation             operation       => _operation;
		public float                 pixelsPerSprite => _pixelsPerSprite;
		public TextAsset             json            => _json;
		public Texture2D             texture         => _texture;
		public IReadOnlyList<Sprite> sprites         => _sprites;

		[Serializable] protected class LayerPivot {
			[SerializeField] protected string  _layerName;
			[SerializeField] protected Vector2 _pivot;

			public string  layerName => _layerName;
			public Vector2 pivot     => _pivot;
		}

		public Vector2 GetPivot(string name) => _layerPivots.FirstOrDefault(t => t.layerName == name)?.pivot ?? _defaultPivot;

		public void SetSprites(IEnumerable<Sprite> sprites) => _sprites = sprites.ToArray();

		public Sprite this[string name] => sprites.FirstOrDefault(t => t.name == name);
	}
}