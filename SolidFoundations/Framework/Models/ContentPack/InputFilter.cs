﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SolidFoundations.Framework.Models.Backport;
using SolidFoundations.Framework.Models.ContentPack.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidFoundations.Framework.Models.ContentPack
{
    public class InputFilter
    {
        [ContentSerializer(Optional = true)]
        public List<RestrictedItem> RestrictedItems;

        [ContentSerializer(Optional = true)]
        public string FilteredItemMessage;

        [ContentSerializer(Optional = true)]
        public string InputChest;

        public class RestrictedItem
        {
            public List<string> RequiredTags { get; set; }
            public int MaxAllowed { get; set; } = -1;
            public bool RejectWhileProcessing { get; set; }
            public string Condition { get; set; }
            public string[] ModDataFlags { get; set; }
        }
    }
}
