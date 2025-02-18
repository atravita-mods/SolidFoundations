﻿using Microsoft.Xna.Framework.Graphics;
using SolidFoundations.Framework.Interfaces;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidFoundations.Framework.Managers
{
    internal class AssetManager
    {
        private IMonitor _monitor;
        private Texture2D _appearanceButton;

        public AssetManager(IMonitor monitor, IModHelper helper)
        {
            _monitor = monitor;


            // Get the asset folder path
            var assetFolderPath = helper.ModContent.GetInternalAssetName(Path.Combine("Framework", "Assets")).Name;

            // Load in the assets
            _appearanceButton = helper.ModContent.Load<Texture2D>(Path.Combine(assetFolderPath, "appearance_button.png"));
        }

        public Texture2D GetAppearanceButton()
        {
            return _appearanceButton;
        }
    }
}
