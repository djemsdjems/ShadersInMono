using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//alright :)
namespace ShadersInMono
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //image to test on
        private Texture2D _image;

        //this is the object that holds the shader. u dont have to think much about it ;)
        private Effect shader;

        //this gonna be used for showing how communication goes between the shader and mono
        private float brightness = 1;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            //loading the test image :)
            _image = Content.Load<Texture2D>("dedodatedwam");

            //Loading the shader code...Shader.fx- in this shader we have both the vertex and the fragment shaders
            //it is inside of the Content Folder!
            shader = Content.Load<Effect>("Shader");

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Up/Down Arrows for modifying the Brightness value!
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                brightness += 0.1f; 
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                brightness -= 0.1f;

            //prevent it from going 5 or lower than 0.2 for good measure ;)
            brightness = Math.Clamp(brightness, 0.2f, 5);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            //the creation of the matrix is here just to make every thing work , u dont have to understand LOL
            Matrix worldViewProjection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height, 0, 0, 1);

            //now Stop and start Looking both at this and at the shader code to understand>:|

            //we pass in the matrix as mention no need to know it just makes things work ;)
            shader.Parameters["WorldViewProjection"].SetValue(worldViewProjection);

            //we pass in the brightness uniform
            shader.Parameters["brightness"]?.SetValue(brightness);

            //we pass in the time uniform
            shader.Parameters["time"]?.SetValue((float)gameTime.TotalGameTime.TotalMilliseconds);

            //the old fashioned clearing stuff
            GraphicsDevice.Clear(Color.Black);

            //we draw the texture but we with the applied using the effect parameter!
            // you can draw multiple textures and they all will have the shader applied!
            _spriteBatch.Begin(effect: shader);//shader is our shader 
            _spriteBatch.Draw(_image, new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
