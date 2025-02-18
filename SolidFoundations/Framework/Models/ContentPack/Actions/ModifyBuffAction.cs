﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SolidFoundations.Framework.Models.ContentPack.Actions.SpecialAction;

namespace SolidFoundations.Framework.Models.ContentPack.Actions
{
    public class ModifyBuffAction
    {
        public string Buff { get; set; }
        public string Description { get; set; }
        public Color Glow { get; set; } = Color.White;
        public int Level { get; set; } = 1;
        public int DurationInMilliseconds { get; set; } = 1000;
        public List<ModifyBuffAction> SubBuffs { get; set; } = new List<ModifyBuffAction>();

        public BuffType GetBuffType()
        {
            if (Enum.TryParse(typeof(BuffType), Buff, true, out var actualBuff) && actualBuff is not null)
            {
                return (BuffType)actualBuff;
            }

            return BuffType.Unknown;
        }
    }
}
