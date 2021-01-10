using Photon.Pun;
using Utils.Extensions;

namespace Utils.Multi.Chat {
	public class OnlineChatter : MonoBehaviourMyPun {
		public void SendMessageToChat(string message) => this.RpcAll(RpcSendMessage, message);

		[PunRPC] private void RpcSendMessage(string message) => OnlineChatEventSystem.onMessageSent.Invoke(photonView.Owner.NickName, message);
	}
}