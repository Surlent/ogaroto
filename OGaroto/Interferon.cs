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
    public enum TiposInterferon
    {
        Pilula,
        Lisossomo,
        FeixeATP,
        Ressonante,
        Imunizador
    }

    class Interferon : ObjetoMovel
    {
        #region Propriedades e accessors
        private TiposInterferon tipo;
        /// <summary>
        /// O tipo do Interferon.
        /// </summary>
        public TiposInterferon Tipo
        {
            get { return tipo; }
        }

        private ObjetoMovel lisoAlvo;
        /// <summary>
        /// O alvo do leucócito.
        /// </summary>
        public ObjetoMovel LisoAlvo
        {
            set { lisoAlvo = value; }
            get { return lisoAlvo; }
        }
        /// <summary>
        /// Duração do interferon (em segundos)
        /// </summary>
        private float duracao = 30;
        private float duracaoRestante = 30;

        protected Rectangle retBarra;
        /// <summary>
        /// O retângulo do Interferon dentro da barra.
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
        /// <param name="oTipo">O tipo.</param>
        /// <param name="aPosition">A posição inicial.</param>
        /// <param name="oDelay">Delay para poder ativar o interferon. (segundos) </param>
        public Interferon(Fase aFase,TiposInterferon oTipo,Vector2 aPosition)
        { 
            tipo = oTipo;
            Fase = aFase;
            Position = aPosition;
            LoadContent(ref animPadrao, "Interferons/" + tipo.ToString(),0.12f);
            Tamanho = new Vector2(animPadrao.FrameWidth, animPadrao.FrameHeight);
            Escala = 1f;
            if (oTipo == TiposInterferon.Pilula)
            Velocidade = new Vector2(2, 0);
            else
            Velocidade = new Vector2(1, 0);
        }


        /// <param name="aFase">A fase atual.</param>
        /// <param name="oTipo">O tipo.</param>
        /// <param name="aPosition">A posição inicial.</param>
        /// <param name="oDelay">Delay para poder ativar o interferon. (segundos)</param>
        /// <param name="aEscala">A escala de tamanho.</param>
        public Interferon(Fase aFase, TiposInterferon oTipo,Vector2 aPosition,  float aEscala)
        {
            tipo = oTipo;
            Fase = aFase;
            LoadContent(ref animPadrao, "Interferons/" + tipo.ToString(), 0.12f);
            Escala = aEscala;
            Tamanho = new Vector2(animPadrao.FrameWidth, animPadrao.FrameHeight) ;
            this.Position = aPosition;
            Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
            RetBarra = Retangulo;
            if (oTipo == TiposInterferon.Pilula)
                Velocidade = new Vector2(2, 0);
            else
                Velocidade = new Vector2(1, 0);
        }

        /// <param name="aFase">A fase atual.</param>
        /// <param name="oTipo">O tipo.</param>
        /// <param name="aPosition">A posição inicial.</param>
        /// <param name="oDelay">Delay para poder ativar o interferon. (segundos) </param>
        public Interferon(Fase aFase, TiposInterferon oTipo, Vector2 aPosition, bool emBarra)
        {
            tipo = oTipo;
            Fase = aFase;
            Position = aPosition;
            LoadContent(ref animPadrao, "Interferons/" + tipo.ToString(), 0.12f);
            Tamanho = new Vector2(animPadrao.FrameWidth, animPadrao.FrameHeight);
            if (emBarra)
            {
                Escala = (70 / Tamanho.X)*0.95f;
            }
            if (oTipo == TiposInterferon.Pilula)
                Velocidade = new Vector2(2, 0);
            else
                Velocidade = new Vector2(1, 0);
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Atualiza o Interferon baseado em seu estado.
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            if (IsFuncionando)
            {
                
                            //Processa o efeito de acordo com o tipo de interferon
                            switch(Tipo)
                            {
                                #region Pilula
                                case TiposInterferon.Pilula:
                                    {
                                        foreach (ObjetoMovel o in Fase.Obstaculos)
                                        {
                                            if ((o is Virus) || (o is Parasita) || (o is Ameba))
                                            {
                                                if (o.IsAlive)
                                                {
                                                    if (o is Parasita)
                                                    {
                                                        Parasita p = (Parasita)o;
                                                        p.CodonInfectado.IsInfectado = false;
                                                    }
                                                    o.IsAlive = false;
                                                    Fase.Scorer.Pontuar(o.Valor);
                                                }
                                            }
                                        }

 
                                        Position +=Velocidade;
                                        if (this.ForaDaTela())
                                        { this.IsAlive = false; }
                                    }
                                    break;
                                #endregion
                                #region Lisossomo
                                case TiposInterferon.Lisossomo:
                                    {
                                        if (LisoAlvo == null)
                                        {
                                            foreach (ObjetoMovel o in Fase.Obstaculos)
                                            {

                                                if (o is Virus)
                                                {
                                                    if (o.IsAlive)
                                                    {
                                                        LisoAlvo = (Virus)o;
                                                    }
                                                }
                                                if (o is Parasita)
                                                {
                                                    if (o.IsAlive)
                                                    {
                                                        LisoAlvo = (Parasita)o;
                                                    }
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DefinirVelocidade(LisoAlvo.Centro, 3);
                                        if(ColisaoPixel(LisoAlvo))
                                        {
                                            if (LisoAlvo is Parasita)
                                            {
                                                Parasita p = (Parasita)LisoAlvo;
                                                p.CodonInfectado.IsInfectado = false;
                                            }
                                        LisoAlvo.IsAlive=false;
                                            this.IsAlive=false;
                                            Fase.Scorer.Pontuar(LisoAlvo.Valor);
                                            break;
                                        }
                                        }

                                        Position += Velocidade;
                                        if (this.ForaDaTela())
                                        { this.IsAlive = false; }
                                    }
                                    break;
                                #endregion
                                #region Feixe de ATP
                                case TiposInterferon.FeixeATP:
                                    {
                                        foreach(ObjetoMovel o in Fase.Obstaculos)
                                        {
                                            if ((!(o is Enzima)) && (!(o is Radion)))
                                            {
                                                if (ColisaoPixel(o))
                                                {
                                                    o.IsAlive = false;
                                                    Fase.Scorer.Pontuar(o.Valor);
                                                }
                                            }
                                        }
                                        Position += Velocidade;
                                        if (this.ForaDaTela())
                                        {
                                            this.IsAlive = false;
                                        }
                                    }
                                    break;
                                #endregion
                                #region Ressonante
                                case TiposInterferon.Ressonante:
                                    {
                                        foreach (ObjetoMovel e in Fase.Obstaculos)
                                        {
                                            if (e is Enzima)
                                            {
                                                Enzima a = (Enzima)e;
                                                if (a.IsAlive)
                                                {
                                                    if (a.IsAtivo)
                                                    {
                                                        a.Normalizar();
                                                        a.Neutralizada = true;
                                                    }
                                                }
                                            }

                                            if(e is Radion)
                                            {
                                            e.IsAlive=false;
                                            Fase.Scorer.Pontuar(e.Valor);
                                            }
                                        }
                                        Position += Velocidade;
                                        if (this.ForaDaTela())
                                        {
                                            foreach (ObjetoMovel e in Fase.Obstaculos)
                                            {
                                                if (e is Enzima)
                                                {
                                                    Enzima a = (Enzima)e;
                                                    if (e.IsAlive)
                                                    {
                                                        if (e.IsAtivo)
                                                        {
                                                            a.Neutralizada = false;
                                                        }
                                                    }
                                                }
                                            }
                                            this.IsAlive = false; 
                                        }
                                    }
                                    break;
                                #endregion
                                #region Imunizador
                                case TiposInterferon.Imunizador:
                                    {
                                     
                                        Position += Velocidade;
                                        if (this.ForaDaTela())
                                        { this.IsAlive = false; }
                                    }
                                    break;
                                #endregion
                            }
                        
                }

            
        }


        /// <summary>
        /// Torna o Interferon funcional.
        /// </summary>
        public void Ativar()
        {
            IsAtivo = true;
            IsFuncionando = true;
            IsVisible = true;
        }

        /// <summary>
        /// Desativa as funções do Interferon.
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
