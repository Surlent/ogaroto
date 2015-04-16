using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OGaroto
{
    /// <summary>
    /// Representa um objeto usável de interface.
    /// </summary>
    class ObjetoInterface:ObjetoFixo
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

       
        /// <param name="oGM">O gerenciador de estados do jogo.</param>
        /// <param name="caminho">O caminho da imagem.</param>
        public ObjetoInterface(GamestateManager oGM,string caminho)
        {
            GM = oGM;
            LoadContent(caminho);
        }

        /// <summary>
        /// Carrega a imagem do objeto.
        /// </summary>
        /// <param name="caminho">O caminho da imagem.</param>
        public new void LoadContent(string caminho)
        {
            Imagem = GM.Content.Load<Texture2D>(caminho);
            Tamanho = new Vector2(Imagem.Width, Imagem.Height);
        }

        /// <summary>
        /// Desenha o objeto de interface.
        /// </summary>
        public void Draw(SpriteBatch SB)
        {
            SB.Draw(Imagem, Retangulo, Color.White);
        }

       
    }
}
