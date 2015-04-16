using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OGaroto
{
    /// <summary>
    /// Representa uma barra da qual se tira Aminos.
    /// </summary>
    class BarraAmino:ObjetoFixo
    {
        #region Propriedades e accessors
        List<Amino> aminos;
        /// <summary>
        /// A lista de Aminos na barra.
        /// </summary>
        public List<Amino> Aminos
        {
            get { return aminos; }
        }

        /// <summary>
        /// Gerador de números aleatórios.
        /// </summary>
        Random r;
       
        List<Rectangle> slots;
        /// <summary>
        /// Lista de espaços para Aminos na barra.
        /// </summary>
        public List<Rectangle> Slots
        {
            get { return slots; }
        }
        #endregion

        #region Construtor
        /// <param name="aFase">A fase atual.</param>
        /// <param name="LimiteAmino">O número máximo de Aminos na barra.</param>
        public BarraAmino(Fase aFase,int LimiteAmino)
        {
            r = new Random();
            Fase = aFase;
            aminos = new List<Amino>(LimiteAmino);
            slots = new List<Rectangle>(LimiteAmino);
            LoadContent("Interface/Misc/barraAmino");
            Tamanho = new Vector2(Imagem.Width, Imagem.Height);
            Escala = 1f;
            this.Centro =new Vector2(Fase.Centro.X,50);
            Retangulo = new Rectangle((int)Position.X, (int)Position.Y, (int)Tamanho.X, (int)Tamanho.Y);
            Slots.Add(new Rectangle((int)Position.X+20,(int)Position.Y,70,70));

            // Define os espaços dos Aminos na barra.
            for (int i = 0; i < Slots.Capacity; i++)
            {
                if (i != 0)
                {
                    Rectangle slotAnterior = Slots.Last<Rectangle>();
                    Slots.Add(new Rectangle(slotAnterior.Right + 1, slotAnterior.Y, slotAnterior.Width, slotAnterior.Height));
                }
                Aminos.Add(NovoAmino(i));
            }


        }
        #endregion

        #region Métodos
        /// <summary>
        /// Atualiza os Aminos da barra.
        /// </summary>
        public void Update(GameTime GT)
        {
            GerenciarAminos();
            for (int i = 0; i < Aminos.Count; i++)
            {
                Aminos[i].Update();
            }      
        }

        /// <summary>
        /// Desenha os Aminos e a imagem da barra.
        /// </summary>
        public void Draw(GameTime GT,SpriteBatch SB)
        {
            SB.Draw(Imagem, Position, null, Color.White, 0f, Vector2.Zero, 1.0f, SpriteEffects.None, 0);
            foreach (Amino a in Aminos)
            {
                a.Draw(GT,SB);
            }
        }

        /// <summary>
        /// Gera um Amino do tipo (indice+1) na posição certa da barra.
        /// </summary>
        /// <param name="indice"></param>
        /// <returns></returns>
        public Amino NovoAmino(int indice)
        {
          
        Amino amin=new Amino(Fase,(indice+1).ToString(),new Vector2(Slots[indice].X,Slots[indice].Y),0.75f);
        return amin;
        }

        /// <summary>
        /// Repõe os Aminos que saem da barra.
        /// </summary>
        public void GerenciarAminos()
        {
                for (int i = 0; i < Aminos.Capacity; i++)
                {
                    if ((Aminos[i].Estado == Amino.EstadoAmino.Encaixado)||(Aminos[i].IsAlive==false))
                    {
                        Aminos[i] = NovoAmino(i);
                    }
                }
            
        }

        /// <summary>
        /// Torna os Aminos clicáveis.
        /// </summary>
        public void Ativar()
        {
            foreach (Amino a in Aminos)
            {
                a.Clicavel = true;
            }
        }

        /// <summary>
        /// Torna os Aminos não-clicáveis.
        /// </summary>
        public void Desativar()
        {
            foreach (Amino a in Aminos)
            {
                a.Clicavel = false;
            }
        }
        #endregion
    }
}
