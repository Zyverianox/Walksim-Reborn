using System;

namespace WalkSim.Patches;

public class Events
{
	public class RoomJoinedArgs : EventArgs
	{
		public bool isPrivate { get; set; }

		public string Gamemode { get; set; }
	}

	public static event EventHandler<RoomJoinedArgs> RoomJoined;

	public static event EventHandler<RoomJoinedArgs> RoomLeft;

	public static event EventHandler GameInitialized;

	public virtual void TriggerRoomJoin(RoomJoinedArgs e)
	{
		Events.RoomJoined?.SafeInvoke(this, e);
	}

	public virtual void TriggerRoomLeft(RoomJoinedArgs e)
	{
		Events.RoomLeft?.SafeInvoke(this, e);
	}

	public virtual void TriggerGameInitialized()
	{
		Events.GameInitialized?.SafeInvoke(this, EventArgs.Empty);
	}
}
