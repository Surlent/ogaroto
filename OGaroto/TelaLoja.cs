using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OGaroto
{
    /// <summary>
    /// A tela na qual são compradas novas partes.
    /// </summary>
    class TelaLoja:Tela
    {
        #region Propriedades e accessors
        private List<Parte> mercadorias;
        /// <summary>
        /// As partes à venda.
        /// </summary>
        public List<Parte> Mercadorias
        {
            set { mercadorias = value; }
            get { return mercadorias; }
        }

        private List<Rectangle> slots;
        /// <summary>
        /// Os espaços para partes sendo vendidas.
        /// </summary>
        public List<Rectangle> Slots
        {
            set { slots = value; }
            get { return slots; }
        }

        private Texture2D quadro;
        /// <summary>
        /// O quadro sobre o qual ficam as partes.
        /// </summary>
        public Texture2D Quadro
        {
            set { quadro = value; }
            get { return quadro; }
        }
        #endregion

        #region Construtor
        /// <param name="oGM">O gerenciador de estados do jogo.</param>
        public TelaLoja(GamestateManager oGM)
        {
            GM = oGM;
            Tipo = "Loja";
            Tamanho = new Vector2(oGM.Game.GraphicsDevice.Viewport.Width, oGM.Game.GraphicsDevice.Viewport.Height);

            #region Posicionamento de Slots
            Slots = new List<Rectangle>(8);
            Slots.Add(new Rectangle((int)(Centro.X -200),(int)( Centro.Y - 100), 100, 100));
            Slots.Add(new Rectangle((int)(Centro.X - 100), (int)(Centro.Y - 100), 100, 100));
            Slots.Add(new Rectangle((int)(Centro.X), (int)(Centro.Y - 100), 100, 100));
            Slots.Add(new Rectangle((int)(Centro.X + 100), (int)(Centro.Y - 100),100, 100));
            Slots.Add(new Rectangle((int)(Centro.X - 200), (int)(Centro.Y), 100, 100));
            Slots.Add(new Rectangle((int)(Centro.X - 100), (int)(Centro.Y), 100, 100));
            Slots.Add(new Rectangle((int)(Centro.X), (int)(Centro.Y), 100, 100));
            Slots.Add(new Rectangle((int)(Centro.X + 100), (int)(Centro.Y), 100, 100));
            #endregion

            #region Carregamento de mercadorias
            Mercadorias = new List<Parte>();
            LoadMercadoria(new Parte(this,"head","1"));
            LoadMercadoria(new Parte(this,"leftLeg", "1"));
            LoadMercadoria(new Parte(this, "leftArm", "0"));
            LoadMercadoria(new Parte(this, "rightFoot", "1"));
            LoadMercadoria(new Parte(this, "nose", "1"));
            LoadMercadoria(new Parte(this, "rightLeg", "1"));
            LoadMercadoria(new Parte(this, "nose", "0"));
            LoadMercadoria(new Parte(this, "eyes", "1"));
            PosicionarMercadorias();
            #endregion

            Quadro = oGM.Content.Load<Texture2D>("Interface/Misc/quadro");
            Buttons = new List<Button>();
            LoadContent("Fundos/garoto","Sons/Musicas/Suspense");          
            AdicionarBotao("Voltar", new Vector2(500, 500), 1f);
            LoadButtons("Interface/Buttons/buttonMaior");
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Atualiza a tela.
        /// </summary>
        public void Update()
        {
            if (IsFuncionando)
            {

                foreach (Button btn in Buttons)
                {

                    btn.Update();
                    if (GM.MouseManager.NovoClique(btn))
                    {
                        // Escolhe o efeito com base no texto do botão.
                        switch (btn.Texto)
                        {
                            case "Voltar":
                                {
                                    GM.Estado = GamestateManager.Gamestate.Garoto;

                                } break;

                        }
                    }
                }
            }
        }

        /// <summary>
        /// Desenha texto na tela.
        /// </summary>
        public void DrawTexto(SpriteBatch SB)
        {
            // Desenha no centro da tela.
            string oTexto = "Loja";
            SB.DrawString(GM.Fonte, oTexto, new Vector2(this.Centro.X - (GM.Fonte.MeasureString(oTexto).X), 30)+new Vector2(1,1), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
            SB.DrawString(GM.Fonte, oTexto, new Vector2(this.Centro.X - (GM.Fonte.MeasureString(oTexto).X), 30), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Adiciona uma mercadoria à lista de venda.
        /// </summary>
        /// <param name="aMercadoria">A mercadoria a ser adicionada.</param>
        public void LoadMercadoria(Parte aMercadoria)
        {
            Mercadorias.Add(aMercadoria);
        }

        /// <summary>
        /// Posiciona as mercadorias de acordo com os Slots da tela.
        /// </summary>
        public void PosicionarMercadorias()
        {
            for(int i=0;i<Slots.Count&&i<Mercadorias.Count;i++)
            {
               
                Mercadorias[i].EscalaVetorial = new Vector2(100 / Mercadorias[i].Tamanho.X/2, 100 / Mercadorias[i].Tamanho.Y/2);
               // Mercadorias[i].Retangulo = Slots[i];   
            }
        }

        /// <summary>
        /// Desenha as mercadorias a venda.
        /// </summary>
        public void DrawMercadorias(SpriteBatch SB)
        {
                for(int i=0;i<Slots.Count;i++)
                {
                    SB.Draw(Quadro,Slots[i], Color.White);
                    
                }

                for (int i = 0; i < Mercadorias.Count; i++)
                {
                    Mercadorias[i].Position = new Vector2(Slots[i].X,Slots[i].Y)+new Vector2(2,2);
                    Mercadorias[i].Draw(SB, new Vector2(Slots[i].Width/Mercadorias[i].Tamanho.X*0.95f,Slots[i].Height/Mercadorias[i].Tamanho.Y*0.95f));
                }
        }
        #endregion
    }
}
