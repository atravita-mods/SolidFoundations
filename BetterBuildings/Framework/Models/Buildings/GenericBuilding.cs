﻿using BetterBuildings.Framework.Models.General;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Buildings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BetterBuildings.Framework.Models.ContentPack
{
    public class GenericBuilding : Building
    {
        public BuildingModel Model { get; set; }
        public string Id { get; set; }
        public string LocationName { get; set; }
        public TileLocation TileLocation { get { return new TileLocation() { X = base.tileX.Value, Y = base.tileY.Value }; } }

        internal bool IsUsingEventOverride { get; set; }
        internal bool DrawOverPlayer { get; set; }
        internal float? AlphaOverride { get; set; }

        public GenericBuilding() : base()
        {

        }

        public GenericBuilding(BuildingModel model, GenericBlueprint genericBlueprint) : base(genericBlueprint, Vector2.Zero)
        {
            RefreshModel(model);

            base.indoors.Value = GetIndoors();
            this.updateInteriorWarps();
        }

        public void RefreshModel(BuildingModel model)
        {
            Model = model;
            Id = model.Id;

            base.tilesHigh.Value = model.Dimensions.Height;
            base.tilesWide.Value = model.Dimensions.Width;
            base.fadeWhenPlayerIsBehind.Value = model.FadeWhenPlayerIsBehind;
        }

        public GameLocation GetIndoors()
        {
            if (Model is not null && !String.IsNullOrEmpty(Model.MapPath))
            {
                var indoorLocation = new GameLocation(Model.MapPath, Model.Id);
                indoorLocation.uniqueName.Value = Model.Id + Guid.NewGuid().ToString();

                if (Model.InteriorType is InteriorType.Greenhouse)
                {
                    indoorLocation.IsGreenhouse = true;
                }
                else if (Model.InteriorType is InteriorType.Coop or InteriorType.Barn)
                {
                    indoorLocation.IsFarm = true;
                }
                indoorLocation.isStructure.Value = true;

                return indoorLocation;
            }

            return null;
        }

        private bool AttemptTunnelDoorTeleport()
        {
            if (Model.Doorways is null || !Model.Doorways.Any(d => d.Type is DoorType.Tunnel))
            {
                return false;
            }

            var tunnelDoorway = Model.Doorways.First(d => d.Type == DoorType.Tunnel);
            foreach (var tile in tunnelDoorway.Tiles)
            {
                if (base.tileX.Value + tile.X == Game1.player.getTileX() && base.tileY.Value + tile.Y == Game1.player.getTileY())
                {
                    // Warp player inside
                    base.indoors.Value.isStructure.Value = true;
                    Game1.player.currentLocation.playSoundAt("doorClose", Game1.player.getTileLocation());

                    // Get warp destination tile
                    var destinationTile = new TileLocation() { X = this.indoors.Value.warps[0].X, Y = this.indoors.Value.warps[0].Y - 1 };
                    if (tunnelDoorway.EntranceTile is not null)
                    {
                        destinationTile = tunnelDoorway.EntranceTile;
                    }

                    Game1.warpFarmer(this.indoors.Value.uniqueName.Value, destinationTile.X, destinationTile.Y, Game1.player.FacingDirection, isStructure: true);
                    return true;
                }
            }

            return false;
        }

        private bool AttemptEventTileTrigger()
        {
            if (Model.EventTiles is null || Model.EventTiles.Count <= 0)
            {
                return false;
            }

            foreach (var eventTile in Model.EventTiles)
            {
                if (base.tileX.Value + eventTile.Tile.X == Game1.player.getTileX() && base.tileY.Value + eventTile.Tile.Y == Game1.player.getTileY())
                {
                    // Trigger the tile
                    eventTile.Trigger(this, Game1.player);

                    return true;
                }
            }

            return false;
        }

        private void ResetEventOverrides()
        {
            AlphaOverride = null;
            DrawOverPlayer = false;

            IsUsingEventOverride = false;
        }

        private bool IsTileToTheRightWalkable(TileLocation tileLocation)
        {
            return Model.WalkableTiles.Any(t => t.X + base.tileX.Value == tileLocation.X + 1 && t.Y + base.tileY.Value == tileLocation.Y);
        }

        private bool IsNearbyTileWalkable(TileLocation tileLocation, int direction)
        {
            return Model.WalkableTiles.Any(t => t.X + base.tileX.Value == tileLocation.X + (direction == 1 ? 1 : 0) + (direction == 3 ? -1 : 0) && t.Y + base.tileY.Value == tileLocation.Y + (direction == 0 ? -1 : 0) + (direction == 2 ? 1 : 0));
        }

        public override bool intersects(Rectangle boundingBox)
        {
            if (base.daysOfConstructionLeft.Value > 0)
            {
                return base.intersects(boundingBox);
            }

            if (base.intersects(boundingBox))
            {
                var boundingTileLocation = new TileLocation() { X = boundingBox.X / 64, Y = boundingBox.Y / 64 };
                if (IsNearbyTileWalkable(boundingTileLocation, -1))
                {
                    if (!AttemptTunnelDoorTeleport())
                    {
                        AttemptEventTileTrigger();
                    }

                    var tileRectangle = new Rectangle(boundingTileLocation.X * 64, boundingTileLocation.Y * 64, 64, 64);
                    if (tileRectangle.Contains(new Vector2(boundingBox.Right, boundingBox.Top)) || IsTileToTheRightWalkable(boundingTileLocation))
                    {
                        return false;
                    }
                }

                return true;
            }

            return base.intersects(boundingBox);
        }

        public override void updateInteriorWarps(GameLocation interior = null)
        {
            if (interior is null)
            {
                if (this.indoors.Value is null)
                {
                    return;
                }

                interior = this.indoors.Value;
            }

            if (Model.Doorways is not null)
            {
                TileLocation exitTile = new TileLocation() { X = 0, Y = base.tilesHigh.Value };
                if (Model.Doorways.FirstOrDefault(d => d.Type is DoorType.Standard or DoorType.Tunnel) is DoorTiles humanDoorway && humanDoorway is not null && humanDoorway.ExitTile is not null)
                {
                    exitTile = humanDoorway.ExitTile;
                }

                foreach (Warp warp in interior.warps)
                {
                    warp.TargetX = base.tileX.Value + exitTile.X;
                    warp.TargetY = base.tileY.Value + exitTile.Y;
                }
            }
        }

        public override void Update(GameTime time)
        {
            this.alpha.Value = Math.Min(1f, this.alpha.Value + 0.05f);

            if (AlphaOverride is not null)
            {
                this.alpha.Value = AlphaOverride.Value;
            }
            else
            {
                var isPlayerNearTopOfBuilding = Game1.player.GetBoundingBox().Intersects(new Rectangle(64 * this.tileX.Value, 64 * (this.tileY.Value + (-(this.getSourceRectForMenu().Height / 16) + tilesHigh)), this.tilesWide.Value * 64, (this.getSourceRectForMenu().Height / 16 - tilesHigh) * 64));

                if (this.fadeWhenPlayerIsBehind.Value && isPlayerNearTopOfBuilding)
                {
                    this.alpha.Value = Math.Max(0.4f, this.alpha.Value - 0.09f);
                }
            }

            if (IsUsingEventOverride && !base.intersects(Game1.player.GetBoundingBox()))
            {
                ResetEventOverrides();
            }
        }

        public override void drawShadow(SpriteBatch b, int localX = -1, int localY = -1)
        {
            if (Model.ShowShadow)
            {
                base.drawShadow(b, localX, localY);
            }
        }

        public override void draw(SpriteBatch b)
        {
            if (!base.isMoving)
            {
                if (base.daysOfConstructionLeft.Value > 0)
                {
                    this.drawInConstruction(b);
                    return;
                }

                this.drawShadow(b);
                b.Draw(base.texture.Value, Game1.GlobalToLocal(Game1.viewport, new Vector2(base.tileX.Value * 64, base.tileY.Value * 64 + base.tilesHigh.Value * 64)), base.texture.Value.Bounds, base.color.Value * base.alpha.Value, 0f, new Vector2(0f, base.texture.Value.Bounds.Height), 4f, SpriteEffects.None, (float)((base.tileY.Value + base.tilesHigh.Value - 3) * 64) / (DrawOverPlayer ? 1000f : 10000f));


                if (BetterBuildings.showWalkableTiles)
                {
                    var playerPosition = Game1.GlobalToLocal(Game1.viewport, new Vector2((Game1.player.GetBoundingBox().X), (Game1.player.GetBoundingBox().Y)));
                    b.Draw(Game1.staminaRect, new Rectangle((int)playerPosition.X, (int)playerPosition.Y, Game1.player.GetBoundingBox().Width, Game1.player.GetBoundingBox().Height), new Rectangle(0, 0, 1, 1), Color.Blue, 0f, Vector2.Zero, SpriteEffects.None, 100f);

                    foreach (var tileLocation in Model.WalkableTiles)
                    {
                        var position = Game1.GlobalToLocal(Game1.viewport, new Vector2((tileLocation.X + base.tileX.Value) * 64, (tileLocation.Y + base.tileY.Value) * 64));
                        b.Draw(Game1.staminaRect, position, new Rectangle(0, 0, 1, 1), Color.Red, 0f, Vector2.Zero, 64, SpriteEffects.None, 10f);
                    }
                }
            }
        }
    }
}
