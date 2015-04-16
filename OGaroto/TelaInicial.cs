using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using OGaroto.Save;

namespace OGaroto
{
    /// <summary>
    /// Representa a tela inicial do jogo.
    /// </summary>
    class TelaInicial:Tela
    {
        //Gerenciador de saves para essa tela.
        private SaveGameManager SM = new SaveGameManager("OGaroto", "OGaroto.svg");

        #region Construtor
        /// <param name="oGM">O gerenciador de estados do jogo.</param>
         public TelaInicial(GamestateManager oGM)
        {
            GM = oGM;
            Tipo = "Inicial";
            Buttons = new List<Button>();
            LoadContent("Fundos/garoto","Sons/Musicas/Suspense");
            AdicionarBotao("Jogar", 1f);
            AdicionarBotao("Carregar jogo", 1f);
            AdicionarBotao("Opções", 1f);
            AdicionarBotao("Sair", 1f);
            LoadButtons("Interface/Buttons/buttonMaior");
            Buttons[0].SetPosition(new Vector2(Centro.X - (Buttons[0].Tamanho.X / 2), 200));
            Buttons[1].SetPosition(new Vector2(Centro.X - (Buttons[1].Tamanho.X/2), 300));
            Buttons[2].SetPosition(new Vector2(Centro.X - (Buttons[2].Tamanho.X / 2), 400));
            Buttons[3].SetPosition(new Vector2(Centro.X - (Buttons[3].Tamanho.X / 2), 500));

        }
        #endregion

        #region Métodos
         /// <summary>
        /// Atualiza a tela.
        /// </summary>
        public void Update()
        {
            if (IsFuncionando)
            {
                TocarMusica();
                foreach (Button btn in Buttons)
                {

                    btn.Update();
                    if (GM.MouseManager.NovoClique(btn))
                    {
                        // Escolhe o efeito baseado no texto do botão pressionado.
                        switch (btn.Texto)
                        {
                            case "Jogar":
                                {
                                    if (GM.TelaGaroto == null)
                                    {
                                        GM.TelaGaroto = new TelaGaroto(GM);
                                        // Aqui deve ser decidido quais partes estarão ativas.
                                        GM.TelaGaroto.Boy.HabilitarParte("leftFoot");
                                        GM.TelaGaroto.Boy.HabilitarParte("rightFoot");
                                        GM.TelaGaroto.Boy.HabilitarCorpo();
                                    }
                                    GM.Estado = GamestateManager.Gamestate.Garoto;
                                } break;

                            case "Carregar jogo":
                                {
                                    SaveData dados = SM.Carregar();

                                    if (GM.TelaGaroto == null)
                                    {
                                        GM.TelaGaroto = new TelaGaroto(GM);
                                        // Aqui carregam-se as partes ativas.
                                        if ((dados.PartesAtivadas==null)||(dados.PartesAtivadas.Count==0))
                                        {
                                            GM.TelaGaroto.Boy.HabilitarParte("leftFoot");
                                            GM.TelaGaroto.Boy.HabilitarParte("rightFoot");
                                        }
                                        else
                                        {
                                            foreach (string parte in dados.PartesAlteradas)
                                            {
                                                GM.TelaGaroto.Boy.LoadParte(parte, "1");
                                            }

                                            foreach (string parte in dados.PartesAtivadas)
                                            {
                                                GM.TelaGaroto.Boy.HabilitarParte(parte);
                                            }
                                        }
                                    }
                                    GM.Estado = GamestateManager.Gamestate.Garoto;
                                }
                                break;
                            case "Opções":
                                {
                                    if (GM.TelaOptions == null)
                                    {
                                        GM.TelaOptions = new TelaOptions(GM);                                       
                                   }
                                    GM.TelaOptions.IrDe(this);
                                } break;
                            case "Sair": { GM.Game.Exit(); } break;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Desenha texto na tela.
        /// </summary>
        public void DrawTexto(SpriteBatch SB)
        {
            // Desenha no centro da tela.
            SB.DrawString(GM.Fonte, "O Garoto", new Vector2(this.Centro.X - (GM.Fonte.MeasureString("O Garoto").X) * 1.5f, 30) + new Vector2(1, 1), Color.Black, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0);
            SB.DrawString(GM.Fonte, "O Garoto", new Vector2(this.Centro.X - (GM.Fonte.MeasureString("O Garoto").X) * 1.5f, 30), Color.Lime, 0f, Vector2.Zero, 3f, SpriteEffects.None, 0);
        }
#endregion
    }
}
