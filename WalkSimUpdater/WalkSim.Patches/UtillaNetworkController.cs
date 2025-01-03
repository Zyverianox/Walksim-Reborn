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
        string gamemode = "";
        if (PhotonNetwork.CurrentRoom != null)
        {
            Room currentRoom = PhotonNetwork.NetworkingClient.CurrentRoom;
            isPrivate = !currentRoom.IsVisible || ((Dictionary<object, object>)currentRoom.CustomProperties).ContainsKey("Description");
            if (((Dictionary<object, object>)currentRoom.CustomProperties).TryGetValue("gameMode", out object value))
            {
                gamemode = value as string;
            }
        }

        string displayMode = GetDisplayMode(gamemode);

        GorillaComputer.instance.currentGameModeText.Value = "CURRENT MODE\n" + displayMode;
        Events.RoomJoinedArgs e = new Events.RoomJoinedArgs
        {
            isPrivate = isPrivate,
            Gamemode = gamemode
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
        GorillaComputer.instance.currentGameModeText.Value = "CURRENT MODE\n-NOT IN ROOM-";
    }

    private string GetDisplayMode(string gamemode)
    {
        Dictionary<string, string> gamemodeMapping = new Dictionary<string, string>
        {
            { "INFECTION", "INFECTION" },
            { "CASUAL", "CASUAL" },
            { "HUNT", "HUNT" },
            { "BATTLE", "PAINTBRAWL" },
            { "NEW_MODE1", "NEW_MODE1_DISPLAY" }, // Add new game modes here
            { "NEW_MODE2", "NEW_MODE2_DISPLAY" }  // Add new game modes here
        };

        foreach (KeyValuePair<string, string> item in gamemodeMapping)
        {
            if (gamemode.Contains(item.Key))
            {
                return item.Value;
            }
        }
        return "UNKNOWN";
    }
}
