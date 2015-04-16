using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;

namespace OGaroto
{
    /// <summary>
    /// Representa a tela de jogo, da qual todos os objetos são dependentes.
    /// </summary>
    class Fase : IDisposable
    {
        #region Propriedades e accessors
        private bool isFuncionando = true;
        /// <summary>
        /// Determina se a fase se atualiza.
        /// </summary>
        public bool IsFuncionando
        {
            set { isFuncionando = value; }
            get { return isFuncionando; }
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

        private GamestateManager gM;
        /// <summary>
        /// O gerenciador de estados do jogo.
        /// </summary>
        public GamestateManager GM
        {
            set { gM = value; }
            get { return gM; }
        }

         List<ObjetoMovel> obstaculos;
        /// <summary>
        /// A lista de obstáculos da fase.
        /// </summary>
         public List<ObjetoMovel> Obstaculos
         {
             get { return obstaculos; }
         }
        
        List<Codon> codons;
        /// <summary>
        /// A lista de Codons da fase.
        /// </summary>
        public List<Codon> Codons
        {
            get { return codons; }
        }

        List<Interferon> interferons;
        /// <summary>
        /// A lista de Interferons da fase.
        /// </summary>
        public List<Interferon> Interferons
        {
            get { return interferons; }
        }

        private BarraAmino barraAmino;
        /// <summary>
        /// A barra de Aminos da fase.
        /// </summary>
        public BarraAmino BarraAminos
        {
            get { return barraAmino; }
        }

        private BarraInterf barraInterf;
        /// <summary>
        /// A barra de interferons da fase.
        /// </summary>
        public BarraInterf BarraInterf
        {
            get { return barraInterf; }
            set { barraInterf = value; }
        }

        private Proteina proteina;
        /// <summary>
        /// A proteína em formação.
        /// </summary>
        public Proteina Proteina
        {
            set { proteina = value; }
            get { return proteina; }
        }

        private MouseManager mouseManager;
            /// <summary>
            /// O gerenciador de Mouse do jogo.
            /// </summary>
        public MouseManager MouseManager
        {
            set { mouseManager = value; }
            get { return mouseManager; }
        }

        Amino aminoArrastando,aminoEsperando;
        
        /// <summary>
        /// O Amino atualmente sendo arrastado.
        /// </summary>
        public Amino AminoArrastando
        {
            set { aminoArrastando = value; }
            get { return aminoArrastando; }
    }

        /// <summary>
        /// O Amino esperando para fazer uma ligação.
        /// </summary>
        public Amino AminoEsperando
        {
            set { aminoEsperando = value; }
            get { return aminoEsperando; }
        }

        private string nivel;
        /// <summary>
        /// O nome da fase sendo jogada.
        /// </summary>
        public string Nivel
        {
            get { return nivel; }
        }

        private float velocidadeDeJogo;
        /// <summary>
        /// A velocidade na qual o jogo transcorre.
        /// </summary>
        public float VelocidadeDeJogo
        {
            set { velocidadeDeJogo = value; }
            get { return velocidadeDeJogo*Acelerador; }
        }

        private float acelerador=1f;
        /// <summary>
        /// O multiplicador de velocidade da fase.
        /// </summary>
        public float Acelerador
        {
            set { acelerador = value; }
            get { return acelerador; }
        }

        private bool acelerada =false;
        /// <summary>
        /// Determina se a fase já foi acelerada ou não.
        /// </summary>
        public bool Acelerada
        {
            set { acelerada = value; }
            get { return acelerada; }
        }

        private bool travada = false;
        /// <summary>
        /// Determina se a fase está travada.
        /// </summary>
        public bool Travada
        {
            set { travada= value; }
            get { return travada; }
        }

        private Scorer scorer;
        /// <summary>
        /// O pontuador da fase, que determina os dados da partida.
        /// </summary>
        public Scorer Scorer
        {
            set { scorer = value; }
            get { return scorer; }
        }

        /// <summary>
        /// Obtém o centro da fase.
        /// </summary>
        public Vector2 Centro
        {
            get { return new Vector2(Tamanho.X / 2, Tamanho.Y / 2); }
        }

        private Vector2 velocidadeDaTela;
        /// <summary>
        /// A velocidade na qual os objetos da tela se movimentam.
        /// </summary>
        public Vector2 VelocidadeDaTela
        {
            set { velocidadeDaTela = value; }
            get { return velocidadeDaTela; }
        }

        private Vector2 tamanho;
        /// <summary>
        /// O tamanho da tela.
        /// </summary>
        public Vector2 Tamanho
        {
            set { tamanho = value; }
            get { return tamanho; }
        }

        private TimeLine tL;
        /// <summary>
        /// A instância da linha de tempo.
        /// </summary>
        public TimeLine TL
        {
            set { tL = value; }
            get { return tL; }
        }

        /// <summary>
        /// O gerenciador de recursos da fase.
        /// </summary>
        public ContentManager Content
        {
            get { return content; }
        }
        ContentManager content;
    
        private Texture2D fundo;
        /// <summary>
        /// A imagem de fundo da fase.
        /// </summary>
        public Texture2D Fundo
        {
            set { fundo = value; }
            get { return fundo; }
        }
     
        
        private List<SoundEffect> sonsEncaixe;
        /// <summary>
        /// A lista de sons de encaixe dos Aminos.
        /// </summary>
        public List<SoundEffect> SonsEncaixe
        {
            set { sonsEncaixe = value; }
            get { return sonsEncaixe; }
        }

        private SoundEffect somAmeba;
        /// <summary>
        /// O som característico das Amebas.
        /// </summary>
        public SoundEffect SomAmeba
        {
            set { somAmeba = value; }
            get { return somAmeba; }
        }

        private SoundEffect somEnzima;
        /// <summary>
        /// O som característico das Enzimas.
        /// </summary>
        public SoundEffect SomEnzima
        {
            set { somEnzima = value; }
            get { return somEnzima; }
        }

        private SoundEffect somRadion;
        /// <summary>
        /// O som característico dos Radions.
        /// </summary>
        public SoundEffect SomRadion
        {
            set { somRadion = value; }
            get { return somRadion; }
        }

        private SoundEffect somParasita;
        /// <summary>
        /// O som característico dos Parasitas.
        /// </summary>
        public SoundEffect SomParasita
        {
            set { somParasita = value; }
            get { return somParasita; }
        }

        private SoundEffect somVirus;
        /// <summary>
        /// O som característico dos Virus.
        /// </summary>
        public SoundEffect SomVirus
        {
            set { somVirus = value; }
            get { return somVirus; }
        }

        private Song musicaDeFundo;
        /// <summary>
        /// A música de fundo da fase.
        /// </summary>
        public Song Musica
        {
            set { musicaDeFundo = value; }
            get { return musicaDeFundo; }
        }

        /// <summary>
        /// O gerador de números aleatórios.
        /// </summary>
        public Random Gerador;

        private Mensageiro messenger;
        /// <summary>
        /// Responsável pelas mensagens na tela.
        /// </summary>
        public Mensageiro Messenger
        {
            set { messenger = value; }
            get { return messenger; }
        }
        #endregion

        #region Construtor
        /// <param name="graphics">O gerenciador gráfico do jogo.</param>
        /// <param name="oGM">O gerenciador de estados do jogo.</param>
        /// <param name="oNivel">A fase a ser jogada, correspondente a uma parte do Garoto.</param>
        public Fase(GamestateManager oGM,string oNivel)
        {
            GM = oGM;
            MouseManager =GM.MouseManager;
            content = new ContentManager(GM.ServiceProvider, "Content");
            nivel = oNivel;
            tamanho.X = oGM.Game.GraphicsDevice.Viewport.Width;
            tamanho.Y = oGM.Game.GraphicsDevice.Viewport.Height;
            velocidadeDeJogo = 1f;
            velocidadeDaTela = new Vector2(-1, 0);
            TL = new TimeLine(this);
            Scorer = new Scorer(this);
            SonsEncaixe = new List<SoundEffect>(10);
            Gerador = new Random();
            Messenger = new Mensageiro(this);

        }
        #endregion

        #region Métodos
        /// <summary>
        /// Carrega os recursos da fase.
        /// </summary>
        public void LoadContent()
       {
           obstaculos = new List<ObjetoMovel>(7);
           codons = new List<Codon>();
           interferons = new List<Interferon>();
           proteina = new Proteina(this);
           barraAmino = new BarraAmino(this,7);
           barraInterf = new BarraInterf(this);
           SomAmeba = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somAmeba");
           SomEnzima = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somEnzima");
           SomParasita = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somParasita");
           SomRadion = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somRadion");
           Fonte = Content.Load<SpriteFont>("Fontes/Fonte2");
           
           Messenger.LoadContent(Fonte,"Interface/Misc/quadroMensagem");
          
           for (int i = 0; i < 7; i++)
           {
               SonsEncaixe.Insert(i,Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somEncaixe" + (i+1).ToString()));
           }
            // Carrega conteúdo específico de cada nível.
           switch (Nivel)
           {
               case "leftFoot":
               case "rightFoot":
                   {
                       Fundo = Content.Load<Texture2D>("Fundos/faseFeet");
                       Musica = Content.Load<Song>("Sons/Musicas/Sineta");
                   }
                   break;
               case "leftLeg":
               case "rightLeg":
                   {
                       Fundo = Content.Load<Texture2D>("Fundos/faseLegs");
                       Musica = Content.Load<Song>("Sons/Musicas/Money");
                       SomAmeba = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somAmeba");
                       SomEnzima = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somEnzima");

                   }
                   break;
               case "trunk":
                   {
                       Fundo = Content.Load<Texture2D>("Fundos/faseTrunk");
                       SomAmeba = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somAmeba");
                       SomEnzima = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somEnzima");
                       Musica = Content.Load<Song>("Sons/Musicas/MobyDick");

                   } break;
               case "leftArm":
               case "rightArm":
                   {
                       Fundo = Content.Load<Texture2D>("Fundos/faseArms");
                       SomParasita = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somParasita");
                       SomAmeba = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somAmeba");
                       SomEnzima = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somEnzima");
                       Musica = Content.Load<Song>("Sons/Musicas/TheOcean");
                   } break;
               case "leftHand":
               case "rightHand":
                   {
                       Fundo = Content.Load<Texture2D>("Fundos/faseHands");
                       SomParasita = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somParasita");
                       SomAmeba = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somAmeba");
                       SomEnzima = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somEnzima");
                       SomVirus = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somVirus");
                       Musica = Content.Load<Song>("Sons/Musicas/Bolhas");
                   } break;
               case "head":
                   {
                       Fundo = Content.Load<Texture2D>("Fundos/faseHead");
                       SomParasita = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somParasita");
                       SomAmeba = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somAmeba");
                       SomEnzima = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somEnzima");
                       SomVirus = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somVirus");
                       Musica = Content.Load<Song>("Sons/Musicas/CustardPie");

                   } break;
               case "leftEar":
               case "rightEar":
                   {
                       Fundo = Content.Load<Texture2D>("Fundos/faseEars");
                       SomParasita = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somParasita");
                       SomAmeba = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somAmeba");
                       SomEnzima = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somEnzima");
                       SomVirus = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somVirus");
                       SomRadion = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somRadion");
                       Musica = Content.Load<Song>("Sons/Musicas/Money");
                   } break;
               case "mouth":
                   {
                       Fundo = Content.Load<Texture2D>("Fundos/faseEars");
                       SomParasita = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somParasita");
                       SomAmeba = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somAmeba");
                       SomEnzima = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somEnzima");
                       SomVirus = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somVirus");
                       SomRadion = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somRadion");
                       Musica = Content.Load<Song>("Sons/Musicas/Tempestade");
                   }
                   break;
               case "nose":
                   {
                       Fundo = Content.Load<Texture2D>("Fundos/faseEars");
                       SomParasita = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somParasita");
                       SomAmeba = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somAmeba");
                       SomEnzima = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somEnzima");
                       SomVirus = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somVirus");
                       SomRadion = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somRadion");
                       Musica = Content.Load<Song>("Sons/Musicas/WholeLottaLove");
                   }
                   break;
               case "eyes":
                   {
                       Fundo = Content.Load<Texture2D>("Fundos/faseEars");
                       SomParasita = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somParasita");
                       SomAmeba = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somAmeba");
                       SomEnzima = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somEnzima");
                       SomVirus = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somVirus");
                       SomRadion = Content.Load<SoundEffect>("Sons/Efeitos Sonoros/somRadion");
                       Musica = Content.Load<Song>("Sons/Musicas/Suspense");
                   } break;
             


               }
       }

        /// <summary>
        /// Atualiza a fase.
        /// </summary>
        public void Update(GameTime GT)
        {
            if (IsFuncionando)
            {
                if (GM.IsSoundOn)
                {
                    // Toca música.
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        MediaPlayer.Play(Musica);
                    }
                }
                // Esc pausa o jogo.
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    GM.Pausar();
                }
                #region Comandos de teste

                // Enter destrava o jogo.
                if ((Keyboard.GetState().IsKeyDown(Keys.Enter)) && (Travada))
                {
                    Destravar();
                }
                // Back trava o jogo.
                if ((Keyboard.GetState().IsKeyDown(Keys.Back)) && (!(Travada)))
                {
                    Travar();
                }

                // + acelera o jogo.
                if ((Keyboard.GetState().IsKeyDown(Keys.Add)) && (!(Acelerada)))
                {
                    Acelerador += 5.0f;
                    Acelerada = true;
                }

                // - lerda o jogo.
                if ((Keyboard.GetState().IsKeyDown(Keys.Subtract)) && (Acelerada))
                {
                    Acelerador -= 5.0f;
                    Acelerada = false;
                }
  
                #endregion

                #region Tratamento da Timeline
                // Realiza os eventos do nível no momento certo.
                TL.Evento(Nivel);

                // Cria novos Codons enquanto a fase não estiver encerrada.
                if (!(TL.Encerrada))
                {
                    TL.NovoCodon(Codons,30);
                }
                else
                {
                    // Assim que o último Codon é descartado, vai para a tela de score.
                    if (!(Codons.Last<Codon>().IsAlive))
                    {
                        Scorer.TotalAminos = Proteina.ContAmino;
                        Acelerador = 1f;
                        if (Proteina.ContAmino > 0)
                        {
                            Proteina.Aminos.Last<Amino>().AnimPadrao = new Animation(Content.Load<Texture2D>("Aminos/amino" + Proteina.Aminos.Last<Amino>().Tipo + "sem"), Proteina.Aminos.Last<Amino>().AnimPadrao.FrameTime, true);
                        }
                        GM.TelaScore = new TelaScore(GM);
                        GM.TelaScore.LoadMusica(Proteina.Aminos);
                        GM.Estado = GamestateManager.Gamestate.Score;
                       

                    }
                }
                #endregion
                Scorer.Update();
                #region Tratamento de Obstáculos


                foreach (ObjetoMovel o in obstaculos)
                {
                    if (o.IsAlive)
                    {
                        o.Update(GT);
                    }


                }
                #endregion
                #region Tratamento de Codons


                foreach (Codon c in codons)
                {
                    if (c.IsAlive)
                    {
                        c.Update(GT);
                    }






                }
                #endregion
                #region Tratamento de Aminos
                barraAmino.Update(GT);
                Proteina.Update(GT);

                if ((AminoEsperando != null) && (!(Proteina.Aminos.Contains(AminoEsperando))))
                {
                    aminoEsperando.Update();
                }
                #endregion
                #region Tratamento de Interferon
                barraInterf.Update(GT);
                foreach (Interferon i in Interferons)
                {
                    if (i.IsAlive)
                    {
                        i.Update(GT);
                    }
                
                }
                #endregion
            }
        }

