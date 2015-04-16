using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using OGaroto.Save;

namespace OGaroto
{
    /// <summary>
    /// Representa a tela de escolha de níveis.
    /// </summary>
    class TelaGaroto:Tela
    {
        //Implementa a interface de save
        private SaveGameManager SM = new SaveGameManager("OGaroto", "OGaroto.svg");

        #region Propriedades e accessors
        private Garoto boy;
        /// <summary>
        /// O garoto cujas partes são conseguidas durante o jogo.
        /// </summary>
        public Garoto Boy
        {
            set { boy = value; }
            get { return boy; }
        }

        private float timer = 0;
        /// <summary>
        /// Contador de tempo.
        /// </summary>
        public float Timer
        {
            set { timer = value; }
            get { return timer; }
        }

        private bool hasSaved;
        /// <summary>
        /// Indica se o jogo foi salvo.
        /// </summary>
        public bool HasSaved
        {
            set { hasSaved = value; }
            get { return hasSaved; }
        }
        
        #endregion

        #region Construtor
        /// <param name="oGM">O gerenciador de estados do jogo.</param>
        public TelaGaroto(GamestateManager oGM)
        {
            GM = oGM;
            Tipo = "Garoto";
            Tamanho = new Vector2(oGM.Game.GraphicsDevice.Viewport.Width, oGM.Game.GraphicsDevice.Viewport.Height);
            Boy = new Garoto(this);
            LoadGaroto();
            Buttons = new List<Button>();
            LoadContent("Fundos/garoto","Sons/Musicas/Suspense");          
            AdicionarBotao("Voltar", new Vector2(500, 500), 1f);
            AdicionarBotao("Salvar", new Vector2(50, 500), 1f);
            LoadButtons("Interface/Buttons/buttonMaior");

        }
        #endregion

        #region Métodos
        public void LoadGaroto()
        {
            
            Boy.LoadCorpo("0");
        }

        /// <summary>
        /// Atualiza a tela.
        /// </summary>
        public void Update(GameTime GT)
        {

            if (IsFuncionando)
            {
                TocarMusica();
                if (HasSaved)
                {
                    if (timer < 10000)
                    {
                        Timer += GT.ElapsedGameTime.Milliseconds;
                    }
                    else
                    {
                        HasSaved = false;
                        Timer = 0;
                    }
                }
                foreach (Button btn in Buttons)
                {

                    btn.Update();
                    if (GM.MouseManager.NovoClique(btn))
                    {
                        switch(btn.Texto)
                        {
                            case "Voltar":
                            {
                                GM.Estado = GamestateManager.Gamestate.Inicio;
                            }
                            break;

                            case "Salvar":
                            {
                                SaveData dados = new SaveData();
                                dados.PartesAlteradas= new List<string>();
                                dados.PartesAtivadas = new List<string>();
                                foreach (Parte p in Boy.Corpo)
                                {
                                    if (p.Clicavel)
                                    { dados.PartesAtivadas.Add(p.Tipo); }

                                    if (p.Numero == "1")
                                    {
                                        dados.PartesAlteradas.Add(p.Tipo);
                                    }
                                }

                                SM.Salvar(dados);
                                HasSaved = true;
                            }
                            break;
                        }
                    }
                }

                Boy.Update();

                foreach (Parte p in Boy.Corpo)
                {
                    if ((GM.MouseManager.NovoClique(p))&&(p==Boy.ParteFocada))
                    {
                        GM.Fase = new Fase(GM, p.Tipo);
                        GM.Fase.LoadContent();
                        GM.Estado = GamestateManager.Gamestate.Jogo;
                        MediaPlayer.Stop();
                    }
                }

                // Esc volta ao início.
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    GM.Estado = GamestateManager.Gamestate.Inicio;
                }
            }
        }

        public void DrawGaroto(SpriteBatch SB)
        {
            Boy.Draw(SB);
        }
        /// <summary>
        /// Desenha o texto da tela.
        /// </summary>
        public void DrawTexto(SpriteBatch SB)
        {
            if (HasSaved)
            {
                string oTexto1 = "Jogo salvo com sucesso.";
                SB.DrawString(GM.Fonte, oTexto1, new Vector2(this.Centro.X - (GM.Fonte.MeasureString(oTexto1).X / 2) + 1, 300), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
                SB.DrawString(GM.Fonte, oTexto1, new Vector2(this.Centro.X - (GM.Fonte.MeasureString(oTexto1).X / 2), 300), Color.Lime, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            }
            // Desenha no centro da tela.
            string oTexto = "Escolha uma parte do Garoto";
            SB.DrawString(GM.Fonte, oTexto, new Vector2(this.Centro.X - (GM.Fonte.MeasureString(oTexto).X / 2)+1, 50+1), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
            SB.DrawString(GM.Fonte, oTexto, new Vector2(this.Centro.X - (GM.Fonte.MeasureString(oTexto).X / 2), 50), Color.MintCream, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
        }
        #endregion
    }
}
