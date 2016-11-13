using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace LingrenGame
{
    public class AnimatedSprite
    {
        //sprite texture and position
        Texture2D spriteImage;
        float _scale = 1;

        public Texture2D SpriteImage
        {
            get { return spriteImage; }
            set { spriteImage = value; }
        }
        public Vector2 position;
        public bool Visible = true; 

        //the number of frames in the sprite sheet
        //the current fram in the animation
        //the time between frames
        int numberOfFrames = 0;
        int currentFrame = 0;
        int mililsecondsBetweenFrames = 100;
        float timer = 0f;
        public Texture2D ShowRect;
        //the width and height of our texture
        protected int spriteWidth = 0;

        public int SpriteWidth
        {
            get { return spriteWidth; }
            set { spriteWidth = value; }
        }
        int spriteHeight = 0;

        public int SpriteHeight
        {
            get { return spriteHeight; }
            set { spriteHeight = value; }
        }

        public float Scale
        {
            get
            {
                return _scale;
            }

            set
            {
                _scale = value;
            }
        }

        //the source of our image within the sprite sheet to draw
        Rectangle sourceRectangle;
        SpriteEffects _effect;

        public Rectangle BoundingRect;

        // Variable added to only all sound to play on initial collision
        // maintains the state of collision of the sprite over the course of updates
        // Set in collision detection function

        public bool InCollision = false;

        public AnimatedSprite(Texture2D texture,Vector2 userPosition, int framecount)
        {
            spriteImage = texture;
            position = userPosition;
            numberOfFrames = framecount;
            spriteHeight = spriteImage.Height;
            spriteWidth = spriteImage.Width / framecount;
            _effect = SpriteEffects.None;
            BoundingRect = new Rectangle((int)this.position.X, (int)this.position.Y, this.spriteWidth, this.spriteHeight);

        }


        public virtual void Update(GameTime gametime)
        {
            timer += (float)gametime.ElapsedGameTime.Milliseconds;

            //if the timer is greater then the time between frames, then animate
                    if (timer > mililsecondsBetweenFrames)
                    {
                        //moce to the next frame
                        currentFrame++;

                        //if we have exceed the number of frames
                        if (currentFrame > numberOfFrames - 1)
                        {
                            currentFrame = 0;
                        }
                        //reset our timer
                        timer = 0f;
                    }
            //set the source to be the current frame in our animation
                    sourceRectangle = new Rectangle(currentFrame * spriteWidth,
                        0, spriteWidth, spriteHeight);
            }
        public bool collisionDetect(AnimatedSprite otherSprite)
        {
            if (otherSprite.Visible)
            {
                BoundingRect = new Rectangle((int)this.position.X, (int)this.position.Y, (int)(this.spriteWidth / _scale), (int)(this.spriteHeight / _scale));
                Rectangle otherBound = new Rectangle((int)otherSprite.position.X, (int)otherSprite.position.Y, (int)(otherSprite.spriteWidth / _scale), (int)(this.spriteHeight));
                if (BoundingRect.Intersects(otherBound))
                {
                    InCollision = true;
                    return true;
                }
                else
                {
                    InCollision = false;
                    return false;
                }
            }
            else InCollision = false;
            return false;
        }
        // Added code for movement and to spot horizontal effect
        // Note assumes right facing sprite to begin
        public void Move(Vector2 delta)
        {
            position += delta;
            // update the new position of the Bounding Rect for an Animated sprite
            BoundingRect = new Rectangle((int)this.position.X, (int)this.position.Y, this.spriteWidth, this.spriteHeight);
            //if (delta.X < 0)
            //    _effect = SpriteEffects.FlipHorizontally;
            //else _effect = SpriteEffects.None;
                
        }
        public virtual void Draw(SpriteBatch spriteBatch, SpriteFont font)
        {

            //draw the sprite , specify the postion and source for the image withtin the sprite sheet
            //spriteBatch.Begin();
            // Changed to allow for sprite effect
            
            if(Visible)
               //spriteBatch.Begin();
               spriteBatch.Draw(spriteImage, position,sourceRectangle,Color.White,0f,Vector2.Zero,_scale,_effect,0f);
            if(ShowRect != null)
                spriteBatch.Draw(ShowRect,BoundingRect, Color.White);
            if (InCollision)
                spriteBatch.DrawString(font, "Im in collision", new Vector2(10, -10) + position, Color.White);
            //spriteBatch.End();
        }       

    }
}
