using System.Collections.Generic;
using GorillaNetworking;
using Photon.Pun;
using Photon.Realtime;

namespace WalkSim.Patches;

public class UtillaNetworkController : MonoBehaviourPunCallbacks
{
	public static Events events = new Events();

	private Events.RoomJoinedArgs lastRoom;

	public override void OnJoinedRoom()
	{
		bool isPrivate = false;
		string text = "";
		if (PhotonNetwork.CurrentRoom != null)
		{
			Room currentRoom = PhotonNetwork.NetworkingClient.CurrentRoom;
			isPrivate = !currentRoom.IsVisible || ((Dictionary<object, object>)(object)((RoomInfo)currentRoom).CustomProperties).ContainsKey((object)"Description");
			if (((Dictionary<object, object>)(object)((RoomInfo)currentRoom).CustomProperties).TryGetValue((object)"gameMode", out object value))
			{
				text = value as string;
			}
		}
		string text2 = "";
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{ "INFECTION", "INFECTION" },
			{ "CASUAL", "CASUAL" },
			{ "HUNT", "HUNT" },
			{ "BATTLE", "PAINTBRAWL" }
		};
		foreach (KeyValuePair<string, string> item in dictionary)
		{
			if (text.Contains(item.Key))
			{
				text2 = item.Value;
				break;
			}
		}
		((GorillaComputer)GorillaComputer.instance).currentGameModeText.Value = "CURRENT MODE\n" + text2;
		Events.RoomJoinedArgs e = new Events.RoomJoinedArgs
		{
			isPrivate = isPrivate,
			Gamemode = text
		};
		events.TriggerRoomJoin(e);
		lastRoom = e;
	}

	public override void OnLeftRoom()
	{
		if (lastRoom != null)
		{
			events.TriggerRoomLeft(lastRoom);
			lastRoom = null;
		}
		((GorillaComputer)GorillaComputer.instance).currentGameModeText.Value = "CURRENT MODE\n-NOT IN ROOM-";
	}
}
