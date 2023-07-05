using System.Collections.Generic;
using UnityEngine;

namespace NiUtils.Aseprite.Editor {
	public static class AsepriteTileSets {
		public const string defaultNeighbourDescriptor = "xxxxxxxx";

		public static IReadOnlyList<string> neighbourDescriptors { get; } = new[] {
			"x0x111x0",
			"x0x11111",
			"x0x0x111",
			"x0x0x1x0",
			"11110111",
			"11111101",
			"x11101x0",
			"11x0x101",
			"x0x10101",
			"01x0x101",
			"01011101",
			"11011101",
			"x11111x0",
			"11111111",
			"11x0x111",
			"x1x0x1x0",
			"11011111",
			"01111111",
			"x10111x0",
			"01x0x111",
			"x10101x0",
			"0101x0x1",
			"01110101",
			"01110111",
			"x111x0x0",
			"1111x0x1",
			"11x0x0x1",
			"x1x0x0x0",
			"x0x10111",
			"x0x11101",
			"x0x101x0",
			"x0x0x101",
			"11110101",
			"01111101",
			"11010101",
			"01010101",
			"x0x1x0x0",
			"x0x1x0x1",
			"x0x0x0x1",
			"x0x0x0x0",
			"1101x0x1",
			"0111x0x1",
			"x101x0x0",
			"01x0x0x1",
			"11010111",
			"01011111",
			"01010111",
			defaultNeighbourDescriptor
		};

		public static IReadOnlyList<Vector3Int> orderedDirections { get; } = new[] {
			new Vector3Int(-1, 1, 0),
			new Vector3Int(0, 1, 0),
			new Vector3Int(1, 1, 0),
			new Vector3Int(1, 0, 0),
			new Vector3Int(1, -1, 0),
			new Vector3Int(0, -1, 0),
			new Vector3Int(-1, -1, 0),
			new Vector3Int(-1, 0, 0)
		};

		public static bool TryGetNeighbour(string neighbourDescriptor, int charIndex, out int neighbour) {
			neighbour = default;
			if (neighbourDescriptor.Length <= charIndex) return false;
			if (neighbourDescriptor[charIndex] == '1') {
				neighbour = 1;
				return true;
			}
			if (neighbourDescriptor[charIndex] == '0') {
				neighbour = 2;
				return true;
			}
			return false;
		}
	}
}