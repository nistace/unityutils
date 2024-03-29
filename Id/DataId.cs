﻿using System;
using System.Collections.Generic;
using System.Linq;
using NiUtils.Extensions;
using UnityEngine;
using Object = UnityEngine.Object;

namespace NiUtils.Id {
	[Serializable]
	public class DataId {
		[SerializeField] protected int _value;

		public int value {
			get => _value;
			set => _value = value;
		}

		public static int GetNextId() => GetNextIds(1)[0];

		private static int[] GetNextIds(int count) {
			var result = new int[count];
			var allTaken = new HashSet<int>();
			allTaken.AddAll(Resources.LoadAll<DataMonoBehaviour>("").Cast<IData>().Select(t => t.id));
			allTaken.AddAll(Resources.LoadAll<DataScriptableObject>("").Cast<IData>().Select(t => t.id));

			var maxTaken = allTaken.Count == 0 ? 1 : allTaken.Max();
			var allGaps = maxTaken.CreateArray(t => t + 1).Except(allTaken).ToArray();
			int i;
			for (i = 0; i < allGaps.Length && i < result.Length; ++i) {
				result[i] = allGaps[i];
			}
			while (i < result.Length) {
				result[i] = maxTaken + i + 1;
				i++;
			}
			return result;
		}

		public static IReadOnlyDictionary<int, E> FromResources<E>(string resourcesDataPath) where E : Object, IData => Resources.LoadAll<E>(resourcesDataPath).ToDictionary(t => t.id, t => t);
		public static IReadOnlyList<E> ListFromResources<E>(string resourcesDataPath) where E : Object, IData => Resources.LoadAll<E>(resourcesDataPath).OrderBy(t => t.id).ToList();

		public static bool Equals<E>(E first, E second) where E : IData => first?.id == second?.id;
	}
}