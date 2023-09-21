using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameState
{
    [Tooltip("First number represents a major content update, second represents minor content or feature update, third represents bug fixes or minor improvements")]
    public static string VersionNumber = "0.00.01";

    public static bool testing = true;

    public static Room currentRoom;
    public static bool currentRoomClear = true;
}
