using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OGaroto
{
    /// <summary>
    /// Representa um elemento visual formador do Garoto.
    /// </summary>
    class Parte:ObjetoFixo
    {
         #region Propriedades e accessors
        Tela tela;
        /// <summary>
        /// A tela à qual pertence.
        /// </summary>
        public Tela Tela
        {
            set { tela=value; }
            get { return tela; }
        }

        private Garoto boy;
        /// <summary>
        /// O Garoto ao qual pertence esta parte.
        /// </summary>
        public Garoto Boy

        {
            set { boy = value; }
            get { return boy; }
        }

        private string tipo;
        /// <summary>
        /// O tipo de parte do corpo.
        /// </summary>
        public string Tipo
        {
            set { tipo = value; }
            get { return tipo; }
        }

        private string numero;
        /// <summary>
        /// O número do conjunto ao qual pertence esta parte.
        /// </summary>
        public string Numero
        {
            set { numero = value; }
            get { return numero; }
        }

        private bool isFocada;
        /// <summary>
        /// Determina se a parte está em foco do mouse.
        /// </summary>
        public bool IsFocada
        {
            set { isFocada = value; }
            get { return isFocada; }
        }

        private Vector2 escalaVet;
        /// <summary>
        /// Escala horizontal e vertical da parte.
        /// </summary>
        public Vector2 EscalaVetorial
        {
            set { escalaVet = value; }
            get { return escalaVet; }
        }

#endregion

        #region Construtores
        /// <param name="oGaroto">O Garoto ao qual pertencerá a parte.</param>
        /// <param name="aPosition">A posição.</param>
        /// <param name="oTipo">O tipo de parte do corpo.</param>
        public Parte(Garoto oGaroto,Vector2 aPosition,string oTipo)
        {           
            Boy = oGaroto;
            Tela = oGaroto.Tela;
            Position = aPosition;
            Tipo = oTipo;
            Escala = 1f;          
        }


        /// <param name="oGaroto">O Garoto ao qual pertencerá a parte.</param>
        /// <param name="oTipo">O tipo de parte do corpo.</param>
        public Parte(Garoto oGaroto, string oTipo)
        {
            Boy = oGaroto;
            Tela = oGaroto.Tela;
            Tipo = oTipo;
            Escala = 1f;
        }


        /// <param name="oTipo">O tipo de parte do corpo.</param>
        /// <param name="oNumero">O número do conjunto ao qual pertence a parte.</param>
        public Parte(Tela aTela,string oTipo,string oNumero)
        {
            Tipo = oTipo;
            Tela = aTela;
            LoadContent(oNumero);
        }


        #endregion

        #region Métodos
        /// <summary>
        /// Define a posição da parte.
        /// </summary>
        /// <param name="aPosition">A nova posição.</param>
        public void SetPosition(Vector2 aPosition)
        {
            Position = aPosition;
        }

        /// <summary>
        /// Carrega os recursos da parte.
        /// </summary>
        /// <param name="num">O número do conjunto ao qual pertence esta parte.</param>
        public new void LoadContent(string num)
        {
            Numero = num;
            Imagem = Tela.GM.Content.Load<Texture2D>("Partes/"+Tipo+num);
            Tamanho = new Vector2(Imagem.Width,Imagem.Height);
        }

        /// <summary>
        /// Atualiza a parte.
        /// </summary>
        public void Update()
        {
            if (Tela.GM.MouseManager.MouseEmCima(this))
            {

                if (Boy.ParteFocada==null)
                {
                    if(Clicavel)
                    {
                    this.IsFocada = true;
                    Boy.ParteFocada = this;
                    }
                }

            }
            else
            {
                if (this.IsFocada)
                {
                    this.IsFocada = false;
                    Boy.ParteFocada = null;
                }
            }
            Escala = IsFocada ? 1.1f : 1f;
            Boy.MontarCorpo(Tela.Centro);
        }

        /// <summary>
        /// Desenha a imagem da parte.
        /// </summary>
        public void Draw(SpriteBatch SB)
        {
           
            SB.Draw(Imagem, Position, null, Color.White, 0f, Vector2.Zero, Escala, SpriteEffects.None, 1);
        }
        #endregion
    }
}
