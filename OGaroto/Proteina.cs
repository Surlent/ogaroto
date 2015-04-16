using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OGaroto
{
    /// <summary>
    /// Representa uma cadeia de Aminos interligados.
    /// </summary>
    class Proteina
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

        /// <summary>
        /// O número de Aminos na proteína.
        /// </summary>
        public int ContAmino
        {
            get { return Aminos.Count; }
        }

        /// <summary>
        /// O último Amino da proteína.
        /// </summary>
        public Amino UltimoAmino
        {
            get { return Aminos.Last<Amino>(); }
        }

        /// <summary>
        /// O primeiro Amino da proteína.
        /// </summary>
        public Amino PrimeiroAmino
        {
            get { return Aminos.First<Amino>(); }
        }

        List<Amino> aminos;
        /// <summary>
        /// A lista de Aminos na proteína.
        /// </summary>
        public List<Amino> Aminos
        {
            set { aminos = value; }
            get { return aminos; }
        }
        #endregion

        #region Construtor
        /// <param name="aFase">A fase atual.</param>
        public Proteina(Fase aFase)
        {
            Fase = aFase;
            Aminos = new List<Amino>();
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Atualiza os Aminos da proteína.
        /// </summary>
        public void Update(GameTime GT)
        {
            try
            {
                foreach (Amino a in Aminos)
                {
                    a.Update();
                }
            }
            catch (Exception e)
            { 
            }

        }

        /// <summary>
        /// Desenha os Aminos da proteína.
        /// </summary>
        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Amino a in Aminos)
            {
                a.Draw(gameTime,spriteBatch);
            }
        }

        /// <summary>
        /// Adiciona um Amino à proteína.
        /// </summary>
        /// <param name="a">O Amino a ser adicionado.</param>
        public void AdicionarAmino(Amino a)
        {
           
            Aminos.Add(a);
            
            if (Aminos.Count>0)
            {
                foreach (Amino amin in Aminos)
                {
                    // Reduz levemente o tamanho dos Aminos na proteína.
                    amin.Escala *= 0.96f;
                }

            }
        
          
            
            
        }

        /// <summary>
        /// Remove um Amino da proteína.
        /// </summary>
        /// <param name="a">O Amino a ser removido.</param>
        public void RemoverAmino(Amino a)
        {
            // O índice do Amino na proteína.
            int indice = Aminos.IndexOf(a);

            // Liga os Aminos ao redor.
            if (a!=PrimeiroAmino)
            {
                if (a != UltimoAmino)
                {
                    Aminos[indice - 1].AminoLigado = Aminos[indice + 1];
                }
                else
                {
                    Aminos[indice - 1].AminoLigado = Fase.AminoEsperando;
                }
            }

             Aminos.Remove(a);

        }
        #endregion
    }
}
