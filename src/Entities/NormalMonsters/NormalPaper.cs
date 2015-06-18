﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ratcycle
{
    class NormalPaper : Monster
    {
        public NormalPaper(Game1 game, View view)
            : base(ContentHandler.GetTexture("monster_NormalPaper"), game, view, new Vector2(1, 1), 100, 1, 15, 3.0f, Model.GameRules.Category.Paper, Model.GameRules.Type.Normal)
        {
        }
    }
}
