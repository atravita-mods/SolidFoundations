﻿using Newtonsoft.Json;
using SolidFoundations.Framework.Models.ContentPack;
using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolidFoundations.Framework.Managers
{
    internal class BuildingManager
    {
        private IMonitor _monitor;
        private IModHelper _helper;

        private Dictionary<string, string> _assetPathToMap;
        private Dictionary<string, string> _assetPathToTexture;
        private Dictionary<string, ExtendedBuildingModel> _idToModels;

        public BuildingManager(IMonitor monitor, IModHelper helper)
        {
            _monitor = monitor;
            _helper = helper;

            _assetPathToMap = new Dictionary<string, string>();
            _assetPathToTexture = new Dictionary<string, string>();
            _idToModels = new Dictionary<string, ExtendedBuildingModel>();
        }

        public void Reset()
        {
            _assetPathToMap.Clear();
            _assetPathToTexture.Clear();
            _idToModels.Clear();
        }

        public void AddBuilding(ExtendedBuildingModel model)
        {
            _idToModels[model.ID] = model;
        }

        public void AddMapAsset(string assetPath, string pathToMap)
        {
            if (String.IsNullOrEmpty(assetPath) || String.IsNullOrEmpty(pathToMap))
            {
                return;
            }

            _assetPathToMap[$"Maps/{assetPath}"] = pathToMap;
        }

        public void AddTextureAsset(string assetPath, string pathToTexture)
        {
            if (String.IsNullOrEmpty(assetPath) || String.IsNullOrEmpty(pathToTexture))
            {
                return;
            }

            _assetPathToTexture[assetPath] = pathToTexture;
        }

        public string GetMapAsset(string assetPath)
        {
            if (_assetPathToMap.ContainsKey(assetPath))
            {
                return _assetPathToMap[assetPath];
            }

            return null;
        }

        public string GetTextureAsset(string assetPath)
        {
            if (String.IsNullOrEmpty(assetPath) is false && _assetPathToTexture.ContainsKey(assetPath))
            {
                return _assetPathToTexture[assetPath];
            }

            return null;
        }

        public List<ExtendedBuildingModel> GetAllBuildingModels()
        {
            return _idToModels.Values.ToList();
        }

        public Dictionary<string, ExtendedBuildingModel> GetIdToModels()
        {
            return _idToModels;
        }

        public ExtendedBuildingModel GetSpecificBuildingModel(string buildingId)
        {
            return String.IsNullOrEmpty(buildingId) is false && _idToModels.ContainsKey(buildingId) ? _idToModels[buildingId] : null;
        }

        public bool DoesBuildingModelExist(string buildingId)
        {
            return String.IsNullOrEmpty(buildingId) is false && _idToModels.ContainsKey(buildingId);
        }
    }
}
