using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
namespace OGaroto
{
    /// <summary>
    /// Representa um objeto que exibe mensagens na tela.
    /// </summary>
    class Mensageiro
    {
        #region Classe aninhada Mensagem
        /// <summary>
        /// Armazena uma mensagem a ser exibida pelo mensageiro.
        /// </summary>
        public class Mensagem
        {
            
            private Mensageiro messenger;
            /// <summary>
            /// O mensageiro que exibirá esta mensagem.
            /// </summary>
            public Mensageiro Messenger
            {
                set { messenger = value; }
                get { return messenger; }
            }

            private string texto;
            /// <summary>
            /// O texto que compõe a mensagem.
            /// </summary>
            public string Texto
            {
                set { texto = value; }
                get { return texto; }
            }

            private float duration;
            /// <summary>
            /// O tempo que a imagem ficará na tela.
            /// </summary>
            public float Duration
            {
                set { duration = value; }
                get { return duration; }
            }

            private float escala;
            /// <summary>
            /// A escala de tamanho da mensagem.
            /// </summary>
            public float Escala
            {
                set { escala = value; }
                get { return escala; }
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

            /// <summary>
        /// A posição do texto que será exibido.
        /// </summary>
            public Vector2 Position
            {
                get {
                return  new Vector2(Messenger.Fase.Centro.X - (Messenger.Fonte.MeasureString(this.Texto).X / 2), Messenger.Fase.Centro.Y - (Messenger.Fonte.MeasureString("A").Y*2));
                }
            }
            
            private Color cor;
            /// <summary>
            /// A cor em que será exibida a mensagem.
            /// </summary>
            public Color Cor
            {
                set { cor = value; }
                get { return cor; }
            }

            
            /// <param name="oMessenger">O mensageiro que exibirá esta mensagem.</param>
            /// <param name="oTexto">O texto a ser exibido.</param>
            /// <param name="aDuration">A duração da mensagem.</param>
            /// <param name="aCor">A cor do texto da mensagem.</param>
            /// <param name="aEscala">A escala de tamanho.</param>
            public Mensagem(Mensageiro oMessenger,string oTexto, float aDuration, Color aCor,float aEscala)
            {
                Messenger = oMessenger;
                Texto = oTexto;
                Duration = aDuration;
                Escala = aEscala;
                Cor=aCor;
            }


        }

        private Texture2D quadro;
        /// <summary>
        /// A imagem sobre a qual fica o texto.
        /// </summary>
        public Texture2D Quadro
        {
            set { quadro = value; }
            get { return quadro; }
        }

        private SpriteFont fonte;
        /// <summary>
        /// A fonte na qual é escrita a mensagem.
        /// </summary>
        public SpriteFont Fonte
        {
            set { fonte = value; }
            get { return fonte; }
        }

        private SoundEffect sonzinho;
        /// <summary>
        /// O som característico de mensagem nova.
        /// </summary>
        public SoundEffect Sonzinho
        {
            set { sonzinho = value; }
            get { return sonzinho; }
        }

        private Mensagem aviso;
        /// <summary>
        /// A mensagem a ser exibida.
        /// </summary>
        public Mensagem Aviso
        {
            set { aviso = value; }
            get { return aviso; }
    }

        private Fase fase;
        /// <summary>
        /// A fase atual.
        /// </summary>
        public Fase Fase
        {
            set { fase = value; }
            get { return fase; }
        }
        #endregion

        #region Construtor
        /// <param name="aFase">A fase atual.</param>
        public Mensageiro(Fase aFase)
        {
            Fase = aFase;

        }
        #endregion

        #region Métodos
        /// <summary>
        /// Carrega a fonte e a imagem de fundo do mensageiro.
        /// </summary>
        /// <param name="aFonte">A fonte usada para escrever.</param>
        /// <param name="caminhoQuadro">A imagem sobre a qual é escrita a mensagem.</param>
        public void LoadContent(SpriteFont aFonte,string caminhoQuadro)
{
    Fonte = aFonte;
    Quadro = Fase.Content.Load<Texture2D>(caminhoQuadro);
    Sonzinho = Fase.Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somMensagem");
    }

        /// <summary>
        /// Desenha a mensagem, caso exista.
        /// </summary>
        public void Draw(GameTime GT, SpriteBatch SB)
        {
            // Desenha enquanto o contador não passar da duração da mensagem.
            if (Aviso != null)
            {
                if (Aviso.Timer < Aviso.Duration)
                {
                    SB.Draw(Quadro, Aviso.Position - new Vector2(25, 25), null, Color.White, 0f, Vector2.Zero, new Vector2((Fonte.MeasureString(Aviso.Texto).X + 50) / Quadro.Width, (Fonte.MeasureString(Aviso.Texto).Y + 50) / Quadro.Height), SpriteEffects.None, 0);
                    SB.DrawString(Fonte, Aviso.Texto, Aviso.Position, Aviso.Cor, 0f, Vector2.Zero, Aviso.Escala, SpriteEffects.None, 0);
                    Aviso.Timer += GT.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    // Apaga o aviso caso tenha passado o tempo de exibição.
                    Aviso = null;
                }
            }
        }

        /// <summary>
        /// Adiciona uma mensagem a ser exibida, na cor escolhida.
        /// </summary>
        /// <param name="oTexto">O texto a ser escrito.</param>
        /// <param name="aDuration">A duração da mensagem na tela.</param>
        /// <param name="aEscala">A escala de tamanho da mensagem.</param>
        /// <param name="aCor">A cor da mensagem.</param>
        public void AdicionarMensagem(string oTexto, float aDuration, float aEscala, Color aCor)
        {
            Aviso = new Mensagem(this, oTexto, aDuration, aCor, aEscala);
            if (Fase.GM.IsSoundOn)
            {
                Sonzinho.Play();
            }
        }

        /// <summary>
        /// Adiciona uma mensagem a ser exibida, na cor preta.
        /// </summary>
        /// <param name="oTexto">O texto a ser escrito.</param>
        /// <param name="aDuration">A duração da mensagem na tela.</param>
        /// <param name="aEscala">A escala de tamanho da mensagem.</param>
        public void AdicionarMensagem(string oTexto, float aDuration, float aEscala)
        {
            Aviso = new Mensagem(this, oTexto, aDuration, Color.Black, aEscala);
            if (Fase.GM.IsSoundOn)
            {
                Sonzinho.Play();
            }

        }
        #endregion
    }
}
