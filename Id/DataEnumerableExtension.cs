using System.Collections.Generic;
using System.Linq;

namespace NiUtils.Id {
	public static class DataEnumerableExtension {
		public static E SingleOrDefault<E>(this IEnumerable<E> items, int id) where E : IData => items.SingleOrDefault(t => t.id == id);
	}
}