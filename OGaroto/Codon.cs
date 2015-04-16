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
    /// Representa um objeto no qual um Amino deve ser encaixado.
    /// </summary>
    class Codon : ObjetoMovel
    {
        #region Propriedades e accessors
        private bool isInfectado=false;
        /// <summary>
        /// Determina se o Codon está infectado por Parasita.
        /// </summary>
        public bool IsInfectado
        {
            set { isInfectado = value; }
            get { return isInfectado; }
        }
     
        private string tipo;
        /// <summary>
        /// O tipo de Codon, de 1 a 7.
        /// </summary>
        public string Tipo
        {
            set { tipo = value; }
            get { return tipo; }
        }
        #endregion

        #region Construtor
        /// <param name="aFase">A fase atual.</param>
        /// <param name="tipo">O tipo, de 1 a 7.</param>
        public Codon(Fase aFase, string tipo)
        {
            Tipo = tipo;
            this.Fase = aFase;
            LoadContent("Codons/codon" + tipo);
            Tamanho = new Vector2(Imagem.Width, Imagem.Height);
            Escala = 1.0f;
            Position = new Vector2(Fase.Tamanho.X, Fase.Tamanho.Y - this.Tamanho.Y);
            Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
            velocidade.X = Fase.VelocidadeDaTela.X*0.75f;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Atualiza o Codon.
        /// </summary>
        new public void Update(GameTime GT)
        {
            if (IsFuncionando)
            {
                // Anda com a velocidade da tela.
                this.position.X += Velocidade.X;

                // Morre se sair da tela.
                if (ForaDaTela())
                {
                    IsAlive = false;
                }
            }
        }

        /// <summary>
        /// Desenha o Codon se estiver visível.
        /// </summary>
       new public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (IsVisible)
            {
                spriteBatch.Draw(Imagem, Position, null, Color.White, 0f, Vector2.Zero, Escala, SpriteEffects.None, 0);
            }
           }

        /// <summary>
        /// Determina se o Codon saiu da tela pela esquerda.
        /// </summary>
       new public bool ForaDaTela()
       {
           if (this.Retangulo.Right < 0)
           {
               return true;
           }
           else return false;
       }
        #endregion
    }
}
