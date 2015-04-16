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
    /// Representa o Vírus, obstáculo que atinge os Aminos.
    /// </summary>
    class Virus : ObjetoMovel
    {
        #region Propriedades e accessors
        private Amino aminoAlvo;
        /// <summary>
        /// O Amino a ser atacado.
        /// </summary>
        public Amino AminoAlvo
        {
            set { aminoAlvo = value; }
            get { return aminoAlvo; }
        }

        private Vector2 alvo;
        /// <summary>
        /// A posição à qual se dirigir.
        /// </summary>
        public Vector2 Alvo
        {
            set { alvo = value; }
            get { return alvo; }
        }

        /// <summary>
        /// Delimita a região de colisão do Virus.
        /// </summary>
        public Rectangle RetanguloDeColisao
        {
            get
            {
                Vector2 size = new Vector2(Retangulo.Width * 0.1f, Retangulo.Height * 0.1f);
                Rectangle a = new Rectangle((int)(Centro.X - (size.X / 2)), (int)(Centro.Y - (size.Y / 2)), (int)size.X, (int)size.Y);
                return a;
            }
        }

        private bool isEncaixado=false;
        /// <summary>
        /// Determina se há um Amino sendo atacado por este Virus.
        /// </summary>
        public bool IsEncaixado
        {
            set { isEncaixado = value; }
            get { return isEncaixado; }
        }

        private float delay = 3000f;
        /// <summary>
        /// O tempo que o Virus demora para destruir um Amino.
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

        #endregion

        #region Construtores
        /// <param name='aFase'>A fase atual.</param>
        /// <param name='aPosition'>A posição inicial.</param>
        public Virus(Fase aFase, Vector2 aPosition)
        {
            this.Fase = aFase;
            this.Position = aPosition;
            if (Fase.Proteina.Aminos.Count > 0)
            {
                AminoAlvo = Fase.Proteina.UltimoAmino;
                Alvo = AminoAlvo.Centro;
            }
            else
            {
                Alvo = new Vector2(-500,-500);
            }
            LoadContent(ref animPadrao, "Obstaculos/Virus", 0.18f);
            Tamanho = new Vector2(AnimPadrao.FrameWidth, AnimPadrao.FrameHeight);
            Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
            IsAtivo = true;
            LoadSom(Fase.SomVirus);
            Valor = 10000;
        }

        /// <param name='aFase'>A fase atual.</param>
        /// <param name='aPosition'>A posição inicial.</param>
        /// <param name='aEscala'>A escala de tamanho.</param>
        public Virus(Fase aFase, Vector2 aPosition, float aEscala)
        {
            Fase = aFase;
            Escala = aEscala;
            Position = aPosition;
            if (Fase.Proteina.Aminos.Count > 0)
            {
                AminoAlvo = Fase.Proteina.UltimoAmino;
                Alvo = AminoAlvo.Centro;
            }
            else
            {
                Alvo = new Vector2(-500,-500);
            }
            DefinirVelocidade(Alvo, 1);
            LoadContent(ref animPadrao, "Obstaculos/Virus", 0.18f);
            Tamanho = new Vector2(AnimPadrao.FrameWidth, AnimPadrao.FrameHeight);
            Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
            IsAtivo = true;
            LoadSom(Fase.SomVirus);
            Valor = 10000;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Atualiza o Virus.
        /// </summary>
        public override void Update(GameTime GT)
        {
            if (IsFuncionando)
            {
                // Afasta o Amino sendo arrastado em caso de colisão.
                DesviarObjeto(Fase.AminoArrastando);

                // Busca um novo Amino para perseguir.
                if (AminoAlvo == null)
                {
                    if (!(ExisteImunizador()))
                    {
                        BuscarAmino();
                    }
                }

                
                if (!(IsEncaixado))
                {
                    // Corrige a direção rumo ao alvo.
                    DefinirVelocidade(Alvo, 2);
                    
                    // Atualiza a posição.
                    this.position.X += Velocidade.X ;
                    this.position.Y += Velocidade.Y ;

                    // Atualiza a posição do alvo e determina se ele foi alcançado.
                    if (AminoAlvo != null)
                    {
                        Alvo = new Vector2(AminoAlvo.Centro.X, AminoAlvo.Position.Y);

                        if (ColisaoPixel(AminoAlvo))
                        {
                            IsEncaixado = true;
         
                        }
                    }
 
                }
                else
                {
                    // Iguala o centro do Virus ao topo do Amino infectado.
                    this.Centro = new Vector2(AminoAlvo.Centro.X, AminoAlvo.Position.Y);

                    // Desencaixa se o Amino infectado tenha sido eliminado.
                    if (AminoAlvo == null)
                    {
                        IsEncaixado = false;                   
                    }
                    PrepararDestruction(GT);
                }

                // Destrói a si próprio caso saia da tela.
                if (this.ForaDaTela())
                {
                    IsAlive = false;
                }
            }

           
        }

        /// <summary>
        /// Busca um novo Amino para perseguir.
        /// </summary>
        public void BuscarAmino()
        {
            // Só busca se houver ao menos 1 Amino na proteína.
            if (Fase.Proteina.Aminos.Count > 0)
            {
                // Escolhe o último Amino que não seja o Amino em espera.
                if (Fase.Proteina.UltimoAmino != Fase.AminoEsperando)
                {
                    AminoAlvo = Fase.Proteina.UltimoAmino;
                  
                }
                else if (Fase.Proteina.ContAmino > 1)
                {
                    AminoAlvo = Fase.Proteina.Aminos[Fase.Proteina.ContAmino - 2];
                   
                }
          

            }
          

        }

        /// <summary>
        /// Ataca o Amino infectado até que seja destruído.
        /// </summary>
        public void PrepararDestruction(GameTime GT)
        {
            if (IsEncaixado)
            {
                // Determina se já passou tempo suficiente para destruir o Amino.
                if (Timer > Delay)
                {
                    DestruirAlvo();
                    // Define um Alvo fora da tela após a destruição
                    Alvo = new Vector2(-500, -500);
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
        /// Elimina o Amino da proteína.
        /// </summary>
        public void DestruirAlvo()
        {
            Fase.Proteina.RemoverAmino(AminoAlvo);
            IsEncaixado = false;
            AminoAlvo = null;

        }
        #endregion

        private bool ExisteImunizador()
        {
            foreach(Interferon o in Fase.Interferons)
            {
                if (o.Tipo == TiposInterferon.Imunizador)
                {
                    return true;
                }
            }
            return false;
        }


    }
}
