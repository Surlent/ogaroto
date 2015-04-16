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
    /// Controla os estados de jogo.
    /// </summary>
    class GamestateManager
    {
        #region Propriedades e accessors
        private bool isFuncionando = true;
        /// <summary>
        /// Determina se um objeto se atualiza.
        /// </summary>
        public bool IsFuncionando
        {
            set { isFuncionando = value; }
            get { return isFuncionando; }
        }

        private bool isSoundOn = true;
        /// <summary>
        /// Determina se há som no jogo.
        /// </summary>
        public bool IsSoundOn
        {
            set { isSoundOn = value; }
            get { return isSoundOn; }
        }
        /**
        private ShiftAnimation animShift;
        /// <summary>
        /// A animação de mudança de estado de jogo.
        /// </summary>
        public ShiftAnimation AnimShift
        {
            set { animShift = value; }
            get { return animShift; }
        }
        */

        private GarotoGame game;
        /// <summary>
        /// O jogo atual.
        /// </summary>
        public GarotoGame Game
        {
            set { game = value; }
            get { return game; }
        }

        private IServiceProvider serviceProvider;
        /// <summary>
        /// O provedor de serviços usado pelo ContentManager.
        /// </summary>
        public IServiceProvider ServiceProvider
        {
            set { serviceProvider = value; }
            get { return serviceProvider; }
        }

        private GraphicsDeviceManager graphics;
        /// <summary>
        /// O gerenciador de dispositivo gráfico do jogo.
        /// </summary>
        public GraphicsDeviceManager Graphics
        {
            set { graphics = value; }
            get { return graphics; }
        }

        private ContentManager content;
        /// <summary>
        /// O gerenciador de conteúdo do jogo, responsável pelo carregamento de recursos.
        /// </summary>
        public ContentManager Content
        {
            set { content = value; }
            get { return content; }
        }

        private MouseManager mouseManager;
        /// <summary>
        /// O gerenciador de mouse do jogo.
        /// </summary>
        public MouseManager MouseManager
        {
            set { mouseManager = value; }
            get { return mouseManager; }
        }

        /// <summary>
        /// A lista de estados de jogo possíveis.
        /// </summary>
       public enum Gamestate { Inicio=1, Garoto=2, Options=3, Jogo=4,Score=5, Pause=6, Loja=7 };
        
        private Gamestate estado;
        /// <summary>
        /// O estado atual de jogo.
        /// </summary>
        public Gamestate Estado
        {
            set { estado = value; }
            get { return estado; }
        }

        private SpriteFont fonte;
        /// <summary>
        /// A fonte usada para escrever na tela.
        /// </summary>
        public SpriteFont Fonte
        {
            set { fonte = value; }
            get { return fonte; }
        }

        private TelaInicial telaInicial;
        /// <summary>
        /// A instância do tela inicial.
        /// </summary>
        public TelaInicial TelaInicial
        {
            set { telaInicial = value; }
            get { return telaInicial; }
        }

        private TelaGaroto telaGaroto;
        /// <summary>
        /// A instância do tela do Garoto.
        /// </summary>
        public TelaGaroto TelaGaroto
        {
            set { telaGaroto = value; }
            get { return telaGaroto; }
        }

        private TelaPause telaPause;
        /// <summary>
        /// A instância do tela de pause.
        /// </summary>
        public TelaPause TelaPause
        {
            set { telaPause = value; }
            get { return telaPause; }
        }

        private TelaScore telaScore;
        /// <summary>
        /// A instância do tela de pontuação.
        /// </summary>
        public TelaScore TelaScore
        {
            set { telaScore = value; }
            get { return telaScore; }
        }

        private TelaOptions telaOptions;
        /// <summary>
        /// A instância do tela de configurações.
        /// </summary>
        public TelaOptions TelaOptions
        {
            set { telaOptions = value; }
            get { return telaOptions; }
        }

        private TelaLoja telaLoja;
        /// <summary>
        /// A instância do tela de compra de partes.
        /// </summary>
        public TelaLoja TelaLoja
        {
            set { telaLoja = value; }
            get { return telaLoja; }
        }

        private Fase fase;
        /// <summary>
        /// A instância da fase atual.
        /// </summary>
        public Fase Fase
        {
            set { fase = value; }
            get { return fase; }
        }
        #endregion

        #region Construtor
        /// <param name='oGame'>O jogo.</param>
        /// <param name='oGraphics'>O gerenciador de dispositivo gráfico.</param>
        /// <param name='estadoInicial'>O estado em que o jogo deve iniciar.</param>
        /// <param name='service'>O provedor de serviços do jogo.</param>
        public GamestateManager(GarotoGame oGame,GraphicsDeviceManager oGraphics, Gamestate estadoInicial, IServiceProvider service)
        {
            Game = oGame;
            content = new ContentManager(service,"Content");
            Graphics = oGraphics;
            Estado = estadoInicial;
            TelaInicial = new TelaInicial(this);
            ServiceProvider = service;
            content = new ContentManager(ServiceProvider, "Content");
            MouseManager = new MouseManager(this);
       
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Carrega recursos.
        /// </summary>
        public void LoadContent()
        {
           
            Fonte = content.Load<SpriteFont>("Fontes/Fonte1");
           
        }

        /// <summary>
        /// Atualiza o jogo.
        /// </summary>
        public void Update(GameTime GT)
        {
            if (IsFuncionando)
            {
                // Atualiza o mouse.
                MouseManager.Update();
                
                // Define qual tela atualizar com base no estado de jogo.
                switch (Estado)
                {
                    case Gamestate.Inicio:
                        {
                            TelaInicial.Update();
                        } break;
                    case Gamestate.Garoto:
                        {
                            TelaGaroto.Update(GT);
                        } break;
                    case Gamestate.Jogo:
                        {
                            Fase.Update(GT);
                        } break;
                    case Gamestate.Score:
                        {

                            TelaScore.Update();
                            TelaScore.UpdateProteina(GT);
                        }
                        break;
                    case Gamestate.Loja: {
                        TelaLoja.Update();
                    } break;
                    case Gamestate.Pause:
                        {
                            TelaPause.Update();
                        } break;
                    case Gamestate.Options:
                        {
                            TelaOptions.Update();
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Desenha o jogo.
        /// </summary>
        public void Draw(GameTime GT, SpriteBatch SB)
        {
            
            // Define qual tela desenhar com base no estado de jogo.
            switch (Estado)
            {
                case Gamestate.Inicio:
                    {
                        TelaInicial.Draw(SB);
                        TelaInicial.DrawTexto(SB);
                    } break;
                case Gamestate.Garoto:
                    {
                        TelaGaroto.Draw(SB);
                        TelaGaroto.DrawGaroto(SB);
                        TelaGaroto.DrawTexto(SB);
                } break;
                case Gamestate.Jogo: {
                    Fase.Draw(GT, SB);
                    Fase.DrawTexto(SB);
                } break;
                case Gamestate.Score:
                    {

                        TelaScore.Draw(SB);
                        TelaScore.DrawProteina(GT, SB);
                        TelaScore.DrawTexto(SB, new Color(0, 150, 0, 255));
                    }
                    break;
                case Gamestate.Loja: 
                    {
                        TelaLoja.Draw(SB);
                        TelaLoja.DrawMercadorias(SB);
                        TelaLoja.DrawTexto(SB);
                } break;
                case Gamestate.Pause: {
                    TelaPause.Draw(SB);
                    TelaPause.DrawTexto(SB);
                    
                } break;

                case Gamestate.Options:
                    {
                        TelaOptions.Draw(SB);
                        TelaOptions.DrawTexto(SB);
                    }
                    break;
            }
           
            
        }

        /// <summary>
        /// Interrompe o jogo e vai para a tela de pause.
        /// </summary>
        public void Pausar()
        {
            TelaPause = new TelaPause(this);
            MediaPlayer.Stop();
            Estado = Gamestate.Pause;
        }

        /// <summary>
        /// Sai da tela de pause, retornando ao jogo.
        /// </summary>
        public void Despausar()
        {
            MediaPlayer.Stop();
            Estado = Gamestate.Jogo;
        }

        /// <summary>
        /// Retorna à tela do Garoto.
        /// </summary>
        public void VoltarGaroto()
        {
            Fase.Finalizar();
            Estado = Gamestate.Garoto;
        }


        #endregion

        #region Classe ShiftAnimation
        /// <summary>
        /// Classe para animação de mudanças de estado
        /// </summary>
        public class ShiftAnimation:ObjetoFixo
        {
            private GamestateManager gM;
            /// <summary>
            /// O gerenciador de estados do jogo.
            /// </summary>
            public GamestateManager GM
            {
                set { gM = value; }
                get { return gM; }
            }

            private bool isRodando;
            /// <summary>
            /// Determina se a animação está rodando.
            /// </summary>
            public bool IsRodando
            {
                set { isRodando = value; }
                get { return isRodando; }
            }

            private bool parou;
            /// <summary>
            /// Determina se a animação já parou para esperar o carregamento de conteúdo.
            /// </summary>
            public bool Parou
            {
                set { parou = value; }
                get { return parou; }
            }


            public ShiftAnimation(GamestateManager oGM)
            {
                GM=oGM;
                Position = new Vector2(oGM.Game.GraphicsDevice.Viewport.Width, 0);              
            }


            public void LoadContent()
            {
                Imagem = GM.Content.Load<Texture2D>("Fundos/telaChangeState");
                Tamanho = new Vector2(Imagem.Width, Imagem.Height);
            }

            public void Update()
            {
                if (IsRodando)
                {
                    position.X -= 5;
                
                if ((Position.X <=0)&&(!(Parou)))
                {
                    IsRodando = false;
                }

                if (Retangulo.Right < 0)
                {
                    this.IsRodando = false;
                }
                }
            }

            public void Draw(SpriteBatch SB)
            {
                if (IsRodando)
                {
                    SB.Draw(Imagem, Position, Retangulo, Color.White, 0, Vector2.Zero, 1f, SpriteEffects.None, 0);
                }
           }

            public void Iniciar()
            {
                Position = new Vector2(GM.Game.GraphicsDevice.Viewport.Width, 0);
                IsRodando= true;
                Parou = false;
            }
        }
        #endregion
    }
}
