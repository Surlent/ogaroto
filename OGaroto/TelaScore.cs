using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace OGaroto
{
    /// <summary>
    /// Representa a tela de pontuação do jogo.
    /// </summary>
    class TelaScore:Tela
    {
        #region Propriedades e accessors
        private Proteina proteina;
        /// <summary>
        /// A proteína da fase jogada.
        /// </summary>
        public Proteina Proteina
        {
            set { proteina = value; }
            get { return proteina; }
        }

        private List<SoundEffectInstance> musicaAminos;
        /// <summary>
        /// A música formada pelos sons dos Aminos.
        /// </summary>
        public List<SoundEffectInstance> MusicaAminos
        {
            set { musicaAminos = value; }
            get { return musicaAminos; }
        }

        private SoundEffectInstance somAtual;
        /// <summary>
        /// A parte da música dos Aminos sendo tocada.
        /// </summary>
        public SoundEffectInstance SomAtual
        {
            set { somAtual = value; }
            get { return somAtual; }
        }

        private int indiceMusica=0;
        /// <summary>
        /// O índice do som da música dos Aminos.
        /// </summary>
        public int IndiceMusica
        {
            set { indiceMusica = value; }
            get { return indiceMusica; }
        }
        #endregion

        #region Construtor
        /// <param name="oGM">O gerenciador de estados do jogo.</param>
         public TelaScore(GamestateManager oGM)
        {
            GM = oGM;
            Tipo = "Score";
            Buttons = new List<Button>();
            MusicaAminos = new List<SoundEffectInstance>();
            LoadContent("Fundos/score","Sons/Musicas/Suspense");
            AdicionarBotao("Continuar", 1f);
            LoadButtons("Interface/Buttons/buttonMaior");
             // Define separadamente a posição do botão.
            Buttons[0].SetPosition(new Vector2(Centro.X-(Buttons[0].Tamanho.X/2), 550));

            if (GM.Fase.Proteina.ContAmino > 0)
            {
                Proteina = GM.Fase.Proteina;

                // Prepara o último Amino da proteína para levar os demais.
                Proteina.UltimoAmino.Estado = Amino.EstadoAmino.Flutuando;
                Proteina.UltimoAmino.Clicavel = true;

                // Reabilita as funções dos Aminos na proteína.
                for (int i = 0; i < Proteina.ContAmino; i++)
                {

                    Proteina.Aminos[i].Ativar();

                    // Liga os Aminos e restabelece seu estado, exceto pelo último.
                    if (i != Proteina.ContAmino - 1)
                    {
                        Proteina.Aminos[i].Estado = Amino.EstadoAmino.Proteina;
                        Proteina.Aminos[i].AminoLigado = Proteina.Aminos[i + 1];
                    }
                }
            }
        }
        #endregion

        #region Métodos
         /// <summary>
        /// Carrega a música dos Aminos.
        /// </summary>
        /// <param name="proteina">A proteína formada na fase.</param>
         public void LoadMusica(List<Amino> proteina)
         { 
             
         foreach(Amino a in proteina)
         {
             MusicaAminos.Add(a.SomUnico.CreateInstance());
         }
         }

        /// <summary>
        /// Atualiza a tela.
        /// </summary>
        public void Update()
        {
            if(IsFuncionando)
            {
                // Toca a música se houver ao menos um Amino na proteína.
            if (MusicaAminos.Count>0)
            {
                AtualizarMusica();
            }

            foreach (Button btn in Buttons)
            {

                btn.Update();

                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    GM.Estado = GamestateManager.Gamestate.Garoto;
                }

                if (GM.MouseManager.NovoClique(btn))
                {
                    switch (btn.Texto)
                    {
                        case "Continuar":
                            {
                                GM.Fase.Finalizar();
                                GM.Estado = GamestateManager.Gamestate.Garoto;
                            } break;


                    }
                }

            }
            }
        }

        /// <summary>
        /// Desenha cada Amino da proteína.
        /// </summary>
        public void DrawProteina(GameTime GT,SpriteBatch SB)
        {
            if (Proteina != null)
            {
                foreach (Amino amin in Proteina.Aminos)
                {
                    amin.Draw(GT, SB);
                }
            }
        }

        /// <summary>
        /// Atualiza cada Amino da proteína.
        /// </summary>
        public void UpdateProteina(GameTime GT)
        {
            if (Proteina != null)
            {
                foreach (Amino amin in Proteina.Aminos)
                {
                    amin.Update();
                }
            }
        }

        /// <summary>
        /// Desenha os dados do jogo na tela, na cor laranja.
        /// </summary>
        public void DrawTexto(SpriteBatch SB)
        {      
            GM.Fase.Scorer.Draw(SB, new Vector2(70, 100), 2f, "Rank: " + GM.Fase.Scorer.Rank.ToString(), Color.Orange);
            GM.Fase.Scorer.Draw(SB, new Vector2(70, 250), 1.5f, "Placar: " + GM.Fase.Scorer.Placar.ToString(), Color.Orange);
            GM.Fase.Scorer.Draw(SB, new Vector2(70, 330), 1.5f, "Pureza da Proteína: " + Math.Round(GM.Fase.Scorer.PorcentagemAcerto, 1).ToString() + "%", Color.Orange);
            GM.Fase.Scorer.Draw(SB, new Vector2(70, 410), 1.5f, "Maior Sequência: " + GM.Fase.Scorer.MaiorSequencia.ToString(), Color.Orange);
        }

        /// <summary>
        /// Desenha os dados do jogo na tela, na cor escolhida.
        /// </summary>
        public void DrawTexto(SpriteBatch SB,Color cor)
        {
            GM.Fase.Scorer.Draw(SB, new Vector2(70, 100), 2f, "Rank: " + GM.Fase.Scorer.Rank.ToString(), cor);
            GM.Fase.Scorer.Draw(SB, new Vector2(70, 250), 1.5f, "Placar: " + GM.Fase.Scorer.Placar.ToString(), cor);
            GM.Fase.Scorer.Draw(SB, new Vector2(70, 330), 1.5f, "Pureza da Proteína: " + Math.Round(GM.Fase.Scorer.PorcentagemAcerto, 1).ToString() + "%",cor);
            GM.Fase.Scorer.Draw(SB, new Vector2(70, 410), 1.5f, "Maior Sequência: " + GM.Fase.Scorer.MaiorSequencia.ToString(), cor);
        }

        /// <summary>
        /// Toca a música dos Aminos.
        /// </summary>
        public void AtualizarMusica()
        {
            if (GM.IsSoundOn)
            {
                // Reseta se não houver som sendo tocado.
                if (SomAtual == null)
                {
                    ResetarMusica();
                }

                // Toca se nenhum som estiver sendo tocado.
                else if (SomAtual.State != SoundState.Playing)
                {
                    // Se chegar ao último som, reseta.
                    if (SomAtual == MusicaAminos.Last<SoundEffectInstance>())
                    {
                        ResetarMusica();
                    }
                    // Passa para o próximo som caso o anterior tenha sido tocado.
                    else
                    {
                        SomAtual = MusicaAminos[IndiceMusica + 1];
                        IndiceMusica++;
                    }
                }


                SomAtual.Play();
            }
        }

        /// <summary>
        /// Zera a música, retornando o índice a 0 e o SomAtual ao primeiro da lista.
        /// </summary>
        public void ResetarMusica()
        {
            SomAtual = MusicaAminos.First<SoundEffectInstance>();
            IndiceMusica = 0;
        }
#endregion
    }
}
