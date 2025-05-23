using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerUtils {
    public static IPlayer GetNextPlayer(IPlayer player, List<IPlayer> players) {
        return player.Position switch {
            Position.North => players.Where(p => p.Position == Position.East).First(),
            Position.East => players.Where(p => p.Position == Position.South).First(),
            Position.South => players.Where(p => p.Position == Position.West).First(),
            Position.West => players.Where(p => p.Position == Position.North).First(),
            _ => throw new NotImplementedException(),
        };
    }
}