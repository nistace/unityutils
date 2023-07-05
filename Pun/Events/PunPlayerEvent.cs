#if PHOTON_UNITY_NETWORKING
using Photon.Realtime;
using UnityEngine.Events;

namespace NiUtils.Pun.Events {
	public class PunPlayerEvent : UnityEvent<Player> { }
}
#endif