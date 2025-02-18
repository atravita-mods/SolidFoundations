﻿using Microsoft.Xna.Framework;
using SolidFoundations.Framework.Models.ContentPack;
using SolidFoundations.Framework.Models.ContentPack.Actions;
using StardewValley;
using StardewValley.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.GameLocation;

namespace SolidFoundations.Framework.UI
{
    internal class SpecialActionDialogueBox : DialogueBox
    {
        private SpecialAction _actionToTrigger;
        private GenericBuilding _genericBuilding;
        private Point _tile;
        private afterQuestionBehavior _afterDialogueBehavior;

        public SpecialActionDialogueBox(List<string> dialogue, SpecialAction actionToTrigger, GenericBuilding building, Point tile) : base(dialogue)
        {
            _actionToTrigger = actionToTrigger;
            _genericBuilding = building;
            _tile = tile;
        }

        public SpecialActionDialogueBox(string dialogue, List<Response> responses, afterQuestionBehavior afterDialogueBehavior, int width = 1200) : base(dialogue, responses, width)
        {
            _afterDialogueBehavior = afterDialogueBehavior;
        }

        public void SetUp()
        {
            Game1.dialogueUp = true;
            Game1.player.CanMove = false;
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (ShouldHandleLeftClick())
            {
                if (_afterDialogueBehavior is not null)
                {
                    _afterDialogueBehavior.Invoke(Game1.player, this.responses[this.selectedResponse].responseKey);
                    this.closeDialogue();
                    return;
                }
            }

            base.receiveLeftClick(x, y, playSound);
        }

        public new void closeDialogue()
        {
            base.closeDialogue();

            if (_actionToTrigger is not null)
            {
                _actionToTrigger.Trigger(Game1.player, _genericBuilding, _tile);
            }
        }

        private bool ShouldHandleLeftClick()
        {
            if (this.transitioning)
            {
                return false;
            }
            if (this.characterIndexInDialogue < this.getCurrentString().Length - 1)
            {
                return false;
            }
            else
            {
                if (this.safetyTimer > 0)
                {
                    return false;
                }
                if (this.isQuestion)
                {
                    if (this.selectedResponse == -1)
                    {
                        return false;
                    }

                    if (this.characterDialogue == null)
                    {
                        if (Game1.eventUp && Game1.currentLocation.afterQuestion == null)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}
