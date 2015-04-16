using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OGaroto
{
    /// <summary>
    /// Representa um ser que causa mutações nos Codons, alterando seu tipo.
    /// </summary>
    class Radion:ObjetoMovel
    {
        #region Propriedades e accessors
        private float delay = 3000f;
        /// <summary>
        /// O intervalo entre cada raio.
        /// </summary>
        public float Delay
        {
            set { delay = value; }
            get { return delay/Fase.Acelerador; }
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

        private Random rand;
        /// <summary>
        /// O gerador de números aleatórios.
        /// </summary>
        public Random Rand
        {
            set { rand = value; }
            get { return rand; }
        }
        #endregion

        #region Construtores
        /// <param name="aFase">A fase atual.</param>
     /// <param name="aPosition">A posição inicial.</param>
       public Radion(Fase aFase, Vector2 aPosition)
    {
        Fase = aFase;
        Position = aPosition;
        LoadContent(ref animPadrao, "Obstaculos/Radion", 0.12f);
        LoadSom(Fase.SomRadion);
        Tamanho = new Vector2(AnimPadrao.FrameWidth, AnimPadrao.FrameHeight);
        Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
        Velocidade =Fase.VelocidadeDaTela;
        Rand = new Random();
        IsAtivo = true;
    }

       /// <param name="aFase">A fase atual.</param>
     /// <param name="aPosition">A posição inicial.</param>
     /// <param name="aEscala">A escala de tamanho.</param>
       public Radion(Fase aFase, Vector2 aPosition, float aEscala)
    {
        Fase = aFase;
        Position = aPosition;
        Escala = aEscala;
        LoadContent(ref animPadrao, "Obstaculos/Radion", 0.12f);
        LoadSom(Fase.SomRadion);
        Tamanho = new Vector2(AnimPadrao.FrameWidth, AnimPadrao.FrameHeight);
        Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
        Velocidade = Fase.VelocidadeDaTela;
        Rand = new Random();
        IsAtivo = true;
    }

       /// <param name="aFase">A fase atual.</param>
       /// <param name="aPosition">A posição inicial.</param>
       /// <param name="aDestino">O ponto para o qual rumará a Enzima.</param>
       /// <param name="aEscala">A escala de tamanho.</param>
       public Radion(Fase aFase, Vector2 aPosition, Vector2 aDestino, float aEscala)

       {
           Fase = aFase;
           Position = aPosition;
           Escala = aEscala;
           LoadContent(ref animPadrao, "Obstaculos/Radion", 0.12f);
           LoadSom(Fase.SomRadion);
           Tamanho = new Vector2(AnimPadrao.FrameWidth, AnimPadrao.FrameHeight);
           Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
           DefinirVelocidade(aDestino, 1);
           Rand = new Random();
           IsAtivo = true;
           Valor = 8000;
       }
        #endregion

        #region Métodos
       /// <summary>
    /// Atualiza o Radion.
    /// </summary>
       public override void Update(GameTime GT)
       {
           if (IsFuncionando)
           {
               // Morre se sair da tela.
               if (this.ForaDaTela())
               {
                   IsAlive = false;
               }

               DesviarObjeto(Fase.AminoArrastando);
            
               // Atualiza a posição na tela.
               this.position.X += Velocidade.X;
               this.position.Y += Velocidade.Y;

               
               PrepararRaios(GT);
              
           }
       }

        /// <summary>
        /// Emite raios a cada intervalo de tempo correspondente ao delay.
        /// </summary>
        /// <param name="GT"></param>
           public void PrepararRaios(GameTime GT)
           {
               if (IsAtivo)
               {
                   if (Timer > Delay)
                   {
                       DispararRaios();
                      AtivarSonzinho();
                       timer = 0f;
                   }
                   else
                   {
                       timer += GT.ElapsedGameTime.Milliseconds;
                   }
               }
           }

        /// <summary>
        /// Causa mutações nos Codons.
        /// </summary>
           public void DispararRaios()
           {
               // Altera o tipo e a imagem de cada Codon em jogo.
                   foreach (Codon c in Fase.Codons)
                   {
                       if (c.IsAlive)
                       {
                           if (Fase.AminoEsperando == null)
                           {
                               c.Tipo = Rand.Next(1, 8).ToString();
                               c.Imagem = Fase.Content.Load<Texture2D>("Codons/codon" + c.Tipo);
                           }
                           else if (Fase.AminoEsperando.CodonEncaixado != c)
                               {
                                   c.Tipo = Rand.Next(1, 8).ToString();
                                   c.Imagem = Fase.Content.Load<Texture2D>("Codons/codon" + c.Tipo);
                               }
                           
                       }
                   }

           }

        /// <summary>
        /// Altera o intervalo de tempo entre as radiações.
        /// </summary>
        /// <param name="oDelay"></param>
           public void AlterarDelay(float oDelay)
           {
               Delay = oDelay;
           }
       #endregion
    }
}
