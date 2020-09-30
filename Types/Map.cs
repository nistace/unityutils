using System.Collections;
using System.Collections.Generic;

namespace Utils.Types {
	public class Map<T1, T2> : ICollection<KeyValuePair<T1, T2>> {
		private IDictionary<T1, T2> lefts  { get; } = new Dictionary<T1, T2>();
		private IDictionary<T2, T1> rights { get; } = new Dictionary<T2, T1>();

		public int  Count      => lefts.Count;
		public bool IsReadOnly => false;

		public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator() => lefts.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public void Add(KeyValuePair<T1, T2> item) => Set(item.Key, item.Value);

		public T2 RightOf(T1 left) => lefts[left];
		public T1 LeftOf(T2 right) => rights[right];

		public void Clear() {
			lefts.Clear();
			rights.Clear();
		}

		public bool Contains(KeyValuePair<T1, T2> item) => lefts.ContainsKey(item.Key) && lefts[item.Key].Equals(item.Value);
		public void CopyTo(KeyValuePair<T1, T2>[] array, int arrayIndex) => lefts.CopyTo(array, arrayIndex);

		public bool Remove(KeyValuePair<T1, T2> item) {
			if (rights.ContainsKey(item.Value)) rights.Remove(item.Value);
			return lefts.Remove(item);
		}

		public void Set(T1 left, T2 right) {
			if (ContainsLeft(left)) RemoveLeft(left);
			if (ContainsRight(right)) RemoveRight(right);
			lefts[left] = right;
			rights[right] = left;
		}

		public bool ContainsLeft(T1 left) => lefts.ContainsKey(left);
		public bool ContainsRight(T2 right) => rights.ContainsKey(right);
		public bool TryGetRight(T1 left, out T2 right) => lefts.TryGetValue(left, out right);
		public bool TryGetLeft(T2 right, out T1 left) => rights.TryGetValue(right, out left);

		public bool RemoveLeft(T1 left) {
			if (!lefts.ContainsKey(left)) return false;
			rights.Remove(lefts[left]);
			lefts.Remove(left);
			return true;
		}

		public bool RemoveRight(T2 right) {
			if (!rights.ContainsKey(right)) return false;
			return RemoveLeft(rights[right]);
		}
	}
}