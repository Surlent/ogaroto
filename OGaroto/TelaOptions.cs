using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace OGaroto
{
    /// <summary>
    /// Representa a tela de configurações.
    /// </summary>
    class TelaOptions:Tela
    {
        #region Propriedades e accessors
        private Tela telaAnterior;
        /// <summary>
        /// A tela que chamou esta.
        /// </summary>
        public Tela TelaAnterior
        {
            set { telaAnterior = value; }
            get { return telaAnterior; }
        }
        #endregion

        #region Construtor
        /// <param name="oGM">O gerenciador de estados do jogo.</param>
         public TelaOptions(GamestateManager oGM)
        {
            GM = oGM;
            Tipo = "Options";
            Buttons = new List<Button>();
            LoadContent("Fundos/score","Sons/Musicas/Suspense");
            AdicionarBotao("Voltar", 1f);
            AdicionarBotao("On/Off", 0.7f);
            AdicionarBotao("On/Mudo", 0.7f);
            LoadButtons("Interface/Buttons/buttonMaior");
            // Define separadamente a posição do botão.
            Buttons[0].SetPosition(new Vector2(Centro.X - (Buttons[0].Tamanho.X / 2), 550));
            Buttons[1].SetPosition(new Vector2(Centro.X - (Buttons[1].Tamanho.X / 2), 200+GM.Fonte.MeasureString("Tela Cheia").Y));
            Buttons[2].SetPosition(new Vector2(Centro.X - (Buttons[2].Tamanho.X / 2), 300 + GM.Fonte.MeasureString("Som").Y));

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
                             case "Voltar":
                                 {
                                     switch(TelaAnterior.Tipo)
                                     {
                                         case "Inicial":
                                         {
                                             GM.Estado = GamestateManager.Gamestate.Inicio;
                                         }
                                         break;

                                         case "Pause":
                                         {
                                             MediaPlayer.Stop();
                                             GM.Estado = GamestateManager.Gamestate.Pause;
                                         }
                                         break;
                                     }
                                 } break;

                             case "On/Off":
                                 {
                                     GM.Graphics.ToggleFullScreen();
                                 }
                                 break;
                             case "On/Mudo":
                                 {
                                     if (GM.IsSoundOn)
                                     {
                                         GM.IsSoundOn = false;
                                         MediaPlayer.Stop();
                                     }
                                     else
                                     {
                                         GM.IsSoundOn = true;
                                     }
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
             string oTexto1 = "Tela Cheia";
             SB.DrawString(GM.Fonte, oTexto1, new Vector2(this.Centro.X - (GM.Fonte.MeasureString(oTexto1).X) /2f,200) + new Vector2(1, 1), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
             SB.DrawString(GM.Fonte, oTexto1, new Vector2(this.Centro.X - (GM.Fonte.MeasureString(oTexto1).X) /2f, 200), Color.Lime, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);

             string oTexto2 = "Opções";
             SB.DrawString(GM.Fonte, oTexto2, new Vector2(this.Centro.X - (GM.Fonte.MeasureString(oTexto2).X / 1) + 1, 30 + 1), Color.Black, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);
             SB.DrawString(GM.Fonte, oTexto2, new Vector2(this.Centro.X - (GM.Fonte.MeasureString(oTexto2).X / 1), 30), Color.Lime, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0);

             string oTexto3 = "Som";
             SB.DrawString(GM.Fonte, oTexto3, new Vector2(this.Centro.X - (GM.Fonte.MeasureString(oTexto1).X) /2f,300) + new Vector2(1, 1), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
             SB.DrawString(GM.Fonte, oTexto3, new Vector2(this.Centro.X - (GM.Fonte.MeasureString(oTexto1).X) /2f, 300), Color.Lime, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0);
         }

         /// <summary>
         /// Muda para esta tela memorizando a anterior.
         /// </summary>
         /// <param name="telaAnterior">A tela que chamou esta.</param>
         public void IrDe(Tela telaAnt)
         {
             TelaAnterior = telaAnt;
             GM.Estado = GamestateManager.Gamestate.Options;
         }
#endregion
    }
}
