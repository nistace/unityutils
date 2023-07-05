using UnityEngine.Events;

namespace NiUtils.Pun.Chat {
	public static class OnlineChatEventSystem {
		public class MessageEvent : UnityEvent<string, string> { }

		public static MessageEvent onMessageSent { get; } = new MessageEvent();
	}
}