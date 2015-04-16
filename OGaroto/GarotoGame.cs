using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace OGaroto
{
    /// <summary>
    /// A classe principal dojogo.
    /// </summary>
    public class GarotoGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        GamestateManager GM;
       /// <summary>
       /// O tamanho da tela.
       /// </summary>
        public static Vector2 tamanhoDaTela;
        public GarotoGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            //Necessário para API de storage do XNA
            this.Components.Add(new GamerServicesComponent(this));
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Obtém o tamanho da tela.
            tamanhoDaTela = new Vector2(GraphicsDevice.Viewport.Width/800, GraphicsDevice.Viewport.Height/600);
            // Cria um novo gerenciador de estados.
            GM = new GamestateManager(this,graphics,GamestateManager.Gamestate.Inicio,Services);
            // Mostra o mouse.
            IsMouseVisible = true;
           // graphics.ToggleFullScreen();
           
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            GM.LoadContent();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // Descarrega todo o conteúdo.
            GM.Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Atualiza o gerenciador de estados.
                GM.Update(gameTime);
            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
                GraphicsDevice.Clear(Color.CornflowerBlue);
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend);

            // Desenha pelo gerenciador de estados.
                GM.Draw(gameTime, spriteBatch);
               
                spriteBatch.End();
                base.Draw(gameTime);

        }
    }
}
