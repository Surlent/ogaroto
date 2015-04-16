using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OGaroto
{
    /// <summary>
    /// Define um objeto que não muda sua posição na tela.
    /// </summary>
     class ObjetoFixo:IDisposable
    {
        #region Propriedades e accessors
        private Fase fase;
        /// <summary>
        /// A fase atual.
        /// </summary>
        public Fase Fase
        {
            set { fase = value; }
            get { return fase; }
        }

        private bool isVisible = true;
        /// <summary>
        /// Determina se está visível.
        /// </summary>
        public bool IsVisible
        {
            set { isVisible = value; }
            get { return isVisible; }
        }

        private bool clicavel = false;
        /// <summary>
        /// Determina se pode ser clicado.
        /// </summary>
        public bool Clicavel
        {
            set { clicavel = value; }
            get { return clicavel; }
        }

        private float escala = 1.0f;
        /// <summary>
        /// Determina a escala de tamanho.
        /// </summary>
        public float Escala
        {
            set { escala = value; }
            get { return escala; }
        }

        protected Vector2 position;
        /// <summary>
        /// Determina a posição.
        /// </summary>
        public Vector2 Position
        {
            set { position = value; }
            get { return position; }
        }

        protected Vector2 tamanho;
        /// <summary>
        /// Determina o tamanho.
        /// </summary>
        public Vector2 Tamanho
        {
            set { tamanho = value; }
            get { return tamanho* Escala; }
        }

        protected Rectangle retangulo;
        /// <summary>
        /// Determina o retângulo em torno.
        /// </summary>
        public Rectangle Retangulo
        {
            set {
                Position = new Vector2(value.X, value.Y);
                retangulo = value; }
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y); }
        }

        /// <summary>
        /// Determina o centro.
        /// </summary>
        public Vector2 Centro
        {
            set
            {
                position.X = value.X - (Tamanho.X/2);
                position.Y = value.Y - (Tamanho.Y/2);

            }
            get { return new Vector2(Position.X + (Tamanho.X / 2), Position.Y + (Tamanho.Y / 2)); }
        }

        private Color[] dadosTextura;
        /// <summary>
        /// Os dados da textura usada, em array de cores.
        /// </summary>
        public Color[] DadosTextura
        {
            set { dadosTextura = value; }
            get { return dadosTextura; }
        }

        protected Texture2D imagem;
        /// <summary>
        /// A textura usada.
        /// </summary>
        public Texture2D Imagem
        {
            set { imagem = value; }
            get { return imagem; }
        }
        #endregion

        #region Métodos
        
        /// <summary>
        /// Carrega recursos.
        /// </summary>
        /// <param name="caminho">O caminho da imagem.</param>
        public void LoadContent(string caminho)
        {
            Imagem = Fase.Content.Load<Texture2D>(caminho);
            DadosTextura = new Color[Imagem.Width * Imagem.Height];
            Imagem.GetData(DadosTextura);
        }

        /// <summary>
        /// Desenha o objeto de interface com escala.
        /// </summary>
        /// <param name="Escala">A escala horizontal e vertical do objeto.</param>
        public void Draw(SpriteBatch SB, Vector2 Escala)
        {
            SB.Draw(Imagem, Position, null, Color.White, 0, Vector2.Zero, Escala, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Determina se há colisão de pixels com um objeto.
        /// </summary>
        /// <param name="o">O objeto possivelmente em colisão.</param>
        public bool ColisaoPixel(ObjetoFixo o)
        {
            // Obtém um retângulo de interseção.
            int topo = Math.Max(this.Retangulo.Top, o.Retangulo.Top);
            int fundo = Math.Min(this.Retangulo.Bottom, o.Retangulo.Bottom);
            int esquerda = Math.Max(this.Retangulo.Left, o.Retangulo.Left);
            int direita = Math.Min(this.Retangulo.Right, o.Retangulo.Right);

            // Busca dentro do retângulo de interseção por pixels comuns aos dois objetos em colisão
            for (int y = topo; y < fundo; y++)
            {
                for (int x = esquerda; x < direita; x++)
                {
                    Color colorA = DadosTextura[(x - this.Retangulo.Left) +
                                (y - this.Retangulo.Top) * this.Retangulo.Width];
                    Color colorB = o.DadosTextura[(x - o.Retangulo.Left) +
                                (y - o.Retangulo.Top) * o.Retangulo.Width];

                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Determina se há colisão de retângulos.
        /// </summary>
        /// <param name="retangulo">O retângulo com o qual se verificará a colisão.</param>
        public bool Colisao(Rectangle retangulo)
        {
            if (this.Retangulo.Intersects(retangulo))
            {
                return true;
            }
            else return false;

        }

        /// <summary>
        /// Descarta a imagem.
        /// </summary>
       public void Dispose()
       {
           imagem.Dispose();
       }
        #endregion
    }
}
