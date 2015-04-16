using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OGaroto
{
    /// <summary>
    /// Representa um ser que impede o encaixe de Aminos no Codon infectado.
    /// </summary>
    class Parasita:ObjetoMovel
    {
        #region Propriedades e accessors
        private Codon codonInfectado;
        /// <summary>
        /// O Codon infectado.
        /// </summary>
        public Codon CodonInfectado
        {
            set { codonInfectado = value; }
            get { return codonInfectado; }
        }
        #endregion

        #region Construtor
        /// <param name="aFase">A fase atual.</param>
        /// <param name="oCodonInfectado">O Codon a ser infectado.</param>
        public Parasita(Fase aFase, Codon oCodonInfectado)
        {
            Fase = aFase;            
            CodonInfectado = oCodonInfectado;
            oCodonInfectado.IsInfectado = true;
            LoadContent(ref animPadrao, "Obstaculos/Parasita", 0.12f);
            LoadSom(Fase.SomParasita);
            Tamanho = new Vector2(AnimPadrao.FrameWidth, AnimPadrao.FrameHeight);
            Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
            this.Centro = new Vector2(CodonInfectado.Centro.X - 3, CodonInfectado.Position.Y - (CodonInfectado.Tamanho.Y / 2) + 12);
            this.Velocidade = CodonInfectado.Velocidade;
            Valor = 2000;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Atualiza o parasita.
        /// </summary>
        public override void Update(GameTime GT)
        {
            if (IsFuncionando)
            {
                if (!(IsAtivo))
                {
                    IsAtivo = true;
                    AtivarSonzinho();
                }
               
                DesviarObjeto(Fase.AminoArrastando);
                
                this.Centro+=CodonInfectado.Velocidade;
               
                if (!(CodonInfectado.IsAlive))
                {
                    IsAlive = false;                   
                }
            }
        }
        #endregion
    }

}
