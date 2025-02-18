﻿using Microsoft.Xna.Framework.Content;
using System.Collections.Generic;

namespace SolidFoundations.Framework.Models.Backport
{
    // TODO: When updated to SDV v1.6, this class should be deleted in favor of using StardewValley.GameData.BuildingItemConversion
    public class BuildingItemConversion
    {
        public List<string> RequiredTags;

        [ContentSerializer(Optional = true)]
        public int RequiredCount = 1;

        [ContentSerializer(Optional = true)]
        public int MaxDailyConversions = 1;

        public string SourceChest;

        public string DestinationChest;

        public List<AdditionalChopDrops> ProducedItems;
    }
}