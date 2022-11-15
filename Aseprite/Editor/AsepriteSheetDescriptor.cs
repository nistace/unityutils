using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;

namespace Utils.Aseprite.Editor {
	[Serializable]
	public class AsepriteSheetDescriptor {
		[JsonProperty("frames"), SerializeField] protected Frame[] _frames = Array.Empty<Frame>();

		[JsonIgnore] public Frame[] frames          => _frames;
		[JsonIgnore] public string  filenamePattern { get; set; } = "layer.tag.frameIndex";

		public static AsepriteSheetDescriptor FromJson(TextAsset jsonFile) => JsonConvert.DeserializeObject<AsepriteSheetDescriptor>(jsonFile.text);

		public IEnumerable<string> GetDistinctLayers() => frames.Select(t => t.filename.Split('.')[0]).Distinct();

		[Serializable]
		public class Frame {
			[JsonProperty("filename"), SerializeField] protected string    _filename;
			[JsonProperty("frame"), SerializeField]    protected FrameRect _frame;

			[JsonIgnore] public string filename => _filename;

			public Rect GetRect(int textureHeight) => new Rect(_frame.x, textureHeight - _frame.h - _frame.y, _frame.w, _frame.h);

			[Serializable]
			public class FrameRect {
				[JsonProperty("x"), SerializeField] protected int _x;
				[JsonProperty("y"), SerializeField] protected int _y;
				[JsonProperty("w"), SerializeField] protected int _w;
				[JsonProperty("h"), SerializeField] protected int _h;

				[JsonIgnore] public int x => _x;
				[JsonIgnore] public int y => _y;
				[JsonIgnore] public int w => _w;
				[JsonIgnore] public int h => _h;
			}
		}
	}
}