using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
namespace OGaroto
{
    /// <summary>
    /// Define um ser que intercepta os Aminos no caminho até os Codons.
    /// </summary>
    class Ameba:ObjetoMovel
    {

        #region Propriedades e accessors
        private Amino aminoPreso;
        /// <summary>
        /// O amino preso pela Ameba.
        /// </summary>
        public Amino AminoPreso
        {
            set { aminoPreso = value; }
            get { return aminoPreso; }
        }
        
        private float delay = 1500f;
        /// <summary>
        /// O tempo de espera da Ameba até poder capturar um novo Amino.
        /// </summary>
        public float Delay
        {
            set { delay = value; }
            get { return delay/Fase.Acelerador; }
        }

        private float timer =0;
        /// <summary>
        /// Contador de tempo.
        /// </summary>
        public float Timer
        {
            set { timer = value; }
            get { return timer; }
        }

        /// <summary>
        /// Define um retângulo no qual a Ameba pode capturar um Amino.
        /// </summary>
        public Rectangle RetanguloDeCaptura
        {
            get { 
                Vector2 size=new Vector2(Retangulo.Width*0.1f,Retangulo.Height*0.1f);
                Rectangle a = new Rectangle((int)(Centro.X - (size.X / 2)), (int)(Centro.Y - (size.Y / 2)), (int)size.X, (int)size.Y);
            return a;}
        }
        #endregion

        #region Construtores
        /// <param name='aFase'>A fase atual.</param>
        /// <param name='aPosition'>A posição inicial.</param>
        public Ameba(Fase aFase, Vector2 aPosition)
        {
            Fase = aFase;
            Position = aPosition;
            LoadContent(ref animPadrao, "Obstaculos/Ameba", 0.12f);
            LoadSom(Fase.SomAmeba);
            Tamanho = new Vector2(AnimPadrao.FrameWidth, AnimPadrao.FrameHeight);
            Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
            Velocidade =Fase.VelocidadeDaTela;
            Valor = 1000;
        }

        /// <param name='aFase'>A fase atual.</param>
        /// <param name='aPosition'>A posição inicial.</param>
        /// <param name='aEscala'>A escala de tamanho.</param>
        public Ameba(Fase aFase, Vector2 aPosition, float aEscala)
        {
            Fase = aFase;
            Position = aPosition;
            Escala = aEscala;
            LoadContent(ref animPadrao, "Obstaculos/Ameba", 0.12f);
            LoadSom(Fase.SomAmeba);
            Tamanho = new Vector2(AnimPadrao.FrameWidth, AnimPadrao.FrameHeight);
            Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
            Velocidade = Fase.VelocidadeDaTela;
            Valor = 1000;
        }

        /// <param name='aFase'>A fase atual.</param>
        /// <param name='aPosition'>A posição inicial.</param>
        /// <param name='aEscala'>A escala de tamanho.</param>
        /// <param name='aDestino'>O ponto ao qual se dirigir.</param>
        public Ameba(Fase aFase, Vector2 aPosition, Vector2 aDestino, float aEscala)
        {
            Fase = aFase;
            Position = aPosition;
            Escala = aEscala;
            LoadContent(ref animPadrao, "Obstaculos/Ameba", 0.12f);
            LoadSom(Fase.SomAmeba);
            Tamanho = new Vector2(AnimPadrao.FrameWidth, AnimPadrao.FrameHeight);
            Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
            DefinirVelocidade(aDestino, Fase.VelocidadeDeJogo);
            Valor = 1000;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Atualiza a Ameba.
        /// </summary>
        public override void Update(GameTime GT)
        {
            if (IsFuncionando)
            {
                // Atualiza a posição da Ameba.
                this.position.X += Velocidade.X;
                this.position.Y += Velocidade.Y;

                // Destrói a si mesmo e ao Amino preso se sair da tela.
                if (this.ForaDaTela())
                {
                    if (AminoPreso != null)
                    {
                        Fase.BarraAminos.Aminos[Convert.ToInt32(AminoPreso.Tipo) - 1] = Fase.BarraAminos.NovoAmino(Convert.ToInt32(AminoPreso.Tipo) - 1);
                        Fase.AminoArrastando = null;
                    }
                    IsAlive = false;
                }

                PrepararCaptura(GT);
                BuscarAmino();
            }
        }

       /// <summary>
        /// Prende o Amino alvo à Ameba.
       /// </summary>
       /// <param name="amino">O Amino em colisão.</param>
        public void Prender(Amino amino)
        {           
            AminoPreso=amino;           
            amino.AmebaLigada = this;
            amino.Estado = Amino.EstadoAmino.Aprisionado;
            
        }

        /// <summary>
        /// Solta o Amino preso.
        /// </summary>
        public void Soltar()
        {
            AminoPreso.AmebaLigada = null;
            AminoPreso = null;
            IsAtivo = false;
            Timer = 0;
        }

        /// <summary>
        /// Busca Aminos no Retângulo de Captura e prende-os.
        /// </summary>
        public void BuscarAmino()
        {
            if (IsAtivo)
            {
                if (Fase.AminoArrastando != null)
                {
                    if (!(Fase.Proteina.Aminos.Contains(Fase.AminoArrastando)))
                    {
                        if (Fase.AminoArrastando.Colisao(this.RetanguloDeCaptura))
                        {
                            if (AminoPreso == null)
                            {
                                Prender(Fase.AminoArrastando);
                                AtivarSonzinho();
                            }
                        }
                    }
                }          
            }
        }

        /// <summary>
        /// Prepara a busca por novos Aminos.
        /// </summary>
        public void PrepararCaptura(GameTime GT)
        {
            // Determina se já passou tempo suficiente para iniciar uma nova busca por Aminos.
            if (!(IsAtivo))
            {
                if (Timer > Delay)
                {
                    IsAtivo = true;
                }
                else
                {
                    timer += GT.ElapsedGameTime.Milliseconds;
                }
            }
        }

        /// <summary>
        /// Altera o tempo de espera para uma nova captura.
        /// </summary>
        /// <param name="oDelay">O tempo de espera até que se possa capturar um novo Amino</param>
        public void AlterarDelay(float oDelay)
        {
            Delay = oDelay;
        }
        #endregion
    }
}
