﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Ratcycle
{
    public class Rat : Entity
    {
        private Garbage _inventory;

		private long _gameOverTick;

		public bool IsAlive { get { return _alive; } }

		public enum Directions
		{
			Up,
			Right,
			Down,
			Left
		}

        public Garbage Inventory
        {
            get { return _inventory; }
        }

        public override Rectangle AttackBox
        {
            get
            {
                if (_flip)
                {
                    return new Rectangle(
                        (int)_position.X - 30,
                        (int)_position.Y + 30,
                        30,
						(int)_sourceRectangleDimensions.Y - 30);
                }
                else
                {
                    return new Rectangle(
						(int)(_position.X + _sourceRectangleDimensions.X),
                        (int)_position.Y + 30,
                        30,
						(int)_sourceRectangleDimensions.Y - 30);
                }
            }
        }

        public Rectangle BodyBox
        {
            get
            {
                return new Rectangle(
                    (int)_position.X,
                    (int)_position.Y,
					(int)_sourceRectangleDimensions.X,
					(int)_sourceRectangleDimensions.Y
                );
            }
        }

		public override Rectangle HitBox
		{
			get
			{
				if (_flip) 
                {
					return new Rectangle (
						(int)_position.X,
						(int)_position.Y + 50,
						(int)(_sourceRectangleDimensions.X - 25),
						(int)(_sourceRectangleDimensions.Y - 50));
				} 
                else 
                {
					return new Rectangle (
						(int)_position.X + 25,
						(int)_position.Y + 50,
						(int)(_sourceRectangleDimensions.X - 25),
						(int)(_sourceRectangleDimensions.Y - 50));
				}
			}
		}

        /// <summary>
        /// Constructs the rat.
        /// </summary>
        /// <param name="position"></param>
		/// <param name="texture"></param>
        /// <param name="game"></param>
        /// <param name="view"></param>
        /// <param name="speed"></param>
        /// <param name="health"></param>
        /// <param name="damage"></param>
		public Rat(Texture2D texture, Vector2 position, Game1 game, View view, Vector2 speed, float health, float damage)
            : base(texture, position, game, view, Color.White, 6, 6, 36, true, speed)
        {
			_health = (100 + (15 * (float) Model.Rat.level));
            _damage = (18 + (3 * (float)Model.Rat.level));
        }
        
        /// <summary>
        /// Moves the rat according to certain keyboard input and sets border boundaries for movement.
        /// </summary>
        private void Move()
        {
            Stage view = (Stage)_view;

			//up
			if (KeyHandler.IsKeyDown(Model.Settings.Key.Up) && view.NotColliding(this, MakeFutureRectangle(Directions.Up), _minCoords, _maxCoords)) 
            {
				_position.Y -= _speed.Y;

                if (_flip)
                    ChangeFrame(3);
                else
                    ChangeFrame(2);
			}
			//down
			if (KeyHandler.IsKeyDown(Model.Settings.Key.Down) && view.NotColliding(this, MakeFutureRectangle(Directions.Down), _minCoords, _maxCoords)) 
            {
				_position.Y += _speed.Y;

                if (_flip)
                    ChangeFrame(3);
                else
                    ChangeFrame(2);
			}
			//left
			if (KeyHandler.IsKeyDown(Model.Settings.Key.Left)) 
            {
				if (!_flip) 
                {
					_position.X += 25;
				}

				ChangeFrame (3);
				
				_flip = true;
                if (view.NotColliding(this, MakeFutureRectangle(Directions.Left), _minCoords, _maxCoords))
                {
                    _position.X -= _speed.X;
                }
			}
			//right
			if (KeyHandler.IsKeyDown(Model.Settings.Key.Right))
            {
					if (_flip) 
                    {
						_position.X -= 25;
					}

					ChangeFrame (2);
					
					_flip = false;
                    if (view.NotColliding(this, MakeFutureRectangle(Directions.Right), _minCoords, _maxCoords))
                    {
                        _position.X += _speed.X;
                    }
			}
			if (!KeyHandler.IsKeyDown (Model.Settings.Key.Right) && !KeyHandler.IsKeyDown (Model.Settings.Key.Left) && !KeyHandler.IsKeyDown(Model.Settings.Key.Up) && !KeyHandler.IsKeyDown(Model.Settings.Key.Down)) 
			{
				if (_flip)
					ChangeFrame (5);
				else
					ChangeFrame (4);
			}
		}
        
        private void Attack()
        {
			if (KeyHandler.checkNewKeyPressed(Model.Settings.Key.Attack))
            {
                //SoundEffect
                _game.soundEffect = new SoundHandler("Woosj", Model.Settings.SoundEffectVolume);
                _game.soundEffect.Play();

                //Animate
				if (_flip)
					StartSingleMotion(1);
				else
					StartSingleMotion(0);
				
                ((Stage)_view).AttackHandler(this, _damage, AttackBox);
            }
        }

        private void PickUp()
        {
			if (KeyHandler.checkNewKeyPressed(Model.Settings.Key.PickUp))
            {
                _inventory = ((Stage)_view).GarbageHandler(_inventory);
            }
        }


		private Rectangle MakeFutureRectangle (Directions direction)
        {
			switch (direction) 
			{
				case Directions.Up:
					return new Rectangle (HitBox.X, HitBox.Y - (int)_speed.Y, HitBox.Width, HitBox.Height);
				case Directions.Down:
					return new Rectangle (HitBox.X, HitBox.Y + (int)_speed.Y, HitBox.Width, HitBox.Height);
				case Directions.Left:
					return new Rectangle (HitBox.X - (int)_speed.X, HitBox.Y, HitBox.Width, HitBox.Height);
				case Directions.Right:
					return new Rectangle (HitBox.X + (int)_speed.X, HitBox.Y, HitBox.Width, HitBox.Height);
				default:
					return new Rectangle();
			}
        }
        

        public override void KillEntity()
        {
			if (_alive)
			{
				_game.Music.Stop ();
                _game.soundEffect = new SoundHandler("DeathSpiral", Model.Settings.SoundEffectVolume);
                _game.soundEffect.Play();

				((Stage)_view).NewPlayerFeedback("Fatality", Color.Red, new Vector2(_position.X - 30, _position.Y), 30f, 100f);
				_alive = false;
				_gameOverTick = Model.Time.CurrentGameTick + (Model.Time.OneSecondOfTicks * 2);
			} 
        }

		public override void Hit (float damage)
		{
			base.Hit (damage);

		}

        /// <summary>
        /// Updates the rat.
        /// </summary>
        public override void Update()
        {
            base.Update();
			if (_alive) 
			{
				Move();
				PickUp();
				Attack();
			}
			else if (!_alive)
			{
				// Makes the rat stop breathing
				if (_flip)
					ChangeFrame (5, 0);
				else
					ChangeFrame (4, 0);

				// Show GameOver Screen at right time
				if (Model.Time.CurrentGameTick >= _gameOverTick)
				{
					((Stage)_view).GameOver();
				}
			}
        }
    }
}
	
