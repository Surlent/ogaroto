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
    /// Representa a tela de pause do jogo.
    /// </summary>
    class TelaPause:Tela
    {
        //Implementa a interface de save
        private SaveGameManager SM = new SaveGameManager("OGaroto", "OGaroto.svg");
        
        #region Construtor
        /// <param name="oGM">O gerenciador de estados do jogo.</param>
        public TelaPause(GamestateManager oGM)
        {
            GM = oGM;
            Tipo = "Pause";
            Buttons = new List<Button>();
            LoadContent("Fundos/pause");
            AdicionarBotao("Continuar",  1f);
            AdicionarBotao("Reiniciar", 1f);
            AdicionarBotao("Salvar", 1f);
            AdicionarBotao("Garoto", 1f);
            AdicionarBotao("Opções", 1f);
            AdicionarBotao("Sair", 1f);
            LoadButtons("Interface/Buttons/buttonMaior");
            Buttons[0].SetPosition(new Vector2(Centro.X - (Buttons[0].Tamanho.X / 2), 140));
            Buttons[1].SetPosition(new Vector2(Centro.X - (Buttons[1].Tamanho.X / 2), 220));
            Buttons[2].SetPosition(new Vector2(Centro.X - (Buttons[2].Tamanho.X / 2), 300));
            Buttons[3].SetPosition(new Vector2(Centro.X - (Buttons[3].Tamanho.X / 2), 380));
            Buttons[4].SetPosition(new Vector2(Centro.X - (Buttons[4].Tamanho.X / 2), 460));
            Buttons[5].SetPosition(new Vector2(Centro.X - (Buttons[5].Tamanho.X / 2), 540));
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

                foreach (Button btn in Buttons)
                {

                    btn.Update();
                    if (GM.MouseManager.NovoClique(btn))
                    {
                        // Escolhe o efeito com base no texto do botão.
                        switch (btn.Texto)
                        {
                            case "Continuar":
                                {
                                    GM.Despausar();

                                } break;

                            case "Reiniciar":
                                {
                                    GM.Fase = new Fase(GM, GM.Fase.Nivel);
                                    GM.Fase.LoadContent();
                                    GM.Despausar();
                                }
                                break;

                            case "Salvar":
                                {
                                    SaveData dados = new SaveData();
                                    dados.PartesAtivadas = new List<string>();
                                    foreach (Parte p in GM.TelaGaroto.Boy.Corpo)
                                    {
                                        if (p.Clicavel)
                                            dados.PartesAtivadas.Add(p.Tipo);
                                    }

                                    SM.Salvar(dados);
                                }
                                break;
                            case "Garoto":
                                {
                                    GM.VoltarGaroto();
                                } break;
                            case "Opções":
                                {
                                    if (GM.TelaOptions == null)
                                    {
                                        GM.TelaOptions = new TelaOptions(GM);
                                    }
                                    GM.TelaOptions.IrDe(this);
                                
                                }
                                break;
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
            SB.DrawString(GM.Fonte, "Pause", new Vector2(this.Centro.X - (GM.Fonte.MeasureString("Pause").X), 50), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
        }

       
        #endregion
    }
}
