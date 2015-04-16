using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
    /// Define um objeto que muda sua posição na tela.
    /// </summary>
    abstract class ObjetoMovel:ObjetoFixo
    {
        #region Propriedades e accessors
        private bool isAtivo = false;
        /// <summary>
        /// Determina se o objeto está com seu efeito especial ativo.
        /// </summary>
        public bool IsAtivo
        {
            set { isAtivo = value; }
            get { return isAtivo; }
        }

        private bool isFuncionando = true;
        /// <summary>
        /// Determina se o objeto se atualiza.
        /// </summary>
        public bool IsFuncionando
        {
            set { isFuncionando = value; }
            get { return isFuncionando; }
        }

        private SoundEffect sonzinho;
        /// <summary>
        /// O som característico do objeto.
        /// </summary>
        public SoundEffect Sonzinho
        {
            set { sonzinho = value; }
            get { return sonzinho; }
        }     

        private int valor;
        /// <summary>
        /// O valor em relação à pontuação do jogo.
        /// </summary>
        public int Valor
        {
            set { valor = value; }
            get { return valor; }
        }

        private float rotation = 0f;
        /// <summary>
        /// A inclinação da imagem, em radianos.
        /// </summary>
        public float Rotation
        {
            set { rotation = value; }
            get { return rotation; }
        }

        private bool isAlive = true;
        /// <summary>
        /// Determina se o objeto está vivo.
        /// </summary>
        public bool IsAlive
        {
            set { isAlive = value; }
            get { return isAlive; }
        }

        protected Animation animPadrao;
        /// <summary>
        /// Representa a animação comum do objeto.
        /// </summary>
        public Animation AnimPadrao
        {
            set { animPadrao = value; }
            get { return animPadrao; }
        }

        protected Animation animDescarte;
        /// <summary>
        /// Representa a animação exibida ao descartar-se.
        /// </summary>
        public Animation AnimDescarte
        {
            set { animDescarte = value; }
            get { return animDescarte; }
        }

        protected Vector2 velocidade;
        /// <summary>
        /// Determina a velocidade.
        /// </summary>
        public Vector2 Velocidade
        {
            set { velocidade = value; }
            get { return velocidade *Fase.VelocidadeDeJogo; }
        }

        /// <summary>
        /// Exibe uma animação.
        /// </summary>
        protected AnimationPlayer sprite;
        #endregion

        #region Métodos
        /// <summary>
        /// Carrega o som característico.
        /// </summary>
        /// <param name="som">O som característico.</param>
        public void LoadSom(SoundEffect som)
        {
            Sonzinho = som;
        }

        /// <summary>
        /// Carrega uma animação.
        /// </summary>
        /// <param name="animation">O atributo que receberá a animação.</param>
        /// <param name="caminho">O caminho da animação.</param>
        public void LoadContent(ref Animation animation, string caminho)
        {
            animation = new Animation(Fase.Content.Load<Texture2D>(caminho), 0.1f, true);
            DadosTextura = new Color[animation.Texture.Width * animation.Texture.Height];
            animation.Texture.GetData(DadosTextura);
        }

        /// <summary>
        /// Carrega uma animação e escolhe a duração de cada frame.
        /// </summary>
        /// <param name="animation">O atributo que receberá a animação.</param>
        /// <param name="caminho">O caminho da animação.</param>
        /// <param name="duracao">A duração de cada frame.</param>
         public void LoadContent(ref Animation animation, string caminho,float duracao)
        {
            animation = new Animation(Fase.Content.Load<Texture2D>(caminho), duracao, true);
            DadosTextura = new Color[animation.Texture.Width * animation.Texture.Height];
            animation.Texture.GetData(DadosTextura);
        }

        /// <summary>
        /// Atualiza o objeto.
        /// </summary>
        public virtual void Update(GameTime GT)
        {
            if (IsFuncionando)
            {
                this.position.X += Velocidade.X;
                this.position.Y += Velocidade.Y;
            }
        }

        /// <summary>
        /// Desenha a animação do objeto.
        /// </summary>
        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                sprite.PlayAnimation(animPadrao);
                sprite.Draw(gameTime, spriteBatch, Position, SpriteEffects.None, Escala,Rotation,Fase.VelocidadeDeJogo);
            }
            }

        /// <summary>
        /// Determina a direção e a velocidade com base num alvo.
        /// </summary>
        /// <param name="pontoAlvo">O alvo na tela.</param>
        /// <param name="velo">A velocidade com que o objeto se moverá.</param>
        public void DefinirVelocidade(Vector2 pontoAlvo, float velo)
        {
            Velocidade = velo * Vector2.Normalize(Vector2.Subtract(pontoAlvo, Centro));

        }

        /// <summary>
        /// Toca um som.
        /// </summary>
        /// <param name="som">O som a ser tocado.</param>
        public void AtivarSonzinho(SoundEffect som)
        {
            if (Fase.GM.IsSoundOn)
            {
                som.Play();
            }
        }

        /// <summary>
        /// Toca o som característico.
        /// </summary>
        public void AtivarSonzinho()
        {
            if (Fase.GM.IsSoundOn)
            {
                Sonzinho.Play();
            }
        }

       
        /// <summary>
        /// Determina se o objeto está fora da tela.
        /// </summary>
        /// <returns></returns>
        public bool ForaDaTela()
        {
            if ((this.Retangulo.Right < 0)||(this.Retangulo.Top>Fase.Tamanho.Y)||(this.Retangulo.Left>Fase.Tamanho.X)||(this.Retangulo.Bottom<0))
            {
                return true;
            }
            else return false;
        }

        /// <summary>
        /// Empurra um objeto para longe, se houver colisão de pixels.
        /// </summary>
        /// <param name="o">O objeto a ser empurrado.</param>
        public void DesviarObjeto(ObjetoMovel o)
        {
            if (o != null)
            {
                if (ColisaoPixel(o))
                {
                    o.Position += new Vector2(Velocidade.X * Fase.VelocidadeDeJogo/3, Velocidade.Y * Fase.VelocidadeDeJogo/3);
                    Fase.MouseManager.Position += new Vector2(Velocidade.X * Fase.VelocidadeDeJogo/3, Velocidade.Y * Fase.VelocidadeDeJogo/3);
                }
            }
        }
        #endregion
    }
}
