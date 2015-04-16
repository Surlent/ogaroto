using System;
using System.Collections.Generic;
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
    /// Define o elemento essencial de jogo, que deve ser encaixado nos Codons.
    /// </summary>
    class Amino:ObjetoMovel
    {
        #region Propriedades e accessors
        private string tipo;
        /// <summary>
        /// O tipo, de 1 a 7, do Amino.
        /// </summary>
        public string Tipo
        {
            get { return tipo; }
        }

        /// <summary>
        /// O ponto de encaixe em Codons.
        /// </summary>
        public Rectangle RetanguloDeEncaixe
        {
            get
            {
                return new Rectangle((int)this.Centro.X,(int)this.Retangulo.Bottom,2,2);
            }
        }

        /// <summary>
        /// A lista de estados do Amino.
        /// </summary>
        public enum EstadoAmino { EmBarra, Arrastando, Encaixado, Aprisionado,Proteina,Flutuando };
      
        /// <summary>
        /// O estado atual do Amino.
        /// </summary>
        public EstadoAmino Estado = EstadoAmino.EmBarra;

        /// <summary>
        /// O som único do Amino.
        /// </summary>
        public SoundEffect SomUnico
        {
            get {
                return Fase.SonsEncaixe[Convert.ToInt32(Tipo) - 1];
            }
        }

        private Codon codonEncaixado;
        /// <summary>
        /// O Codon no qual este Amino está encaixado.
        /// </summary>
        public Codon CodonEncaixado
        {
            set { codonEncaixado = value; }
            get { return codonEncaixado; }
        }

        private Amino aminoLigado;
        /// <summary>
        /// O Amino no qual este está ligado.
        /// </summary>
        public Amino AminoLigado
        {
            set { aminoLigado = value; }
            get { return aminoLigado; }
        }

        private Ameba amebaLigada;
        /// <summary>
        /// A Ameba que está prendendo este Amino.
        /// </summary>
        public Ameba AmebaLigada
        {
            set { amebaLigada = value; }
            get { return amebaLigada; }
        }

        protected Rectangle retBarra;
        /// <summary>
        /// O retângulo do Amino dentro da barra.
        /// </summary>
        public Rectangle RetBarra
        {
            set
            {
                retBarra = value;
            }
            get { return retBarra; }
        }
#endregion

        #region Construtores
        /// <param name="aFase">A fase atual.</param>
        /// <param name="aTipo">O tipo, de 1 a 7.</param>
        /// <param name="aPosition">A posição inicial.</param>
        public Amino(Fase aFase,string aTipo,Vector2 aPosition)
        { 
            tipo = aTipo;
            Fase = aFase;
            LoadContent(ref animPadrao, "Aminos/amino" + tipo,0.12f);
            Tamanho = new Vector2(animPadrao.FrameWidth, animPadrao.FrameHeight);
            Escala = 1.0f;
            Clicavel = true;
        }


        /// <param name="aFase">A fase atual.</param>
        /// <param name="aTipo">O tipo, de 1 a 7.</param>
        /// <param name="aPosition">A posição inicial.</param>
        /// <param name="aEscala">A escala de tamanho.</param>
        public Amino(Fase aFase, string aTipo,Vector2 aPosition,float aEscala)
        {
            tipo = aTipo;
            Fase = aFase;
            LoadContent(ref animPadrao, "Aminos/amino" + tipo,0.12f);
            Escala = aEscala;
            Tamanho = new Vector2(animPadrao.FrameWidth, animPadrao.FrameHeight) ;
            this.Position = aPosition;
            Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
            RetBarra = Retangulo;
            Clicavel = true;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Atualiza o Amino baseado em seu estado.
        /// </summary>
        public void Update()
        {
            if (IsFuncionando)
            {
                switch (Estado)
                {
                    #region Update dos Aminos da Barra
                    case EstadoAmino.EmBarra:
                        {
                            // Arrasta o Amino caso ele seja clicável e receba um novo clique
                            if ((Fase.MouseManager.NovoClique(this)) && (Fase.AminoArrastando == null))
                            {
                                Arrastar();
                            }

                        }
                        break;
                    #endregion

                    #region Update do Amino arrastando
                    case EstadoAmino.Arrastando:
                        {
                            // Atualiza a posição do centro com base na posição do mouse
                            this.Centro = new Vector2(Fase.MouseManager.Position.X, Fase.MouseManager.Position.Y - 40);

                            // Verifica colisão caso se solte o botão do Mouse e retorna ao estado anterior caso não haja
                            if (Fase.MouseManager.EsquerdoSoltou())
                            {
                                if (Fase.Proteina.Aminos.Contains(this))
                                {
                                    Estado = EstadoAmino.Flutuando;
                                    break;
                                }

                                VerificarColisao(Fase.Codons);
                                if (CodonEncaixado != null)
                                {                                 
                                    Estado = EstadoAmino.Encaixado;
                                    break;
                                }
                                else if (Fase.BarraAminos.Aminos.Contains(this))
                                {
                                    Embarrear();
                                    break;
                                }
                                 


                            }

                           



                        }
                        break;
                    #endregion

                    #region Update do Amino encaixado
                    case EstadoAmino.Encaixado:
                        {
                            // Atualiza a posição do centro com base no Codon encaixado.
                            Centro = new Vector2(CodonEncaixado.Centro.X, CodonEncaixado.Centro.Y - (Tamanho.Y / 2)+4 );

                            // Caso o Codon encaixado saia da tela, desativa os Aminos na proteína.
                            if ((CodonEncaixado.IsAlive == false) && (CodonEncaixado != null))
                            {
                             
                                foreach (Amino amin in Fase.Proteina.Aminos)
                                {
                                    amin.Desativar();
                                }
                                Fase.AminoEsperando = null;
                                Fase.Scorer.SequenciaAminos = 0;
                            }
                        }
                        break;
                    #endregion

                    #region Update dos Aminos aprisionados
                    case EstadoAmino.Aprisionado:
                        {
                            // Iguala o centro ao da Ameba ligada.
                            Centro = AmebaLigada.Centro;

                            // Verifica por clique para se libertar da Ameba.
                            if (Fase.MouseManager.NovoClique(this))
                            {
                                AmebaLigada.Soltar();
                                Arrastar();
                            }
                        }
                        break;
                    #endregion

                    #region Update dos Aminos na Proteína
                    case EstadoAmino.Proteina:
                        {
                            // Atualiza a posição do centro com base na posição do Amino ligado.
                            Centro = new Vector2(AminoLigado.Centro.X - (AminoLigado.Tamanho.X / 3), AminoLigado.Centro.Y);

                        }
                        break;
                    #endregion

                    #region Update dos Aminos flutuando
                    case EstadoAmino.Flutuando:
                        {
                            // Iguala o centro ao centro da fase.
                            Centro = new Vector2(Fase.Centro.X, Fase.Centro.Y);

                            // Arrasta o Amino caso seja clicado.
                            if (Fase.MouseManager.NovoClique(this))
                            {
                                Arrastar();
                            }
                        }
                        break;
                    #endregion
                }
            }
        }

     
        /// <summary>
        /// Verifica por colisão com Codons de mesmo tipo.
        /// </summary>
        /// <param name="c">Um Codon possivelmente em colisão.</param>
        /// <returns></returns>
        public bool Colisao(Codon c)
        {
            if (Tipo == c.Tipo)
            {
                if (this.RetanguloDeEncaixe.Intersects(c.Retangulo))
                {
                    return true;
                }
                else return false;
            }
            else return false;
        }

        /// <summary>
        /// Verifica colisão com um objeto móvel qualquer.
        /// </summary>
        /// <param name="o">Um objeto móvel possivelmente em colisão.</param>
        /// <returns></returns>
        public bool Colisao(ObjetoMovel o)
        {
            if (o.Retangulo.Intersects(this.Retangulo))
            {
                return true;
            }
            else
                return false;

        }

    
        /// <summary>
        /// Busca um Codon para se encaixar.
        /// </summary>
        /// <param name="osCodons">A lista de Codons da fase.</param>
        public void VerificarColisao(List<Codon> osCodons)
        {
           // Caso não haja um Amino em espera, encaixa sem fazer ligações.
            if ((Fase.AminoEsperando == null)||(Fase.AminoEsperando.CodonEncaixado==null))
            {
                foreach (Codon c in osCodons)
                {
                    if ((c.IsAlive)&&(!(c.IsInfectado)))
                    {
                        if (Colisao(c))
                        {
                            // Se houver colisão, encaixa e toca o som único do Amino.
                            Encaixar(c);
                            if (Fase.GM.IsSoundOn)
                            {
                                Fase.SonsEncaixe[Convert.ToInt32(this.Tipo) - 1].Play();
                            }
                        }
                    }

                }
            }
                // Caso haja um Amino em espera, faz uma ligação.
            else
            {
                if (!(Fase.AminoEsperando.CodonEncaixado==Fase.Codons[Fase.Codons.Count-1]))
                {
                    
                    Codon codonAlvo = osCodons[osCodons.IndexOf(Fase.AminoEsperando.CodonEncaixado) + 1];
                    if (!(codonAlvo.IsInfectado))
                    {
                        if (Colisao(codonAlvo))
                        {
                            // Se houver colisão, encaixa e toca o som único do Amino.
                            Encaixar(codonAlvo);
                            if (Fase.GM.IsSoundOn)
                            {
                                Fase.SonsEncaixe[Convert.ToInt32(this.Tipo) - 1].Play();
                            }
                        }
                    }
                }

            }
       
         

       
        }

        /// <summary>
        /// Liga o Amino ao Codon alvo, tornando-os interdependentes.
        /// </summary>
        /// <param name="c">O Codon no qual este Amino será encaixado.</param>
        public void Encaixar(Codon c)
        {
            CodonEncaixado = c;
            Clicavel = false;
            
            // Remove este Amino da barra.
            if (Fase.BarraAminos.Aminos.Contains(this))
            {
                Fase.BarraAminos.Aminos[Fase.BarraAminos.Aminos.IndexOf(this)] = Fase.BarraAminos.NovoAmino(Fase.BarraAminos.Aminos.IndexOf(this));
            }
            
            // Torna este o Amino em espera, caso não haja nenhum.
            if ((Fase.AminoEsperando == null))
            {
                
                Fase.AminoEsperando = this;
                Fase.AminoArrastando = null;
            }
            else if (!(Fase.Proteina.Aminos.Contains(Fase.AminoEsperando)))
            {
                Ligar();
            }
            else
            {
                Fase.AminoEsperando.Estado = EstadoAmino.Encaixado;
                Fase.Proteina.Aminos.Remove(Fase.AminoEsperando);
                Fase.AminoArrastando = null;
            }
            Fase.BarraAminos.Ativar();
        }

        /// <summary>
        /// Faz com que o Amino tenha sua posição determinada pelo Mouse.
        /// </summary>
        public void Arrastar()
        {
            Fase.AminoArrastando = this;
            Estado = EstadoAmino.Arrastando;
            Escala = 1.35f;
        }

        /// <summary>
        /// Liga este Amino à proteína.
        /// </summary>
        public void Ligar()
        {
            Fase.AminoEsperando.CodonEncaixado.IsAlive = false;
            Fase.AminoEsperando.LoadContent(ref Fase.AminoEsperando.animPadrao, "Aminos/amino" + Fase.AminoEsperando.Tipo + "sem");
            Fase.AminoEsperando.Tamanho = new Vector2(Fase.AminoEsperando.animPadrao.FrameWidth, Fase.AminoEsperando.animPadrao.FrameHeight);
            Fase.AminoEsperando.AminoLigado = this; 
            Fase.Proteina.AdicionarAmino(Fase.AminoEsperando);
            Fase.AminoEsperando.Estado = EstadoAmino.Proteina;
            Fase.AminoEsperando = this;
            Fase.AminoArrastando = null;
            Fase.Scorer.SequenciaAminos += 1;
            Fase.Scorer.Pontuar(100);
           
        }


        /// <summary>
        /// Retorna à barra.
        /// </summary>
        public void Embarrear()
        {
            Retangulo = RetBarra;
            Estado = EstadoAmino.EmBarra;
            Fase.AminoArrastando = null;
            Escala = 0.75f;
        }

        /// <summary>
        /// Torna o Amino funcional.
        /// </summary>
        public void Ativar()
        {
            IsAtivo = true;
            IsFuncionando = true;
            IsVisible = true;
        }

        /// <summary>
        /// Desativa as funções do Amino.
        /// </summary>
        public void Desativar()
        {
            IsAtivo = false;
            IsFuncionando = false;
            IsVisible = false;
        }
        #endregion
    }
}
