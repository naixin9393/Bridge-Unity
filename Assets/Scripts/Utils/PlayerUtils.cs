using System;
using System.Collections.Generic;
using System.Linq;

public static class PlayerUtils {
    public static IPlayer GetNextPlayer(IPlayer player, List<IPlayer> players) {
        return player.Position switch {
            Position.North => players.Where(p => p.Position == Position.East).First(),
            Position.East => players.Where(p => p.Position == Position.South).First(),
            Position.South => players.Where(p => p.Position == Position.West).First(),
            Position.West => players.Where(p => p.Position == Position.North).First(),
            _ => throw new NotImplementedException(),
        };
    }

    public static IPlayer PartnerOf(IPlayer player, List<IPlayer> players) => players.Where(p => p.Position == PositionOfPartner(player)).First();

    public static Position PositionOfPartner(IPlayer player) => OppositePosition(player.Position);

    public static bool OnDifferentTeam(IPlayer player, IPlayer player2) => player.Position != player2.Position && player.Position != PositionOfPartner(player2);

    public static bool OnDifferentTeam(Position position, Position position2) => position != position2 && position != OppositePosition(position2);

    private static Position OppositePosition(Position position) {
        return position switch {
            Position.North => Position.South,
            Position.East => Position.West,
            Position.South => Position.North,
            Position.West => Position.East,
            _ => throw new NotImplementedException(),
        };
    }
}