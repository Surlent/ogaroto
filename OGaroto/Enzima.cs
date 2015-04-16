using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
namespace OGaroto
{
    /// <summary>
    /// Representa um ser que acelera o jogo.
    /// </summary>
    class Enzima:ObjetoMovel
    {
        #region Propriedades e accessors
        private float velExtra;
        /// <summary>
        /// A velocidade que a Enzima adiciona ao jogo.
        /// </summary>
        public float VelExtra
        {
            set { velExtra = value; }
            get { return velExtra; }
        }

        private bool neutralizada=false;
        /// <summary>
        /// Indica se a enzima foi neutralizada.
        /// </summary>
        public bool Neutralizada
        {
            set { neutralizada = value; }
            get { return neutralizada; }
        }

        #endregion

        #region Construtores
        /// <param name="aFase">A fase atual.</param>
        /// <param name="aPosition">A posição inicial.</param>
        /// <param name="velocidadeExtra">O aumento de velocidade.</param>
        public Enzima(Fase aFase, Vector2 aPosition,float velocidadeExtra)
        {
            Fase = aFase;
            Position = aPosition;
            LoadContent(ref animPadrao, "Obstaculos/Enzima", 0.12f);
            LoadSom(Fase.SomEnzima);
            Tamanho = new Vector2(AnimPadrao.FrameWidth, AnimPadrao.FrameHeight);
            Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
            Velocidade =Fase.VelocidadeDaTela;
            VelExtra = velocidadeExtra;
            Valor = 1000;
        }

        /// <param name="aFase">A fase atual.</param>
        /// <param name="aPosition">A posição inicial.</param>
        /// <param name="velocidadeExtra">O aumento de velocidade.</param>
        /// <param name="aEscala">A escala de tamanho.</param>
         public Enzima(Fase aFase, Vector2 aPosition,float velocidadeExtra, float aEscala)
        {
            Fase = aFase;
            Position = aPosition;
            Escala = aEscala;
            LoadContent(ref animPadrao, "Obstaculos/Enzima", 0.12f);
            LoadSom(Fase.SomEnzima);
            Tamanho = new Vector2(AnimPadrao.FrameWidth, AnimPadrao.FrameHeight);
            Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
            Velocidade = Fase.VelocidadeDaTela;
            VelExtra = velocidadeExtra;
            Valor = 1000;
        }


         /// <param name="aFase">A fase atual.</param>
         /// <param name="aPosition">A posição inicial.</param>
        /// <param name="aDestino">A posição da tela para o qual rumará.</param>
         /// <param name="velocidadeExtra">O aumento de velocidade.</param>
         /// <param name="aEscala">A escala de tamanho.</param>
         public Enzima(Fase aFase, Vector2 aPosition, Vector2 aDestino,float velocidadeExtra, float aEscala)
         {
             Fase = aFase;
             Position = aPosition;
             Escala = aEscala;
             LoadContent(ref animPadrao, "Obstaculos/Enzima", 0.12f);
             LoadSom(Fase.SomEnzima);
             Tamanho = new Vector2(AnimPadrao.FrameWidth, AnimPadrao.FrameHeight);
             Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
             DefinirVelocidade(aDestino, Fase.VelocidadeDeJogo);
             VelExtra = velocidadeExtra;
             Valor = 1000;
         }
        #endregion

        #region Métodos
         /// <summary>
        /// Atualiza a Enzima.
        /// </summary>
        public override void Update(GameTime GT)
         {
             if (IsFuncionando)
             {
                 // Caso não esteja ativada, ativar.
                 if ((!(IsAtivo))&&(!(Neutralizada)))
                 {
                     Acelerar();
                     
                     AtivarSonzinho();
                 }
                
                 DesviarObjeto(Fase.AminoArrastando);

                 // Atualiza a posição na tela.
                 this.position.X += Velocidade.X;
                 this.position.Y += Velocidade.Y;

                 // Remove o efeito e morre ao sair da tela.
                 if (this.ForaDaTela())
                {
                    if (IsAtivo)
                    {
                        Normalizar();
                     IsAlive = false;
                    }
                 }
             }
         }

        /// <summary>
        /// Aumenta a velocidade do jogo.
        /// </summary>
         public void Acelerar()
         {
             Fase.Acelerador += VelExtra;
             IsAtivo = true;
         }

        /// <summary>
        /// Normaliza a velocidade do jogo.
        /// </summary>
         public void Normalizar()
         {
             Fase.Acelerador -=VelExtra;
             IsAtivo = false;
         }
#endregion
    }
}
