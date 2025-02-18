﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using SolidFoundations.Framework.Models.Backport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidFoundations.Framework.Models.ContentPack
{
    public class ExtendedBuildingChest : BuildingChest
    {
        [ContentSerializer(Optional = true)]
        public int Capacity { get { return _capacity <= 0 ? 1 : _capacity; } set { _capacity = value; } }
        protected int _capacity = 36;
    }
}
