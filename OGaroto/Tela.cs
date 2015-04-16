using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;


namespace OGaroto
{
    /// <summary>
    /// Representa uma parte do jogo.
    /// </summary>
    abstract class Tela
    {
        #region Propriedades e accessors
        private bool isFuncionando = true;
        /// <summary>
        /// Determina se a tela se atualiza.
        /// </summary>
        public bool IsFuncionando
        {
            set { isFuncionando = value; }
            get { return isFuncionando; }
        }

        private Song musicaDeFundo;
        /// <summary>
        /// Representa a música de fundo da tela.
        /// </summary>
        public Song Musica
        {
            set { musicaDeFundo = value; }
            get { return musicaDeFundo; }
        }

        private GamestateManager gM;
        /// <summary>
        /// O gerenciador de estados do jogo.
        /// </summary>
        public GamestateManager GM
        {
            set { gM = value; }
            get { return gM; }
        }

        /// <summary>
        /// O centro da tela.
        /// </summary>
        public Vector2 Centro
        {
            get { return new Vector2(Tamanho.X / 2, Tamanho.Y / 2); }
        }

        private Vector2 tamanho;
        /// <summary>
        /// O tamanho da tela.
        /// </summary>
        public Vector2 Tamanho
        {
            set { tamanho = value; }
            get { return tamanho; }
        }

        List<Button> buttons;
        /// <summary>
        /// A lista de botões da tela.
        /// </summary>
        public List<Button> Buttons
        {
            set { buttons=value;}
            get { return buttons; }
        }

        private Texture2D fundo;
        /// <summary>
        /// A imagem de fundo da tela.
        /// </summary>
        public Texture2D Fundo
        {
            set { fundo = value; }
            get { return fundo; }
        }

        private string tipo;
        /// <summary>
        /// O tipo de tela.
        /// </summary>
        public string Tipo
        {
            set { tipo = value; }
            get { return tipo; }
        }

        #endregion

        #region Métodos
        /// <summary>
        /// Carrega recursos da tela.
        /// </summary>
        /// <param name="caminhoFundo">O caminho para a imagem de fundo da tela.</param>
        /// <param name="caminhoMusica">O caminho para a música de fundo da tela.</param>
        public void LoadContent(string caminhoFundo,string caminhoMusica)
        {
            Fundo = GM.Content.Load<Texture2D>(caminhoFundo);
            Musica = GM.Content.Load<Song>(caminhoMusica);
            Tamanho = new Vector2(Fundo.Width, Fundo.Height);
        }

        /// <summary>
        /// Carrega recursos da tela.
        /// </summary>
        /// <param name="caminhoFundo">O caminho para a imagem de fundo da tela.</param>
        public void LoadContent(string caminhoFundo)
        {
            Fundo = GM.Content.Load<Texture2D>(caminhoFundo);
            Tamanho = new Vector2(Fundo.Width, Fundo.Height);
        }

        /// <summary>
        /// Carrega a imagem dos botões da tela.
        /// </summary>
        /// <param name="caminhoBotao">O caminho da imagem dos botões.</param>
        public void LoadButtons(string caminhoBotao)
        {
            foreach (Button btn in Buttons)
            {
                btn.LoadContent(caminhoBotao);
            }
        }

        /// <summary>
        /// Desenha o fundo e os botões da tela.
        /// </summary>
        public void Draw(SpriteBatch SB)
        {
            SB.Draw(Fundo, Vector2.Zero, Color.White);

            foreach (Button btn in Buttons)
            {
                btn.Draw(SB);
            }
        }

        /// <summary>
        /// Adiciona um botão à tela.
        /// </summary>
        /// <param name="oTexto">O texto a ser exibido no botão.</param>
        /// <param name="aPosition">A posição inicial do botão.</param>
        /// <param name="aEscala">A escala de tamanho do botão.</param>
        public void AdicionarBotao(string oTexto, Vector2 aPosition, float aEscala)
        {
            Buttons.Add(new Button(this, aPosition, oTexto, aEscala));
        }

        /// <summary>
        /// Adiciona um botão à tela.
        /// </summary>
        /// <param name="oTexto">O texto a ser exibido no botão.</param>
        /// <param name="aEscala">A escala de tamanho do botão.</param>
        public void AdicionarBotao(string oTexto, float aEscala)
        {
            Buttons.Add(new Button(this, oTexto, aEscala));
        }

        /// <summary>
        /// Toca a música da tela, se o som estiver ativado.
        /// </summary>
        public void TocarMusica()
        {
            if (GM.IsSoundOn)
            {
                // Toca música.
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(Musica);
                }
            }
        }

       
        #endregion
    }
}
