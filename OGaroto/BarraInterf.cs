using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace OGaroto
{
    /// <summary>
    /// Representa uma barra onde estão os interferons.
    /// </summary>
    class BarraInterf : ObjetoFixo
    {
        #region Propriedades e accessors
        List<Button> interfBtns;
        /// <summary>
        /// A lista de Interferons na barra.
        /// </summary>
        public List<Button> InterfButtons
        {
            get { return interfBtns; }
        }

        List<Rectangle> slots;
        /// <summary>
        /// Lista de espaços para Interferons na barra.
        /// </summary>
        public List<Rectangle> Slots
        {
            get { return slots; }
        }

     
        #endregion

        #region Construtor
        /// <param name="aFase">A fase atual.</param>
        public BarraInterf(Fase aFase)
        {
            Fase = aFase;
            interfBtns = new List<Button>(5);
            slots = new List<Rectangle>(5);
            LoadContent("Interface/Misc/barraInterf");
            Escala = 1f;
            Tamanho = new Vector2(Imagem.Width, Imagem.Height);
           
            this.Position  = new Vector2(10, 100);
            Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
            Slots.Add(new Rectangle((int)Position.X, (int)Position.Y+5, 70, 70));

            // Define os espaços dos Interferons na barra.
            for (int i = 0; i < Slots.Capacity; i++)
            {
                if (i != 0)
                {
                    Rectangle slotAnterior = Slots.Last<Rectangle>();
                    Slots.Add(new Rectangle(slotAnterior.Left, slotAnterior.Bottom, slotAnterior.Width, slotAnterior.Height));
                }

                InterfButtons.Add(new Button(this.Fase,"Interface/Misc/interf"+i.ToString(),1f));
                InterfButtons[i].LoadContent("Interface/Buttons/buttonMenor");
                InterfButtons[i].Centro = new Vector2(slots[i].Center.X,slots[i].Center.Y);
 
            }
            InterfButtons[0].Delay = 20000f;
            InterfButtons[1].Delay = 5000f;
            InterfButtons[2].Delay = 10000f;
            InterfButtons[3].Delay = 10000f;
            InterfButtons[4].Delay = 20000f;


        }
        #endregion

        #region Métodos
        /// <summary>
        /// Atualiza os Interferons da barra.
        /// </summary>
        public void Update(GameTime GT)
        {
            for(int i=0;i<InterfButtons.Count;i++)
            {
                if (InterfButtons[i].IsDisponivel)
                {
                    if (InterfButtons[i].Clicavel)
                    {
                        InterfButtons[i].Update();
                        if (Fase.MouseManager.NovoClique(InterfButtons[i]))
                        {
                            NovoInterf(i);
                            InterfButtons[i].Clicavel = false;
                        }
                    }
                    else
                    {
                        if (InterfButtons[i].Timer < InterfButtons[i].Delay)
                        {
                            InterfButtons[i].Timer += GT.ElapsedGameTime.Milliseconds;
                        }
                        else
                        {
                            InterfButtons[i].Clicavel = true;
                            InterfButtons[i].Timer = 0;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Desenha os Interferons e a imagem da barra.
        /// </summary>
        public void Draw(GameTime GT, SpriteBatch SB)
        {
            SB.Draw(Imagem, Position, null, Color.White, 0, Vector2.Zero, Escala, SpriteEffects.None, 0);
            foreach (Button btn in InterfButtons)
            {
                btn.Draw(GT,SB);
            }
        }

        /// <summary>
        /// Gera um Interferon na posição certa da barra.
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public void NovoInterf(int indice)
        {
            TiposInterferon tipo;
            switch (indice)
            {
                case 0:
                    {
                    tipo = TiposInterferon.Pilula;
                    }
                    break;
                case 1:
                    tipo = TiposInterferon.Lisossomo;
                    break;
                case 2:
                    tipo = TiposInterferon.FeixeATP;
                    break;
                case 3:
                    tipo = TiposInterferon.Ressonante;
                    break;
                case 4:
                    tipo = TiposInterferon.Imunizador;
                    break;
                default:
                    tipo = TiposInterferon.Pilula;
                    break;
            }
           Fase.Interferons.Add(new Interferon(Fase, tipo, new Vector2(0, Fase.Centro.Y),1f));
        }

        public void DesativarBotao(int indice)
        {
            InterfButtons[indice].Clicavel = false;
        }

        public void AtivarBotao(string oTipo)
        {
            switch (oTipo)
            {
                case "Pilula": 
                    {
                        InterfButtons[0].IsDisponivel = true;
                } break;

                case "Lisossomo":
                    {
                        InterfButtons[1].IsDisponivel = true;
                    } break;
                case "FeixeATP":
                    {
                        InterfButtons[2].IsDisponivel = true;
                    } break;
                case "Ressonante":
                    {
                        InterfButtons[3].IsDisponivel = true;
                    } break;
                case "Imunizador":
                    {
                        InterfButtons[4].IsDisponivel = true;
                    } break;
                
            }
        }
        #endregion
    }
}
