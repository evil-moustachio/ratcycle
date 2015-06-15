﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ratcycle
{
    public class StrongOther : Monster
    {
        public StrongOther(Vector2 position, Game1 game, View view)
            : base(ContentHandler.GetTexture("PC_REST_GROOT"), position, game, view, new Vector2(1,1), 100, 1, 15, 0.8f, Model.GameRules.Category.Other, Model.GameRules.Type.Strong)
        {
        }
    }
}
