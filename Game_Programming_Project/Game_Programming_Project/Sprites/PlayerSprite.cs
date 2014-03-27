﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game_Programming_Project.Sprites
{
    class PlayerSprite: Sprite
    {

        
        float jumpSpeed = 5;
        float maxJump = 120;
        float curMaxJumpHeight = 0;
        public Vector2 collisionLocation = Vector2.Zero;
        public bool jumping = false;
        public bool attacking = false;


        /*
         * Constructor to use default frame rate
         */
        public PlayerSprite(Texture2D textureImage, Vector2 position, Point frameSize, 
            int collisionOffset, Point currentFrame, Point sheetSize, Vector2 speed)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed)
        {
        }


        /*
         * Constructor to set framerate
         */
        public PlayerSprite(Texture2D textureImage, Vector2 position,
            Point frameSize, int collisionOffset, Point currentFrame, Point sheetSize,
            Vector2 speed, int millisecondsPerFrame)
            : base(textureImage, position, frameSize, collisionOffset, currentFrame,
            sheetSize, speed, millisecondsPerFrame)
        {
        }


        /*
         * Get direction of sprite based on player input and speed
         */
        public override Vector2 direction
        {
            get
            {
                Vector2 inputDirection = Vector2.Zero;

                //Check if player is moving left
                if ((Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A)) &&
                    collisionLocation.X != 1)
                {
                    inputDirection.X -= 1;
                }

                //Check if player is moving right
                if ( (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D)) &&
                    collisionLocation.X != -1)
                {
                    inputDirection.X += 1;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
                {
                }

                return inputDirection * speed;
            }


        }


        /*
         * 
         */
        public override void Update(GameTime gameTime, Rectangle clientBounds)
        {
            // Move the sprite based on direction
            position += direction;

            // If sprite is off the screen, move it back within the game window
            if (position.X < 0)
                position.X = 0;
            if (position.Y < 0)
                position.Y = 0;
            if (position.X > Game.resolution.X - frmSize.X)
                position.X = Game.resolution.X - frmSize.X;
            if (position.Y > Game.resolution.Y - frmSize.Y)
                position.Y = Game.resolution.Y - frmSize.Y;

            //Check which direction the player is moving, and add necessary SpriteEffect.
            if (direction.X < 0)
                spriteEffect = SpriteEffects.FlipHorizontally;
            else if (direction.X > 0)
                spriteEffect = SpriteEffects.None;

            //Allow the player to jump
            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W) ||
                Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                //Only allow the player to jump if they are standing on the ground
                if (position.Y == (Game.resolution.Y - frmSize.Y) || collisionLocation.Y == 1)
                {
                    jumping = true;
                    curMaxJumpHeight = position.Y - maxJump;
                }
            }
            else //Stop jumping when the key is released
            {
                jumping = false;
                curMaxJumpHeight = 0;
            }

            //Move the player along the Y axis as necessary
            if ((!jumping || (jumping && position.Y <= curMaxJumpHeight)) &&
                collisionLocation.Y != 1)
            {
                if (position.Y < clientBounds.Height - frmSize.Y)
                {
                    position.Y += SpriteManager.FALLSPEED;
                    jumping = false;
                    curMaxJumpHeight = 0;
                }
            }
            else if(jumping)
            {
                position.Y -= jumpSpeed;
            }

            //Allow the player to attack
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                attacking = true;
            }
            else
            {
                attacking = false;
            }

            collisionLocation = Vector2.Zero;

            base.Update(gameTime, clientBounds);

        }

    }
}