        public void Draw(GameTime GT,SpriteBatch SB)
        {
            
            SB.Draw(Fundo, Vector2.Zero, Color.White);
            Messenger.Draw(GT, SB);
            // Desenha cada tipo de obstáculo numa ordem específica.
            #region Tratamento de Códons
            foreach (Codon c in codons)
            {
                if (c.IsAlive)
                {
                    c.Draw(GT, SB);
                }

            }
            #endregion          
            #region Tratamento das Enzimas
            foreach (Enzima e in Obstaculos.OfType<Enzima>())
            {
                if (e.IsAlive)
                {
                    e.Draw(GT, SB);
                }
            }
            #endregion
            #region Tratamento das Amebas
            foreach (Ameba a in Obstaculos.OfType<Ameba>())
            {
                if (a.IsAlive)
                {
                    a.Draw(GT, SB);
                }
            }
            #endregion
            #region Tratamento dos Radions
            foreach (Radion r in Obstaculos.OfType<Radion>())
            {
                if (r.IsAlive)
                {
                    r.Draw(GT, SB);
                }
            }
            #endregion
            #region Tratamento dos Parasitas
            foreach (Parasita p in Obstaculos.OfType<Parasita>())
            {
                if (p.IsAlive)
                {
                    p.Draw(GT, SB);
                }
            }
            #endregion
            #region Tratamento dos Virus
            foreach (Virus v in Obstaculos.OfType<Virus>())
            {
                if (v.IsAlive)
                {
                    v.Draw(GT, SB);
                }
            }
            #endregion

            #region Tratamento de Aminos
            Proteina.Draw(GT, SB);
            barraAmino.Draw(GT, SB);



            if ((AminoEsperando != null) && (!(Proteina.Aminos.Contains(AminoEsperando))))
            {
                aminoEsperando.Draw(GT, SB);
            }
            #endregion


            #region Tratamento de Interferons
            barraInterf.Draw(GT, SB);
            foreach (Interferon i in Interferons)
            {
                if (i.IsAlive)
                {
                    i.Draw(GT,SB);
                }

            }
            #endregion
        }

