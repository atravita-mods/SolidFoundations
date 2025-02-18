﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace SolidFoundations.Framework.Models.Backport
{
    // TODO: When updated to SDV v1.6, this class should be deleted in favor of using StardewValley.GameData.IndoorItemMove
    public class IndoorItemMove
    {
        [ContentSerializer(Optional = true)]
        public Point Source;

        [ContentSerializer(Optional = true)]
        public Point Destination;

        [ContentSerializer(Optional = true)]
        public Point Size = new Point(1, 1);
    }
}