using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OGaroto
{
    /// <summary>
    /// Representa um objeto que realiza alguma ação ao ser clicado.
    /// </summary>
    class Button:ObjetoFixo
    {
        #region Propriedades e accessors
        Tela tela;
        /// <summary>
        /// A tela à qual pertence.
        /// </summary>
        public Tela Tela
        {
            set { tela=value; }
            get { return tela; }
        }

        Texture2D imagemNormal;
        /// <summary>
        /// A imagem quando não está sendo clicado.
        /// </summary>
        public Texture2D ImagemNormal
        {
            set { imagemNormal = value; }
            get { return imagemNormal; }
        }

        Texture2D imagemApertado;
        /// <summary>
        /// A imagem quando está sendo clicado.
        /// </summary>
        public Texture2D ImagemApertado
        {
            set { imagemApertado = value; }
            get { return imagemApertado; }
        }

        Texture2D imagem2;
        /// <summary>
        /// A imagem por cima do botão.
        /// </summary>
        public Texture2D Imagem2
        {
            set { imagem2 = value; }
            get { return imagem2; }
        }

        string texto;
        /// <summary>
        /// O texto exibido no centro.
        /// </summary>
        public string Texto
        {
            set { texto = value; }
            get { return texto; }
        }

        /// <summary>
        /// A posição do texto.
        /// </summary>
        public Vector2 TextPosition
        {
    get {
        return new Vector2(this.Centro.X - (Tela.GM.Fonte.MeasureString(this.Texto).X * Escala / 2), this.Centro.Y - Tela.GM.Fonte.MeasureString(this.Texto).Y * Escala / 2);
        }
        }

        private float delay = 3000f;
        /// <summary>
        /// O tempo de espera até que o botão possa ser pressionado novamente.
        /// </summary>
        public float Delay
        {
            set { delay = value; }
            get { return delay / Fase.Acelerador; }
        }

        private float timer = 0;
        /// <summary>
        /// O contador de tempo.
        /// </summary>
        public float Timer
        {
            set { timer = value; }
            get { return timer; }
        }

        private bool isDisponivel =false;
        /// <summary>
        /// Determina se a fase se atualiza.
        /// </summary>
        public bool IsDisponivel
        {
            set { isDisponivel = value; }
            get { return isDisponivel; }
        }
        #endregion

        #region Construtores
        /// <param name="tela">A tela à qual pertencerá o botão.</param>
        /// <param name="aPosition">A posição.</param>
        /// <param name="oTexto">O texto a ser exibido.</param>
        public Button(Tela tela,Vector2 aPosition,string oTexto)
        {
            Tela = tela;
            Position = aPosition;
            Escala = 1f;
            Texto = oTexto;            
            Clicavel = true;
        }

        /// <param name="tela">A tela à qual pertencerá o botão.</param>
        /// <param name="aPosition">A posição.</param>
        /// <param name="oTexto">O texto a ser exibido.</param>
        /// <param name="aEscala">A escala de tamanho.</param>
        public Button(Tela tela,Vector2 aPosition,string oTexto,float aEscala)
        {
            Tela = tela;
            Position = aPosition;
            Escala = aEscala;
            Texto = oTexto;
            Clicavel = true;
        }

        /// <summary>
        /// Cria um botão sem definir sua posição inicial.
        /// </summary>
        /// <param name="tela">A tela à qual pertencerá o botão.</param>
        /// <param name="oTexto">O texto a ser exibido.</param>
        /// <param name="aEscala">A escala de tamanho.</param>
        public Button(Tela tela, string oTexto, float aEscala)
        {
            Tela = tela;
            Escala = aEscala;
            Texto = oTexto;
            Clicavel = true;
        }

        /// <summary>
        /// Cria um botão sem definir sua posição inicial.
        /// </summary>
        /// <param name="aFase">A fase à qual pertencerá o botão.</param>
        /// <param name="caminho2">A imagem representativa do botão.</param>
        /// <param name="aEscala">A escala de tamanho.</param>
        public Button(Fase  aFase, string caminho2, float aEscala)
        {
            Fase = aFase;
            Escala = aEscala;
            Imagem2 = Fase.Content.Load<Texture2D>(caminho2);
            Clicavel = true;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Define a posição do botão.
        /// </summary>
        /// <param name="aPosition">A nova posição.</param>
        public void SetPosition(Vector2 aPosition)
        {
            Position = aPosition;
        }

        /// <summary>
        /// Carrega os recursos do botão.
        /// </summary>
        /// <param name="caminhoImagem"></param>
        public new void LoadContent(string caminhoImagem)
        {
            if (!(Tela == null))
            {
                ImagemNormal = Tela.GM.Content.Load<Texture2D>(caminhoImagem);
                ImagemApertado = Tela.GM.Content.Load<Texture2D>(caminhoImagem + "apertado");
            }
            else if (!(Fase == null))
            {
                ImagemNormal = Fase.Content.Load<Texture2D>(caminhoImagem);
                ImagemApertado = Fase.Content.Load<Texture2D>(caminhoImagem + "apertado");
            }

            Imagem = ImagemNormal;
            Tamanho = new Vector2(Imagem.Width,Imagem.Height);
        }

        /// <summary>
        /// Atualiza o botão.
        /// </summary>
        public void Update()
        {
            if (Tela != null)
            {
                Imagem = Tela.GM.MouseManager.MouseEmCima(this) ? ImagemApertado : ImagemNormal;
            }
            else
            {
                Imagem = Fase.GM.MouseManager.MouseEmCima(this) ? ImagemApertado : ImagemNormal;
            }
            
        }

        /// <summary>
        /// Desenha a imagem e o texto do botão.
        /// </summary>
        public void Draw(SpriteBatch SB)
        {
            SB.Draw(Imagem, Position, null, Color.White, 0f, Vector2.Zero, Escala, SpriteEffects.None, 0);
            SB.DrawString(Tela.GM.Fonte, Texto, TextPosition, Color.Black, 0f, Vector2.Zero, Escala, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Desenha o fundo e a imagem do botão.
        /// </summary>
        public void Draw(GameTime GT,SpriteBatch SB)
        {
            SB.Draw(Imagem, Position, null, Color.White, 0f, Vector2.Zero, Escala, SpriteEffects.None, 0);
            SB.Draw(Imagem2, new Vector2(Centro.X - (Imagem2.Width / 2), Centro.Y - (Imagem2.Height / 2)),Color.White);
        }
        #endregion
    }
}
