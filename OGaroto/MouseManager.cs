using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace OGaroto
{
    /// <summary>
    /// Gerencia os eventos e a posição do mouse.
    /// </summary>
    class MouseManager
    {
        #region Propriedades e accessors
        private GamestateManager gM;
        /// <summary>
        /// O gerenciador de estados do jogo.
        /// </summary>
        public GamestateManager GM
        {
            set { gM = value; }
            get { return gM; }
        }

        /// <summary>
        /// Determina a posição do Mouse.
        /// </summary>
        public Vector2 Position
        {
            set {
                Mouse.SetPosition((int)value.X, (int)value.Y);
            }
            get { return new Vector2(Mouse.GetState().X, Mouse.GetState().Y); }
        }

        private bool cliquePronto=false;
        /// <summary>
        /// Determina se um clique já foi efetuado.
        /// </summary>
        public bool CliquePronto
        {
            set { cliquePronto = value; }
            get { return cliquePronto; }

        }
        #endregion

        #region Construtor
        /// <param name="oGM">O gerenciador de estados do jogo.</param>
        public MouseManager(GamestateManager oGM)
        {
            GM = oGM;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Atualiza os cliques de Mouse.
        /// </summary>
        public void Update()
        {
            if (EsquerdoSoltou())
            {
                CliquePronto = false;
            }
          
        }

        /// <summary>
        /// Determina se o Mouse está em cima de um objeto.
        /// </summary>
        /// <param name="o">O objeto possivelmente sob o Mouse.</param>
        public bool MouseEmCima(ObjetoMovel o)
        {
            MouseState MS = Mouse.GetState();
            // Determina se as coordenadas do Mouse estão dentro do retângulo do objeto.
            if ((MS.X > o.Retangulo.Left) && (MS.X < o.Retangulo.Right) && (MS.Y > o.Retangulo.Top) && (MS.Y < o.Retangulo.Bottom))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determina se o Mouse está em cima de um objeto.
        /// </summary>
        /// <param name="o">O objeto possivelmente sob o Mouse.</param>
        public bool MouseEmCima(ObjetoFixo o)
        {
            MouseState MS = Mouse.GetState();
            // Determina se as coordenadas do Mouse estão dentro do retângulo do objeto.
            if ((MS.X > o.Retangulo.Left) && (MS.X < o.Retangulo.Right) && (MS.Y > o.Retangulo.Top) && (MS.Y < o.Retangulo.Bottom))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Determina se um Mouse está sendo pressionado sobre um objeto.
        /// </summary>
        /// <param name="o">O objeto possivelmente sendo pressionado.</param>
        public bool MousePressionando(ObjetoMovel o)
        {
            if ((MouseEmCima(o)) && (EsquerdoPressionou()) && (o.Clicavel))
            {

                return true;
            }
            else return false;
        }

        /// <summary>
        /// Determina se um Mouse está sendo pressionado sobre um objeto.
        /// </summary>
        /// <param name="o">O objeto possivelmente sendo pressionado.</param>
        public bool MousePressionando(ObjetoFixo o)
        {
            if ((MouseEmCima(o)) && (EsquerdoPressionou()) && (o.Clicavel))
            {

                return true;
            }
            else return false;
        }

        /// <summary>
        /// Determina se o botão estava pressionado no Update anterior.
        /// </summary>
        /// <returns>True se não estava sendo pressionado, false caso contrário</returns>
        public bool NovoClique()
        {
            if ((!(CliquePronto)) && (EsquerdoPressionou()))
                return true;
            else return false;
        }

        /// <summary>
        /// Determina se o botão estava pressionado sobre o objeto no Update anterior.
        /// </summary>
        /// <param name="o">O objeto possivelmente sendo pressionado.</param>
        /// <returns>True se não estava sendo pressionado, false caso contrário</returns>
        public bool NovoClique(ObjetoMovel o)
        {
            if ((!(CliquePronto)) && (MousePressionando(o)))
            {
                CliquePronto = true;
                return true;
            }
            else
            {
                return false;
            }
        
        }

        /// <summary>
        /// Determina se o botão estava pressionado sobre o objeto no Update anterior.
        /// </summary>
        /// <param name="o">O objeto possivelmente sendo pressionado.</param>
        /// <returns>True se não estava sendo pressionado, false caso contrário</returns>
        public bool NovoClique(ObjetoFixo o)
        {
            if ((!(CliquePronto)) && (MousePressionando(o)))
            {
                CliquePronto = true;
                return true;
            }
            else
            {
                return false;
            }
        
        }

        /// <summary>
        /// Determina se o botão esquerdo está solto.
        /// </summary>
        public bool EsquerdoSoltou()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Released)
            { return true; }
            else return false;
        }

        /// <summary>
        /// Determina se o botão esquerdo está pressionado.
        /// </summary>
        /// <returns></returns>
        public bool EsquerdoPressionou()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            { return true; }
            else return false;
        }
        #endregion
    }
}
