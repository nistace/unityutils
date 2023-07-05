#if PHOTON_UNITY_NETWORKING
using NiUtils.Pun.Extensions;
using Photon.Pun;

namespace NiUtils.Pun.Chat {
	public class OnlineChatter : MonoBehaviourMyPun {
		public void SendMessageToChat(string message) => this.Rpc(RpcTarget.All, RpcSendMessage, message);

		[PunRPC] private void RpcSendMessage(string message) => OnlineChatEventSystem.onMessageSent.Invoke(photonView.Owner.NickName, message);
	}
}
#endif