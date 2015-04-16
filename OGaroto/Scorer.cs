using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
namespace OGaroto
{
    /// <summary>
    /// Representa um objeto que computa dados sobre a partida.
    /// </summary>
    class Scorer
    {
        #region Propriedades e accessors
        private Fase fase;
        /// <summary>
        /// A fase atual.
        /// </summary>
        public Fase Fase
        {
            set { fase = value; }
            get { return fase; }
        }

        private int placar;
        /// <summary>
        /// A pontuação do jogador.
        /// </summary>
        public int Placar
        {
            set { placar = value; }
            get { return placar; }
    }
      
         private int multiplicador;
        /// <summary>
        /// Multiplica cada ponto ganho de acordo com a sequência atual de Aminos.
        /// </summary>
        public int Multiplicador
        {
            set { multiplicador = value; }
            get { return multiplicador; }
    }

        private int sequenciaAminos=0;
        /// <summary>
        /// Determina o número de Aminos atualmente ligados em sequência.
        /// </summary>
        public int SequenciaAminos
        {
            set { sequenciaAminos = value; }
            get { return sequenciaAminos; }
        }

        private int totalAminos;
        /// <summary>
        /// Determina o número de Aminos na proteína.
        /// </summary>
        public int TotalAminos
        {
            set {totalAminos=value; }
            get { return totalAminos; }
        }

 
        /// <summary>
        /// Obtém uma letra representando a pontuação do jogador, baseando-se na porcentagem de acerto.
        /// </summary>
        public char Rank
        {
            get {
                if (PorcentagemAcerto == 0)
                { return 'Z'; }
                else if (PorcentagemAcerto < 30)
                { return 'E'; }
                else if (PorcentagemAcerto < 50)
                { return 'D'; }
                else if (PorcentagemAcerto < 70)
                { return 'C'; }
                else if (PorcentagemAcerto < 90)
                { return 'B'; }
                else if (PorcentagemAcerto < 100)
                { return 'A'; }
                else if (PorcentagemAcerto == 100)
                { return 'S'; }
                else
                    return '?';
            }
        }

        private int maiorSequencia;
        /// <summary>
        /// Determina a maior sequência de Aminos obtida até o momento.
        /// </summary>
        public int MaiorSequencia
        {
            set { maiorSequencia = value; }
            get { return maiorSequencia; }
        }

        /// <summary>
        /// Determina a porcentagem de acerto do jogador.
        /// </summary>
        public float PorcentagemAcerto
        {
            get
            {
                float a=(float)(TotalAminos+1) / (float)Fase.Codons.Count;
                return a*100; }
        }
        #endregion

        #region Construtor
        /// <param name="aFase">A fase atual.</param>
        public Scorer(Fase aFase)
        {
            Fase=aFase;
            placar = 0;
            multiplicador = 1;
            sequenciaAminos = 0;
        }
        #endregion

        #region Métodos
        /// <summary>
        /// Adiciona pontos de acordo com o valor de um objeto e com o multiplicador.
        /// </summary>
        /// <param name="o">O objeto a ser pontuado.</param>
        public void Pontuar(ObjetoMovel o)
        {
        placar+=o.Valor*multiplicador;
        }

        /// <summary>
        /// Adiciona pontos de acordo com um valor inteiro escolhido e com o multiplicador.
        /// </summary>
        /// <param name="valor"></param>
        public void Pontuar(int valor)
        {
            placar += valor * multiplicador;
        }

      
        /// <summary>
     /// Atualiza o Scorer.
     /// </summary>
        public void Update()
        {
            // A cada 5 Aminos em sequência, aumenta em 1 o multiplicador.
            if ((sequenciaAminos % 5) == 0)
            {
                Multiplicador = 1+(SequenciaAminos / 5);
            }

            // Se a sequência atual for maior que a última maior sequência, atualiza a maior sequência.
            if (SequenciaAminos > MaiorSequencia)
            { MaiorSequencia = SequenciaAminos; }

            
        }

        
        /// <summary>
        /// Desenha dados do jogo.
        /// </summary>
        /// <param name="position">A posição do texto.</param>
        /// <param name="escala">A escala de tamanho do texto.</param>
        /// <param name="texto">O texto.</param>
        public void Draw(SpriteBatch SB,Vector2 position,float escala,string texto)
        {
            SB.DrawString(Fase.GM.Fonte, texto, position, Color.Maroon,0,Vector2.Zero,escala,SpriteEffects.None,0);
        }

        /// <summary>
        ///  Desenha dados do jogo.
        /// </summary>
        /// <param name="position">A posição do texto.</param>
        /// <param name="escala">A escala de tamanho do texto.</param>
        /// <param name="texto">O texto.</param>
        /// <param name="cor">A cor do texto a ser escrito.</param>
         public void Draw(SpriteBatch SB,Vector2 position,float escala,string texto,Color cor)
        {
            SB.DrawString(Fase.GM.Fonte, texto, position, cor, 0, Vector2.Zero, escala, SpriteEffects.None, 0);
        }
        #endregion


    }
}
