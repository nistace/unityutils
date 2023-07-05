#if ENABLE_2D
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.EditorCoroutines.Editor;
using UnityEditor;
using UnityEngine;

namespace NiUtils.Aseprite.Editor {
	public class AsepriteSheetBuilder : EditorWindow {
		private static AsepriteSheet config { get; set; }

		[MenuItem("Sprites/Aseprite Sheet Builder")]
		private static void GetWindow() => GetWindow(typeof(AsepriteSheetBuilder), false, "Aseprite Sheet Builder", true).Show();

		private void OnGUI() {
			config = (AsepriteSheet)EditorGUILayout.ObjectField(config, typeof(AsepriteSheet), false);

			if (config && GetProcessAction() != null && GUILayout.Button("Process")) {
				EditorCoroutineUtility.StartCoroutine(GetProcessAction()(), this);
			}
		}

		private static Func<IEnumerator> GetProcessAction() {
			return config.operation switch {
				AsepriteSheet.Operation.SplitTexture    => DoSplitSheet,
				AsepriteSheet.Operation.CreateRuleTiles => DoSplitInRuleTiles,
				_                                       => null
			};
		}

		private static IEnumerator DoSplitSheet() {
			yield return null;
			SplitTexture(t => $"{t.Split('.')[0]}.{t.Split('.')[1]}.{int.Parse(t.Split('.')[2]):000}");
			yield return null;
		}

		private static IEnumerator DoSplitInRuleTiles() {
			yield return null;
			SplitTexture(t => $"{t.Split('.')[0]}-{AsepriteTileSets.neighbourDescriptors[int.Parse(t.Split('.')[2])]}");
			yield return null;
			CreateRuleTile();
			yield return null;
		}

		private static void CreateRuleTile() {
			var texturePath = AssetDatabase.GetAssetPath(config.texture);
			var ruleTilePathRoot = $"{texturePath[..texturePath.LastIndexOf("/", StringComparison.Ordinal)]}";
			var ruleTilePrefix = $"{texturePath[(texturePath.LastIndexOf("/", StringComparison.Ordinal) + 1)..texturePath.LastIndexOf(".", StringComparison.Ordinal)]}-";

			var ruleTiles = new Dictionary<string, RuleTile>();
			foreach (var existingRuleTile in Directory.GetFiles(texturePath[..texturePath.LastIndexOf("/", StringComparison.Ordinal)]).Select(AssetDatabase.LoadAssetAtPath<ScriptableObject>)
							.OfType<RuleTile>()) {
				EditorUtility.SetDirty(existingRuleTile);
				existingRuleTile.m_TilingRules.Clear();
				ruleTiles.Add(existingRuleTile.name, existingRuleTile);
				Debug.Log($"Added {existingRuleTile.name}");
			}

			foreach (var sprite in AssetDatabase.LoadAllAssetsAtPath(texturePath).OfType<Sprite>().OrderBy(t => t.name).ToArray()) {
				var ruleTileName = $"{ruleTilePrefix}{sprite.name[..sprite.name.LastIndexOf("-", StringComparison.Ordinal)]}";
				var spriteNeighbourDescriptor = sprite.name[(sprite.name.LastIndexOf("-", StringComparison.Ordinal) + 1)..];
				if (!ruleTiles.ContainsKey(ruleTileName)) {
					var ruleTile = CreateInstance<RuleTile>();
					ruleTiles.Add(ruleTileName, ruleTile);
					AssetDatabase.CreateAsset(ruleTile, $"{Path.Combine(ruleTilePathRoot, ruleTileName)}.asset");
				}
				var positions = new List<Vector3Int>();
				var neighbours = new List<int>();
				for (var directionIndex = 0; directionIndex < AsepriteTileSets.orderedDirections.Count; directionIndex++) {
					if (AsepriteTileSets.TryGetNeighbour(spriteNeighbourDescriptor, directionIndex, out var neighbour)) {
						positions.Add(AsepriteTileSets.orderedDirections[directionIndex]);
						neighbours.Add(neighbour);
					}
				}
				if (spriteNeighbourDescriptor == AsepriteTileSets.defaultNeighbourDescriptor) ruleTiles[ruleTileName].m_DefaultSprite = sprite;
				ruleTiles[ruleTileName].m_TilingRules.Add(new RuleTile.TilingRule { m_NeighborPositions = positions, m_Neighbors = neighbours, m_Sprites = new[] { sprite } });
				EditorUtility.SetDirty(ruleTiles[ruleTileName]);
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			EditorUtility.FocusProjectWindow();
			Selection.activeObject = config.texture;
		}

		private static void SplitTexture(Func<string, string> getSpriteName) {
			var path = AssetDatabase.GetAssetPath(config.texture);
			var textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
			textureImporter.isReadable = true;
			textureImporter.spriteImportMode = SpriteImportMode.Multiple;
			textureImporter.spritePixelsPerUnit = config.pixelsPerSprite;
			textureImporter.alphaSource = TextureImporterAlphaSource.FromInput;
			textureImporter.alphaIsTransparency = true;
			textureImporter.filterMode = FilterMode.Point;
			textureImporter.wrapMode = TextureWrapMode.Clamp;
			textureImporter.compressionQuality = 0;

			textureImporter.spritesheet = AsepriteSheetDescriptor.FromJson(config.json).frames.Select(frame =>
				new SpriteMetaData { pivot = config.GetPivot(frame.filename.Split('.')[0]), alignment = 9, name = getSpriteName(frame.filename), rect = frame.GetRect(config.texture.height) }).ToArray();
			AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
			Undo.RecordObject(config, "Update sprite list");
			config.SetSprites(AssetDatabase.LoadAllAssetsAtPath(path).Where(t => t is Sprite).Cast<Sprite>().OrderBy(t => t.name));
			EditorUtility.SetDirty(config);
		}
	}
}
#endif