﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Ratcycle
{
	public class MenuStart : View
	{
		public MenuStart (Game1 game, ViewController viewController, Boolean mouseVisible) : base(game, viewController, mouseVisible)
		{
			//TODO: Add logo
			_gameObjects.Add (new Button (new Vector2(300, 310), _game, this, ContentHandler.GetTexture("StartButton"), 3, new Stage(_game, _viewController, false)));
		}
	}
}

