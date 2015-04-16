using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace OGaroto
{
    /// <summary>
    /// Representa o Garoto, cujas partes correspondem a fases do jogo.
    /// </summary>
    class Garoto
    {
        #region Propriedades e accessors
        Tela tela;
        /// <summary>
        /// A tela à qual pertence.
        /// </summary>
        public Tela Tela
        {
            set { tela = value; }
            get { return tela; }
        }

        protected Vector2 position;
        /// <summary>
        /// Determina a posição.
        /// </summary>
        public Vector2 Position
        {
            set { position = value; }
            get { return position; }
        }

        /// <summary>
        /// Determina o centro.
        /// </summary>
        public Vector2 Centro
        {
            set
            {
                corpo[0].Centro = value; ;

            }
            get { return corpo[0].Centro; }
        }

        private List<Parte> corpo; 
        /// <summary>
        /// As partes que formam o corpo.
        /// </summary>
        public List<Parte> Corpo
        {
        set{corpo=value;}
            get{return corpo;}
        }

        private Parte parteFocada;
        /// <summary>
        /// A parte sobre a qual está o Mouse.
        /// </summary>
        public Parte ParteFocada
        {
            set { parteFocada = value; }
            get { return parteFocada; }
        }



       
        #endregion

        #region Construtor
        /// <param name="aTela">A tela ao qual pertence o Garoto.</param>
        public Garoto(Tela aTela) 
        {
            Tela = aTela;
            Corpo = new List<Parte>(15);
            Corpo.Add(new Parte(this,"trunk")); // O Tronco 
            Corpo.Add(new Parte(this,"head")); // A cabeça
            Corpo.Add(new Parte(this, "leftArm")); // O braço esquerdo
            Corpo.Add(new Parte(this, "rightArm")); // O braço direito
            Corpo.Add(new Parte(this, "leftLeg")); // A perna esquerda
            Corpo.Add(new Parte(this, "rightLeg")); // A perna direita
            Corpo.Add(new Parte(this, "leftHand")); // A mão esquerda
            Corpo.Add(new Parte(this, "rightHand")); // A mão direita
            Corpo.Add(new Parte(this, "leftFoot")); // O pé esquerdo
            Corpo.Add(new Parte(this, "rightFoot")); // O pé direito
            Corpo.Add(new Parte(this, "eyes")); // Os olhos
            Corpo.Add(new Parte(this, "nose")); // O nariz
            Corpo.Add(new Parte(this, "mouth")); // A boca           
            Corpo.Add(new Parte(this, "leftEar")); // A orelha esquerda
            Corpo.Add(new Parte(this, "rightEar")); //A orelha direita


        }
        #endregion

        #region Métodos
        /// <summary>
        /// Atualiza o Garoto.
        /// </summary>
        public void Update()
        {
            foreach (Parte p in Corpo)
            {
                p.Update();
                if (Tela.GM.MouseManager.MouseEmCima(p))
                {
                    if (Corpo[1].Clicavel)
                    {
                        // Lida com as partes contidas na cabeça separadamente
                        if ((p.Tipo == "mouth") || (p.Tipo == "nose") || (p.Tipo == "eyes"))
                        {
                            if (p.Clicavel)
                            {
                                Corpo[1].IsFocada = false;
                                p.IsFocada = true;
                                ParteFocada = p;
                                DesabilitarParte("head");
                            }
                        }
                        else
                        {
                            if (Corpo[1].Clicavel == false)
                            {
                                HabilitarParte("head");
                            }
                        }
                    }
                }
                else
                {
                    if (p.IsFocada)
                    {
                        p.IsFocada = false;
                        ParteFocada = null;                    
                    }
                }
            }

            
        }

        /// <summary>
        /// Desenha o Garoto.
        /// </summary>
        /// <param name="SB"></param>
        public void Draw(SpriteBatch SB)
        {
            foreach (Parte p in Corpo)
            {
                p.Draw(SB);
               
            }
   
           
        }

        /// <summary>
        /// Muda a imagem de uma única parte escolhida.
        /// </summary>
        /// <param name="tipo">O tipo da parte cuja imagem será alterada.</param>
        /// <param name="numeroImagem">O número do conjunto ao qual pertence a nova imagem.</param>
        public void LoadParte(string tipo,string numeroImagem)
        {
           
       GetParte(tipo).LoadContent(numeroImagem);
        }

        /// <summary>
        /// Carrega todas as imagens de um conjunto.
        /// </summary>
        /// <param name="numeroImagem">O número do conjunto.</param>
        public void LoadCorpo(string numeroImagem)
        {
            foreach (Parte p in Corpo)
            {
                p.LoadContent(numeroImagem);
            }
            MontarCorpo(Tela.Centro);
        }

        /// <summary>
        /// Estrutura o corpo de forma humanóide.
        /// </summary>
        /// <param name="centroTronco">A posição do centro do tronco.</param>
        public void MontarCorpo(Vector2 centroTronco)
        {
            // Posiciona o tronco.
            corpo[0].Centro = centroTronco; 
            
            // Posiciona a cabeça.
            corpo[1].Centro = new Vector2(corpo[0].Centro.X, corpo[0].Position.Y-(corpo[1].Tamanho.Y/2) );
            
            // Posiciona o braço esquerdo.
            corpo[2].Position = new Vector2(corpo[0].Retangulo.Right, corpo[0].Position.Y);
           
            // Posiciona o braço direito.
            corpo[3].Position = new Vector2(corpo[0].Position.X - corpo[3].Tamanho.X, corpo[0].Position.Y);
          
            // Posiciona a perna esquerda.
            corpo[4].Position = new Vector2(corpo[0].Centro.X, corpo[0].Retangulo.Bottom);
           
            // Posiciona a perna direita.
            corpo[5].Position = new Vector2(corpo[0].Centro.X-corpo[5].Tamanho.X, corpo[0].Retangulo.Bottom);
           
            // Posiciona a mão esquerda.
            corpo[6].Position = new Vector2(corpo[2].Retangulo.Right-corpo[6].Tamanho.X, corpo[2].Retangulo.Bottom);
            
            // Posiciona a mão direita.
            corpo[7].Position = new Vector2(corpo[3].Position.X, corpo[3].Retangulo.Bottom);
           
            // Posiciona o pé esquerdo.
            corpo[8].Position = new Vector2(corpo[4].Position.X, corpo[4].Retangulo.Bottom);

            // Posiciona o pé direito.
            corpo[9].Position = new Vector2(corpo[5].Retangulo.Right - corpo[9].Tamanho.X, corpo[5].Retangulo.Bottom);

            // Posiciona os olhos.
            corpo[10].Centro = new Vector2(corpo[1].Centro.X, corpo[1].Centro.Y - corpo[10].Tamanho.Y);

            // Posiciona o nariz.
            corpo[11].Centro = corpo[1].Centro;

            // Posiciona a boca.
            corpo[12].Centro = new Vector2(corpo[1].Centro.X, corpo[11].Retangulo.Bottom);

            // Posiciona a orelha esquerda.
            corpo[13].Position = new Vector2(corpo[1].Retangulo.Right, corpo[1].Centro.Y - corpo[13].Tamanho.Y);

            // Posiciona a orelha direita.
            corpo[14].Position = new Vector2(corpo[1].Position.X - corpo[14].Tamanho.X, corpo[1].Centro.Y - corpo[14].Tamanho.Y);
        }

       /// <summary>
        /// Permite que uma parte específica seja clicada.
       /// </summary>
       /// <param name="tipo">O nome da parte que será habilitada.</param>
        public void HabilitarParte(string tipo)
        {
            foreach (Parte p in Corpo)
            {
                if (p.Tipo == tipo)
                {
                    p.Clicavel = true;
                }
            }
        }

        /// <summary>
        /// Torna uma parte específica não-clicável.
        /// </summary>
        /// <param name="tipo">O nome da parte que será desabilitada.</param>
        public void DesabilitarParte(string tipo)
        {
            foreach (Parte p in Corpo)
            {
                if (p.Tipo == tipo)
                {
                    p.Clicavel =false;
                }
            }
        }

        /// <summary>
        /// Torna todas as partes do corpo clicáveis.
        /// </summary>
        public void HabilitarCorpo()
        {
            foreach (Parte p in Corpo)
            {
                p.Clicavel = true;
            }
        }

        /// <summary>
        /// Torna todas as partes do corpo não-clicáveis.
        /// </summary>
        public void DesabilitarCorpo()
        {
            foreach (Parte p in Corpo)
            {
                p.Clicavel = false;
            }
        }

        public Parte GetParte(string tipo)
        {
            foreach (Parte p in Corpo)
            {
                if (p.Tipo == tipo)
                {

                    return p;
                }
            }
            return null;
        }
      
        #endregion
    }
}