        /// <summary>
        /// Escreve na tela.
        /// </summary>
        public void DrawTexto(SpriteBatch SB)
        {
            // Escreve o placar com sombra.
            SB.DrawString(GM.Fonte, "Placar\n" + Scorer.Placar.ToString(), new Vector2(20, 10)+new Vector2(1,1), Color.Black, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            SB.DrawString(GM.Fonte, "Placar\n" + Scorer.Placar.ToString(), new Vector2(20, 10), new Color(0, 200, 0, 255), 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            
            // Escreve o número de Aminos em sequência com sombra.
            SB.DrawString(GM.Fonte, "Hit\n" + Scorer.SequenciaAminos.ToString(), new Vector2(BarraAminos.Retangulo.Right + 20, 10)+new Vector2(1,1), Color.Black, 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
            SB.DrawString(GM.Fonte, "Hit\n" + Scorer.SequenciaAminos.ToString(), new Vector2(BarraAminos.Retangulo.Right + 20, 10), new Color(0,200,0,255), 0f, Vector2.Zero, 0.8f, SpriteEffects.None, 0);
        }

        /// <summary>
        /// Gera um novo Codon de tipo aleatório.
        /// </summary>
        public void GerarCodon()
        {
            codons.Add(new Codon( this, Gerador.Next(1,8).ToString()));    
        }

        /// <summary>
        /// Trava a fase, zerando a velocidade de jogo.
        /// </summary>
        public void Travar()
        {
            VelocidadeDeJogo = 0;
            BarraAminos.Desativar();
            if (MediaPlayer.State == MediaState.Playing)
            {
                MediaPlayer.Stop();
            }
            if (AminoArrastando != null)
            {
                AminoArrastando.Embarrear();
            }

            foreach (ObjetoMovel o in Obstaculos)
            {
                o.IsFuncionando = false;
            }

            Travada = true;
        }

        /// <summary>
        /// Destrava a fase, retornando à velocidade anterior.
        /// </summary>
        public void Destravar()
        {
            VelocidadeDeJogo = 1f;
            BarraAminos.Ativar();
            if (GM.IsSoundOn)
            {
                MediaPlayer.Play(Musica);
            }
            foreach (ObjetoMovel o in Obstaculos)
            {
                o.IsFuncionando = true;
            }
            Travada =false;
        }

        /// <summary>
        /// Acaba com a fase.
        /// </summary>
        public void Finalizar()
        {
            MediaPlayer.Stop();
            Dispose();
        }
       
        /// <summary>
        /// Descarta os recursos da fase.
        /// </summary>
        public void Dispose()
        { content.Unload(); }
        #endregion
    }
}
