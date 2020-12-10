using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Utils.Extensions {
	public static class EventTriggerExtension {
		public static void AddListener(this EventTrigger et, EventTriggerType eventType, UnityAction<BaseEventData> func) {
			var entry = new EventTrigger.Entry {eventID = eventType};
			entry.callback.AddListener(func);
			et.triggers.Add(entry);
		}
	}
}