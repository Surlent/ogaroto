using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace OGaroto
{
    /// <summary>
    /// Controla o desenrolar do jogo.
    /// </summary>
    class TimeLine
    {
        #region Propriedades e accessors.
        /// <summary>
        /// O limite horizontal da tela, em pixels.
        /// </summary>
        public float LimiteX
        {
            get { return Fase.Tamanho.X; }
        }

        /// <summary>
        /// O limite vertical da tela, em pixels.
        /// </summary>
        public float LimiteY
        {
            get { return Fase.Tamanho.Y; }
        }

        bool[] eventos = new bool[110];
        /// <summary>
        /// Determina se o evento de determinado índice já aconteceu.
        /// </summary>
        public bool[] Eventos
        {
            set { eventos = value; }
        get{return eventos;}
        
        }

        private Fase fase;
        /// <summary>
        /// A fase atual.
        /// </summary>
        public Fase Fase
        {
            set { fase = value; }
            get { return fase; }
        }

        private int numCodons=0;
        /// <summary>
        /// O número de codons em jogo.
        /// </summary>
        public int NumCodons
        {
            set { numCodons = value; }
            get { return numCodons; }
        }

        private bool encerrada=false;
        /// <summary>
        /// Determina se a fase acabou.
        /// </summary>
        public bool Encerrada
        {
            set { encerrada = value; }
            get { return encerrada; }
        }
        #endregion

        #region Construtor
        /// <param name="aFase">A fase atual.</param>
        public TimeLine(Fase aFase)
        {
            Fase = aFase;
        }
        #endregion

        #region Métodos

       /// <summary>
        /// Determina os eventos que acontecerão em cada fase.
       /// </summary>
        /// <param name="oNivel">A fase a ser jogada, correspondente a uma parte do Garoto.</param>
        public void Evento(string oNivel)
        {
            switch (oNivel)
            {
                #region Fase 1 : Pés
                case "leftFoot":
                case "rightFoot":
                    {
                        {
                            switch (NumCodons)
                            {
                                case 1:
                                    {
                                        if (Eventos[1] == false)
                                        {
                                            Fase.Acelerador -= 0.45f;
                                            Fase.Messenger.AdicionarMensagem("É hora de iniciar a síntese protéica.", 4000f, 1.0f);
                                            Eventos[1] = true;
                                        }
                                    }
                                    break;

                                case 3:
                                    {
                                        if (Eventos[3] == false)
                                        {
                                            Fase.Messenger.AdicionarMensagem("A barra acima é a barra de Aminos. Os Aminos são os\n componentes da proteína que será formada.", 5000f, 1.0f);
                                            Eventos[3] = true;
                                        }
                                    }
                                    break;

                                case 5:
                                    {
                                        if (Eventos[5] == false)
                                        {

                                            Fase.Messenger.AdicionarMensagem("Abaixo estão os Codons. Observe\nque eles têm formato compatível\ncom os Aminos.", 5000f, 1.0f);
                                            Eventos[5] = true;
                                        }
                                    }
                                    break;

                                case 7:
                                    {
                                        if (Eventos[7] == false)
                                        {

                                            if ((!(Fase.Travada)) && (Fase.AminoEsperando == null))
                                            {
                                                Fase.Messenger.AdicionarMensagem("Agora tente encaixá-los. Clique em \num Amino e arraste-o até um Codon complementar.", 20000f, 1.0f, Color.Red);
                                                Fase.Travar();
                                                Fase.BarraAminos.Ativar();
                                            }
                                            Eventos[7] = true;
                                        }

                                        if (Eventos[4] == false)
                                        {
                                            if (Fase.AminoEsperando != null)
                                            {
                                                Fase.Messenger.AdicionarMensagem("Perfeito.", 5000f, 1.0f);
                                                Fase.Destravar();
                                                Eventos[4] = true;
                                            }
                                        }
                                    }
                                    break;

                                case 8:
                                    {
                                        if (Eventos[8] == false)
                                        {
                                            if (Fase.Proteina.ContAmino == 0)
                                            {
                                                Fase.Messenger.AdicionarMensagem("Agora encaixe um Amino no Codon imediatamente \nà direita do encaixado.", 20000f, 1.0f, Color.Red);
                                                Fase.Travar();
                                                Fase.BarraAminos.Ativar();
                                            }
                                            Eventos[8] = true;
                                        }

                                        if (Eventos[9] == false)
                                        {
                                            if (Fase.Proteina.ContAmino > 0)
                                            {
                                                Fase.Messenger.AdicionarMensagem("Excelente. Você acaba de ligar os Aminos.", 3000f, 1f);
                                                Fase.Destravar();
                                                Eventos[9] = true;
                                            }
                                        }
                                    }
                                    break;

                                case 10:
                                    {
                                        if (Eventos[10] == false)
                                        {
                                            Fase.Messenger.AdicionarMensagem("Tome cuidado para que o Amino encaixado\n não saia da tela. Caso isso \n aconteça, a sequência será quebrada e\na proteína será prejudicada.", 5000f, 1.0f);
                                            Eventos[10] = true;
                                        }

                                    }
                                    break;

                                case 12:
                                    {
                                        if (Eventos[12] == false)
                                        {
                                            try
                                            {
                                                if (Fase.AminoEsperando.CodonEncaixado != Fase.Codons[Fase.Codons.Count - 2])
                                                {
                                                    Fase.Messenger.AdicionarMensagem("Continue encaixando Aminos nos Codons adjacentes\npara construir a proteína.", 50000f, 1.0f, Color.Red);
                                                    Fase.Travar();
                                                    Fase.BarraAminos.Ativar();
                                                }
                                            }
                                            catch (Exception ex)
                                            { }
                                            Eventos[12] = true;
                                        }

                                        if (Eventos[13] == false)
                                        {
                                            try
                                            {
                                                if (Fase.AminoEsperando.CodonEncaixado == Fase.Codons[Fase.Codons.Count - 2])
                                                {
                                                    Fase.Messenger.AdicionarMensagem("Muito bom. Continue assim.", 3000f, 1f);
                                                    Fase.Destravar();
                                                    Eventos[13] = true;
                                                }
                                            }
                                            catch (Exception ex)
                                            { }
                                        }
                                    }
                                    break;

                                case 14:
                                    {
                                        if (Eventos[14] == false)
                                        {
                                            Fase.Messenger.AdicionarMensagem("Cada ligação entre Aminos fornece pontos.\n\n O placar está marcado no canto\n superior esquerdo da tela.", 6000f, 1.0f);
                                            Eventos[14] = true;
                                        }
                                    }
                                    break;

                                case 17:
                                    {
                                        if (Eventos[17] == false)
                                        {
                                            Fase.Messenger.AdicionarMensagem("Quanto mais Aminos forem encaixados \n em sequência, mais pontos cada um dará. \n Portanto, procure sempre manter a sequência.", 6000f, 1.0f);
                                            Eventos[17] = true;
                                        }
                                    }
                                    break;

                                case 19:
                                    {
                                        if (Eventos[19] == false)
                                        {
                                            Fase.Messenger.AdicionarMensagem("A sequência está marcada no\ncanto superior direito da tela.", 6000f, 1.0f);
                                            Eventos[19] = true;
                                        }
                                    }
                                    break;

                                case 25:
                                    {
                                        if (Eventos[25] == false)
                                        {
                                            Fase.Messenger.AdicionarMensagem("Lute sempre para que a sequência\nnão seja quebrada", 6000f, 1.0f);
                                            Eventos[25] = true;
                                        }
                                    }
                                    break;

                                case 28:
                                    {
                                        if (Eventos[28] == false)
                                        {
                                            Fase.Acelerador += 0.45f;
                                            Fase.Messenger.AdicionarMensagem("Acabou o mole, vamos agora\nao mundo real!", 6000f, 1.0f);
                                            Eventos[28] = true;
                                        }
                                    }
                                    break;

                                case 32:
                                    {
                                        if (Eventos[32] == false)
                                        {
                                            Fase.Messenger.AdicionarMensagem("Quanto menos for interrompida a síntese\nprotéica,mais pura será a proteína.\nIsto afetará diretamente seu desempenho...\ne sua recompensa.", 6000f, 1.0f);
                                            Eventos[32] = true;
                                        }
                                    }
                                    break;

                                case 38:
                                    {
                                        if (Eventos[38] == false)
                                        {
                                            Fase.Messenger.AdicionarMensagem("Eis a reta final da síntese protéica...\nVocê logo saberá seu resultado.", 15000f, 1.0f);
                                            Fase.GM.TelaGaroto.Boy.LoadParte("leftFoot", "1");
                                            Fase.GM.TelaGaroto.Boy.LoadParte("rightFoot", "1");
                                            Fase.GM.TelaGaroto.Boy.HabilitarParte("leftLeg");
                                            Fase.GM.TelaGaroto.Boy.HabilitarParte("rightLeg");
                                            Encerrada = true;
                                            Eventos[38] = true;
                                        }
                                    }
                                    break;
                            }
                        }
                    } break;

                #endregion
                #region Fase 2 : Pernas
                case "leftLeg":
                case "rightLeg":
                    {
                        switch (NumCodons)
                        {

                            case 2:
                                {
                                    if (Eventos[2] == false)
                                    {
                                        Eventos[2] = true;
                                    }
                                } break;
                            case 7:
                                {
                                    if (Eventos[7] == false)
                                    {
                                        /**
                                        Fase.GM.TelaGaroto.Boy.LoadParte("trunk", "1");
                                        Fase.GM.TelaGaroto.Boy.HabilitarParte("trunk");
                                        Encerrada = true;
                                        */
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 250), 0.25f, 0.8f));
                                        Eventos[7] = true;
                                    }
                                } break;

                            case 17:
                                {
                                    if (Eventos[17] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 300), 0.3f, 1.2f));
                                        Eventos[17] = true;
                                    }
                                }
                                break;

                            case 20:
                                {
                                    if (Eventos[20] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.0f));
                                        Eventos[20] = true;
                                    }
                                } break;
                            case 30:
                                {
                                    if (Eventos[30] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 270), 0.25f, 0.8f));
                                        Eventos[30] = true;
                                    }
                                } break;
                            case 45:
                                {
                                    if (Eventos[45] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.2f));
                                        Eventos[45] = true;
                                    }
                                } break;
                            case 55:
                                {
                                    if (Eventos[55] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 240), 0.3f, 1f));
                                        Eventos[55] = true;
                                    }

                                } break;

                            case 60:
                                {
                                    if (Eventos[60] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 280), 0.5f, 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), 0.5f));
                                        Eventos[60] = true;
                                    }
                                } break;

                            case 70:
                                {
                                    if (Eventos[70] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 350), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 500), 1.0f));
                                        Eventos[70] = true;
                                    }
                                } break;
                            case 80:
                                {
                                    if (Eventos[80] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 280), 0.4f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 440), 0.4f));
                                        Fase.BarraInterf.AtivarBotao("FeixeATP");
                                        Fase.Messenger.AdicionarMensagem("Desenvolvemos uma nova arma\ncontra os obstáculos. Teste\nna barra à esquerda.", 10000f, 1f);



                                        Eventos[80] = true;
                                    }
                                } break;
                            case 82:
                                {
                                    if (Eventos[82] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 350), 0.4f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), 0.4f));
                                        Eventos[82] = true;
                                    }
                                } break;
                            case 84:
                                {
                                    if (Eventos[84] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 250), 0.4f));

                                        Eventos[84] = true;
                                    }
                                } break;

                            case 90:
                                {
                                    if (Eventos[90] == false)
                                    {
                                        Fase.GM.TelaGaroto.Boy.LoadParte("leftLeg", "1");
                                        Fase.GM.TelaGaroto.Boy.LoadParte("rightLeg", "1");
                                        Fase.GM.TelaGaroto.Boy.HabilitarParte("trunk");
                                        Encerrada = true;
                                    }
                                    Eventos[90] = true;
                                } break;
                            default: { } break;

                        }
                    } break;

                #endregion
                #region Fase 3 : Tronco
                case "trunk":
                    {

                        switch (NumCodons)
                        {
                            case 1:
                                {
                                    if (Eventos[1] == false)
                                    {
                                        Fase.BarraInterf.AtivarBotao("FeixeATP");
                                    }
                                } break;

                            case 5:
                                {
                                    if (Eventos[5] == false)
                                    {
                                        //5 especiais
                                       // Fase.Messenger.AdicionarMensagem("Estes códons são especiais.\nAcertar uma sequência inteira destes\nhabilitará um modo que\ndeixará o jogo um pouco mais fácil.", 10000f, 1.0f);
                                        Eventos[5] = true;
                                    }
                                } break;
                            case 10:
                                {
                                    if (Eventos[10] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 300), 1.2f, 1.3f));

                                        Eventos[10] = true;
                                    }
                                } break;
                            case 15:
                                {
                                    if (Eventos[15] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 500), 0.6f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 250), 0.9f));
                                        Eventos[15] = true;
                                    }
                                } break;
                            case 17:
                                {
                                    if (Eventos[17] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 350), new Vector2(0, 250), 1.0f));
                                        Eventos[17] = true;
                                    }
                                } break;
                            case 18:
                                {
                                    if (Eventos[18] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 0.7f));
                                        Eventos[18] = true;
                                    }
                                } break;
                            case 20:
                                {
                                    if (Eventos[20] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), new Vector2(0, 300), 0.8f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.1f));
                                        Eventos[20] = true;
                                    }
                                } break;
                            case 29:
                                {
                                    if (Eventos[29] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 100), 1.0f));
                                        Eventos[29] = true;
                                    }
                                } break;
                            case 30:
                                {
                                    if (Eventos[30] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 250), 0.7f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), 1.2f));
                                        Eventos[30] = true;
                                    }
                                } break;
                            case 32:
                                {
                                    if (Eventos[32] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 100), new Vector2(0, 300), 1.3f));
                                        Eventos[32] = true;
                                    }
                                } break;
                            //3 especiais em 40
                            case 45:
                                {
                                    if (Eventos[45] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 200), 0.9f, 1.0f));

                                        Eventos[45] = true;
                                    }
                                } break;
                            case 54:
                                {
                                    if (Eventos[54] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 300), 1.1f));
                                        Eventos[54] = true;
                                    }
                                } break;
                            case 55:
                                {
                                    if (Eventos[55] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), 1.0f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), 1.0f));
                                        Eventos[55] = true;
                                    }
                                } break;
                            case 59:
                                {
                                    if (Eventos[59] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 0.8f));
                                        Eventos[59] = true;
                                    }
                                } break;
                            case 60:
                                {
                                    if (Eventos[60] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 100), new Vector2(0, 500), 0.95f));
                                        Eventos[60] = true;
                                    }
                                } break;
                            case 61:
                                {
                                    if (Eventos[61] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), new Vector2(0, 300), 1.3f));
                                        Eventos[61] = true;
                                    }
                                } break;
                            //5 especiais em 70
                            case 75:
                                {
                                    if (Eventos[75] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 400), new Vector2(0, 200), 1.3f, 1.3f));

                                        Eventos[75] = true;
                                    }
                                } break;
                            case 83:
                                {
                                    if (Eventos[83] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 0.8f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 200), 0.9f));
                                        Eventos[83] = true;
                                    }
                                } break;
                            case 85:
                                {
                                    if (Eventos[85] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.1f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 100), 1.2f));
                                        Eventos[85] = true;
                                    }
                                } break;
                            case 86:
                                {
                                    if (Eventos[86] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 300), 1.6f));
                                        Eventos[86] = true;
                                    }
                                } break;
                            case 89:
                                {
                                    if (Eventos[89] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 450), 1.2f));
                                        Eventos[89] = true;
                                    }
                                } break;
                            case 90:
                                {
                                    if (Eventos[90] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 300), 1.0f));
                                        Eventos[90] = true;
                                    }
                                } break;
                            case 91:
                                {
                                    if (Eventos[91] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 0.9f));
                                        Eventos[91] = true;
                                    }
                                } break;
                            case 93:
                                {
                                    if (Eventos[93] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), 2.0f));
                                        Eventos[93] = true;
                                    }
                                } break;
                            case 95:
                                {
                                    if (Eventos[95] == false)
                                    {

                                        Fase.GM.TelaGaroto.Boy.LoadParte("trunk", "1");
                                        Fase.GM.TelaGaroto.Boy.HabilitarParte("leftArm");
                                        Fase.GM.TelaGaroto.Boy.HabilitarParte("rightArm");
                                        Encerrada = true;

                                        Eventos[95] = true;
                                    }
                                } break;
                            //default: ( ) break;

                        }

                    } break;
                #endregion
                #region Fase 4 : Braços
                case "leftArm":
                case "rightArm":
                    {
                        switch (NumCodons)
                        {
                            case 1:
                                {
                                    if (Eventos[1] == false)
                                    {
                                        Fase.BarraInterf.AtivarBotao("FeixeATP");
                                    }
                                } break;
                            //10 especiais em 5
                            case 9:
                                {
                                    if (Eventos[9] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.2f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 200), 1.0f));
                                        Eventos[9] = true;
                                    }
                                } break;
                            case 10:
                                {
                                    if (Eventos[10] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 150), 0.9f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.2f));
                                        Eventos[10] = true;
                                    }
                                } break;
                            case 13:
                                {
                                    if (Eventos[13] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.1f));
                                        Eventos[13] = true;
                                    }
                                } break;
                            case 20:
                                {
                                    if (Eventos[20] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 400), new Vector2(0, 300), 0.8f, 1.0f));

                                        Eventos[20] = true;
                                    }
                                } break;
                            case 26:
                                {
                                    if (Eventos[26] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), 1.2f));
                                        Eventos[26] = true;
                                    }
                                } break;
                            case 25:
                                {
                                    if (Eventos[10] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 150), new Vector2(0, 400), 1.1f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 350), 1.1f));
                                        Eventos[25] = true;
                                    }
                                } break;
                            case 23:
                                {
                                    if (Eventos[23] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), 1.0f));
                                        Eventos[23] = true;
                                    }
                                } break;
                            //5 especiais em 35
                            case 42:
                                {
                                    if (Eventos[42] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.2f));
                                        Eventos[42] = true;
                                    }
                                } break;
                            case 44:
                                {
                                    if (Eventos[44] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 200), 1.0f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), 0.8f));
                                        Eventos[44] = true;
                                    }
                                } break;
                            case 45:
                                {
                                    if (Eventos[45] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 500), new Vector2(0, 200), 0.9f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 150), 1.3f));
                                        Eventos[45] = true;
                                    }
                                } break;
                            case 47:
                                {
                                    if (Eventos[47] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.0f));
                                        Eventos[47] = true;
                                    }
                                } break;

                            case 55:
                                {
                                    if (Eventos[55] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Fase.Messenger.AdicionarMensagem("Eis um Parasita.", 100000f, 1.0f);
                                        Eventos[55] = true;
                                    }
                                } break;
                            case 60:
                                {
                                    if (Eventos[60] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.0f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 150), 1.0f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.0f));
                                        Eventos[60] = true;
                                    }
                                } break;
                            //5 especiais em 65

                            case 69:
                                {
                                    if (Eventos[69] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[69] = true;
                                    }
                                } break;

                            case 70:
                                {
                                    if (Eventos[70] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Fase.Messenger.AdicionarMensagem("Para curar seus códon de parasitas, use o Feixe de ATP.", 100000f, 1.0f);
                                        Eventos[70] = true;
                                    }
                                } break;
                            case 75:
                                {
                                    if (Eventos[75] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 200), new Vector2(0, 350), 1.8f, 0.8f));

                                        Eventos[75] = true;
                                    }
                                } break;
                            case 85:
                                {
                                    if (Eventos[85] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[85] = true;
                                    }
                                } break;
                            case 86:
                                {
                                    if (Eventos[86] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[86] = true;
                                    }
                                } break;
                            case 90:
                                {
                                    if (Eventos[90] == false)
                                    {
                                        Fase.GM.TelaGaroto.Boy.LoadParte("leftArm", "1");
                                        Fase.GM.TelaGaroto.Boy.LoadParte("rightArm", "1");
                                        Fase.GM.TelaGaroto.Boy.HabilitarParte("leftHand");
                                        Fase.GM.TelaGaroto.Boy.HabilitarParte("rightHand");
                                        Encerrada = true;
                                        Eventos[90] = true;
                                    }
                                }
                                break;

                        }

                    } break;
                #endregion
                #region Fase 5 : Mãos
                case "leftHand":
                case "rightHand":
                    {
                        switch (NumCodons)
                        {
                            case 1:
                                {
                                    if (Eventos[1] == false)
                                    {
                                        Fase.BarraInterf.AtivarBotao("FeixeATP");
                                    }
                                } break;
                            //5 especiais em 5
                            case 8:
                                {
                                    if (Eventos[8] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.3f));
                                        Eventos[8] = true;
                                    }
                                } break;
                            case 10:
                                {
                                    if (Eventos[10] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 150), 1.4f));
                                        Eventos[10] = true;
                                    }
                                } break;
                            case 11:
                                {
                                    if (Eventos[11] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 350), 1.2f));
                                        Eventos[11] = true;
                                    }
                                } break;
                            case 13:
                                {
                                    if (Eventos[13] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.5f));
                                        Eventos[13] = true;
                                    }
                                } break;
                            case 15:
                                {
                                    if (Eventos[15] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 400), 1.2f));
                                        Eventos[15] = true;
                                    }
                                } break;
                            case 16:
                                {
                                    if (Eventos[16] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[16] = true;
                                    }
                                } break;
                            //5 especiais em 20
                            case 28:
                                {
                                    if (Eventos[28] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.3f));
                                        Eventos[28] = true;
                                    }
                                } break;
                            case 30:
                                {
                                    if (Eventos[30] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 150), 1.2f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), 1.2f));
                                        Eventos[30] = true;
                                    }
                                } break;
                            case 32:
                                {
                                    if (Eventos[32] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 150), 1.0f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.0f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), 1.0f));
                                        Eventos[32] = true;
                                    }
                                } break;
                            case 34:
                                {
                                    if (Eventos[34] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[34] = true;
                                    }
                                } break;
                            case 35:
                                {
                                    if (Eventos[35] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[35] = true;
                                    }
                                } break;
                            case 37:
                                {
                                    if (Eventos[37] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[37] = true;
                                    }
                                } break;
                            case 40:
                                {
                                    if (Eventos[40] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 150), 1.2f));
                                        Eventos[40] = true;
                                    }
                                } break;
                            case 48:
                                {
                                    if (Eventos[48] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.3f));
                                        Eventos[48] = true;
                                    }
                                } break;
                            case 50:
                                {
                                    if (Eventos[50] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(LimiteX, 400), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(LimiteX, 150), 1.4f));
                                        Eventos[50] = true;
                                    }
                                } break;
                            case 51:
                                {
                                    if (Eventos[51] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 350), 1.2f));
                                        Eventos[51] = true;
                                    }
                                } break;
                            case 53:
                                {
                                    if (Eventos[53] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.5f));
                                        Eventos[53] = true;
                                    }
                                } break;
                            //3 especiais em 55
                            case 60:
                                {
                                    if (Eventos[60] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[60] = true;
                                    }
                                } break;
                            case 63:
                                {
                                    if (Eventos[63] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.3f));
                                        Eventos[63] = true;
                                    }
                                } break;
                            case 64:
                                {
                                    if (Eventos[64] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(LimiteX, 400), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(LimiteX, 150), 1.4f));
                                        Eventos[64] = true;
                                    }
                                } break;
                            case 65:
                                {
                                    if (Eventos[65] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[65] = true;
                                    }
                                } break;
                            case 66:
                                {
                                    if (Eventos[66] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 350), 1.2f));
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 150), 1.0f));
                                        Eventos[66] = true;
                                    }
                                } break;
                            case 68:
                                {
                                    if (Eventos[68] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[68] = true;
                                    }
                                } break;
                            case 74:
                                {
                                    if (Eventos[74] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.1f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 200), 1.3f));
                                        Eventos[74] = true;
                                    }
                                } break;
                            case 76:
                                {
                                    if (Eventos[76] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.1f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 100), 1.2f));
                                        Eventos[76] = true;
                                    }
                                } break;
                            case 78:
                                {
                                    if (Eventos[78] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 300), 1.9f));
                                        Eventos[78] = true;
                                    }
                                } break;
                            case 85:
                                {
                                    if (Eventos[85] == false)
                                    {
                                        Fase.GM.TelaGaroto.Boy.LoadParte("leftHand", "1");
                                        Fase.GM.TelaGaroto.Boy.LoadParte("rightHand", "1");
                                        Fase.GM.TelaGaroto.Boy.HabilitarParte("head");
                                        Encerrada = true;
                                        Eventos[85] = true;
                                    }
                                }
                                break;

                        }

                    } break;
                #endregion
                #region Fase 6 : Cabeça
                case "head":
                    {
                        switch (NumCodons)
                        {
                            case 1:
                                {
                                    if (Eventos[1] == false)
                                    {
                                        Fase.Messenger.AdicionarMensagem("Foi desenvolvida uma nova proteção contra parasitas.\nÉ o lissossomo, e você já pode selecioná-lo\nna barra à direita.", 100000f, 1.0f);
                                        Fase.BarraInterf.AtivarBotao("FeixeATP");
                                        Fase.BarraInterf.AtivarBotao("Lisossomo");
                                        Eventos[1] = true;
                                    }
                                } break;

                            //10 especiais em 5
                            case 14:
                                {
                                    if (Eventos[14] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[14] = true;
                                    }
                                } break;
                            case 15:
                                {
                                    if (Eventos[15] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[15] = true;
                                    }
                                } break;
                            case 16:
                                {
                                    if (Eventos[16] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[16] = true;
                                    }
                                } break;
                            case 20:
                                {
                                    if (Eventos[20] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 150), 1.2f));
                                        Eventos[20] = true;
                                    }
                                } break;
                            case 28:
                                {
                                    if (Eventos[28] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.2f));
                                        Eventos[28] = true;
                                    }
                                } break;
                            case 29:
                                {
                                    if (Eventos[29] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.2f));
                                        Eventos[29] = true;
                                    }
                                } break;
                            case 30:
                                {
                                    if (Eventos[30] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), 1.4f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 150), 1.4f));
                                        Eventos[30] = true;
                                    }
                                } break;
                            case 32:
                                {
                                    if (Eventos[32] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 100), 1.0f));
                                        Eventos[32] = true;
                                    }
                                } break;
                            case 35:
                                {
                                    if (Eventos[35] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[35] = true;
                                    }
                                } break;
                            case 37:
                                {
                                    if (Eventos[37] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[37] = true;
                                    }
                                } break;
                            case 45:
                                {
                                    if (Eventos[45] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 300), 1.2f));
                                        Eventos[45] = true;
                                    }
                                } break;
                            case 48:
                                {
                                    if (Eventos[48] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.3f));
                                        Eventos[48] = true;
                                    }
                                } break;
                            case 50:
                                {
                                    if (Eventos[50] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 150), 1.4f));
                                        Eventos[50] = true;
                                    }
                                } break;
                            case 51:
                                {
                                    if (Eventos[51] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 350), 1.2f));
                                        Eventos[51] = true;
                                    }
                                } break;
                            case 53:
                                {
                                    if (Eventos[53] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.5f));
                                        Eventos[53] = true;
                                    }
                                } break;
                            //3 especiais em 55
                            case 54:
                                {
                                    if (Eventos[54] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[54] = true;
                                    }
                                } break;
                            case 56:
                                {
                                    if (Eventos[56] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[56] = true;
                                    }
                                } break;
                            case 58:
                                {
                                    if (Eventos[58] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[58] = true;
                                    }
                                } break;
                            case 64:
                                {
                                    if (Eventos[64] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 300), 1.1f));
                                        Eventos[64] = true;
                                    }
                                } break;
                            case 65:
                                {
                                    if (Eventos[65] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), 1.0f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), 1.0f));
                                        Eventos[65] = true;
                                    }
                                } break;
                            case 73:
                                {
                                    if (Eventos[73] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 200), 1.4f));
                                        Eventos[73] = true;
                                    }
                                } break;
                            case 75:
                                {
                                    if (Eventos[75] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.1f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 200), 1.1f));
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 300), 1.1f));
                                        Eventos[75] = true;
                                    }
                                } break;
                            case 74:
                                {
                                    if (Eventos[74] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 300), 1.4f));
                                        Eventos[74] = true;
                                    }
                                } break;
                            case 77:
                                {
                                    if (Eventos[77] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.6f));
                                        Eventos[77] = true;
                                    }
                                } break;
                            case 83:
                                {
                                    if (Eventos[83] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[83] = true;
                                    }
                                } break;
                            case 90:
                                {
                                    if (Eventos[90] == false)
                                    {
                                        Fase.GM.TelaGaroto.Boy.LoadParte("head", "1");
                                        Fase.GM.TelaGaroto.Boy.HabilitarParte("leftEar");
                                        Fase.GM.TelaGaroto.Boy.HabilitarParte("rightEar");
                                        Encerrada = true;
                                        Eventos[90] = true;
                                    }
                                }
                                break;

                        }

                    } break;
                #endregion
                #region Fase 7 : Orelhas
                case "leftEar":
                case "rightEar":
                    {
                        switch (NumCodons)
                        {
                            case 1:
                                {
                                    if (Eventos[1] == false)
                                    {
                                        Fase.BarraInterf.AtivarBotao("FeixeATP");
                                        Fase.BarraInterf.AtivarBotao("Lisossomo");
                                    }
                                } break;
                            case 8:
                                {
                                    if (Eventos[8] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.3f));
                                        Eventos[8] = true;
                                    }
                                } break;
                            case 10:
                                {
                                    if (Eventos[10] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 150), 1.4f));
                                        Eventos[10] = true;
                                    }
                                } break;
                            case 12:
                                {
                                    if (Eventos[12] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 350), 1.2f));
                                        Eventos[12] = true;
                                    }
                                } break;
                            case 14:
                                {
                                    if (Eventos[14] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.5f));
                                        Eventos[14] = true;
                                    }
                                } break;
                            //5 especiais em 15
                            case 20:
                                {
                                    if (Eventos[20] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[20] = true;
                                    }
                                } break;
                            case 15:
                                {
                                    if (Eventos[15] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 150), 1.3f));
                                        Eventos[15] = true;
                                    }
                                } break;
                            case 25:
                                {
                                    if (Eventos[25] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[25] = true;
                                    }
                                } break;
                            case 33:
                                {
                                    if (Eventos[33] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 150), new Vector2(0, 350), 1.5f));
                                        Eventos[33] = true;
                                    }
                                } break;
                            case 35:
                                {
                                    if (Eventos[35] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.4f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(200, 0), 1.1f));
                                        Eventos[35] = true;
                                    }
                                } break;
                            case 38:
                                {
                                    if (Eventos[38] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), 1.6f));
                                        Eventos[38] = true;
                                    }
                                } break;
                            case 40:
                                {
                                    if (Eventos[40] == false)
                                    {
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 250)));
                                        Fase.BarraInterf.AtivarBotao("Ressonante");
                                        Fase.Messenger.AdicionarMensagem("Este é o Radion. Ele muda o formato dos seus códons.\nUse o ressonante para eliminá-lo.", 7000f, 1.0f);
                                        Eventos[40] = true;
                                    }
                                } break;
                            case 42:
                                {
                                    if (Eventos[42] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.3f));
                                        Eventos[42] = true;
                                    }
                                } break;
                            case 45:
                                {
                                    if (Eventos[45] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 150), 1.4f));
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 250), 1.0f));
                                        Eventos[45] = true;
                                    }
                                } break;
                            case 47:
                                {
                                    if (Eventos[47] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 350), 1.2f));
                                        Eventos[47] = true;
                                    }
                                } break;
                            case 49:
                                {
                                    if (Eventos[49] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.5f));
                                        Eventos[49] = true;
                                    }
                                } break;
                            //5 especiais em 50
                            case 55:
                                {
                                    if (Eventos[55] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[55] = true;
                                    }
                                } break;
                            case 56:
                                {
                                    if (Eventos[56] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[56] = true;
                                    }
                                } break;
                            case 65:
                                {
                                    if (Eventos[65] == false)
                                    {
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 250)));
                                        Eventos[65] = true;
                                    }
                                } break;
                            case 70:
                                {
                                    if (Eventos[70] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.1f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 200), 1.2f));
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 250), 1.1f));
                                        Eventos[70] = true;
                                    }
                                } break;
                            case 72:
                                {
                                    if (Eventos[72] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), 1.2f));
                                        Eventos[72] = true;
                                    }
                                } break;
                            //5 especiais em 75
                            case 80:
                                {
                                    if (Eventos[80] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 150), 1.0f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), 1.0f));
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[80] = true;
                                    }
                                } break;
                            case 85:
                                {
                                    if (Eventos[85] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 200), 1.0f));
                                        Eventos[85] = true;
                                    }
                                } break;
                            case 90:
                                {
                                    if (Eventos[90] == false)
                                    {
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 400), 1.5f));
                                        Eventos[90] = true;
                                    }
                                } break;
                            case 99:
                                {
                                    if (Eventos[99] == false)
                                    {
                                        Fase.GM.TelaGaroto.Boy.LoadParte("leftEar", "1");
                                        Fase.GM.TelaGaroto.Boy.LoadParte("rightEar", "1");
                                        Fase.GM.TelaGaroto.Boy.HabilitarParte("mouth");
                                        Encerrada = true;
                                        Eventos[99] = true;
                                    }
                                }
                                break;

                        }

                    } break;
                #endregion
                #region Fase 8 : Boca
                case "mouth":
                    {
                        switch (NumCodons)
                        {
                            case 1:
                                {
                                    if (Eventos[1] == false)
                                    {
                                        Fase.BarraInterf.AtivarBotao("FeixeATP");
                                        Fase.BarraInterf.AtivarBotao("Lisossomo");
                                        Fase.BarraInterf.AtivarBotao("Ressonante");
                                    }
                                } break;
                            case 3:
                                {
                                    if (Eventos[3] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.3f));
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 200), 0.7f));
                                        Eventos[3] = true;
                                    }
                                } break;
                            case 5:
                                {
                                    if (Eventos[5] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 150), 1.4f));
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 250), 0.7f));
                                        Eventos[5] = true;
                                    }
                                } break;
                            case 7:
                                {
                                    if (Eventos[7] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 350), 1.2f));
                                        Eventos[7] = true;
                                    }
                                } break;
                            case 9:
                                {
                                    if (Eventos[9] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.5f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 150), 0.8f));
                                        Eventos[9] = true;
                                    }
                                } break;
                            case 10:
                                {
                                    if (Eventos[10] == false)
                                    {
                                        //travar a fase aqui
                                        Fase.BarraInterf.AtivarBotao("Pilula");
                                        Fase.Messenger.AdicionarMensagem("Devido à grande dificuldade de manter a síntese,\nfoi desenvolvida uma pílula. Ela acabará com os\nobstáculos que você encontrar", 7000f, 1.0f);
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.3f));
                                        Eventos[10] = true;
                                    }
                                } break;
                            case 13:
                                {
                                    if (Eventos[13] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 150), 1.4f));
                                        Eventos[13] = true;
                                    }
                                } break;
                            case 14:
                                {
                                    if (Eventos[14] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 350), 1.2f));
                                        Eventos[14] = true;
                                    }
                                } break;
                            case 15:
                                {
                                    if (Eventos[15] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.5f));
                                        Eventos[15] = true;
                                    }
                                } break;
                            //5 especiais em 15
                            case 18:
                                {
                                    if (Eventos[18] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 250), 1.1f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 100), 1.3f));
                                        Eventos[18] = true;
                                    }
                                } break;
                            case 19:
                                {
                                    if (Eventos[19] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(250, 0), 1.2f));
                                        Eventos[19] = true;
                                    }
                                } break;
                            case 20:
                                {
                                    if (Eventos[20] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.1f));
                                        Eventos[20] = true;
                                    }
                                } break;
                            case 22:
                                {
                                    if (Eventos[22] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[22] = true;
                                    }
                                } break;
                            case 25:
                                {
                                    if (Eventos[25] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 250), 0.7f));
                                        Eventos[25] = true;
                                    }
                                } break;

                            case 30:
                                {
                                    if (Eventos[30] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 200)));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 250), 1.1f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 100), 1.3f));
                                        Eventos[30] = true;
                                    }
                                } break;
                            case 31:
                                {
                                    if (Eventos[31] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(100, 0), 1.2f));
                                        Eventos[31] = true;
                                    }
                                } break;
                            case 33:
                                {
                                    if (Eventos[33] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.1f));
                                        Eventos[33] = true;
                                    }
                                } break;
                            //5 especiais em 45
                            case 45:
                                {
                                    if (Eventos[45] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[45] = true;
                                    }
                                } break;
                            case 50:
                                {
                                    if (Eventos[22] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 400), 0.95f));
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 200)));
                                        Eventos[22] = true;
                                    }
                                } break;
                            case 54:
                                {
                                    if (Eventos[54] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 150), new Vector2(0, 400), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.4f));
                                        Eventos[54] = true;
                                    }
                                } break;
                            case 55:
                                {
                                    if (Eventos[55] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[55] = true;
                                    }
                                } break;
                            case 56:
                                {
                                    if (Eventos[56] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), new Vector2(0, 200), 1.2f));
                                        Eventos[56] = true;
                                    }
                                } break;
                            case 64:
                                {
                                    if (Eventos[64] == false)
                                    {
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 200)));
                                        Eventos[64] = true;
                                    }
                                } break;
                            case 65:
                                {
                                    if (Eventos[65] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[65] = true;
                                    }
                                } break;
                            //10 especiais em 70
                            case 73:
                                {
                                    if (Eventos[73] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 0.8f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 200), 0.9f));
                                        Eventos[73] = true;
                                    }
                                } break;
                            case 75:
                                {
                                    if (Eventos[75] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.1f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 100), 1.2f));
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 250), 1.0f));
                                        Eventos[75] = true;
                                    }
                                } break;
                            case 76:
                                {
                                    if (Eventos[76] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 300), 1.6f));
                                        Eventos[76] = true;
                                    }
                                } break;
                            case 80:
                                {
                                    if (Eventos[80] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 200)));
                                        Eventos[80] = true;
                                    }
                                } break;
                            case 85:
                                {
                                    if (Eventos[85] == false)
                                    {
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 400)));
                                        Eventos[85] = true;
                                    }
                                } break;
                            case 88:
                                {
                                    if (Eventos[88] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.3f));
                                        Eventos[88] = true;
                                    }
                                } break;
                            case 90:
                                {
                                    if (Eventos[90] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(LimiteX, 400), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(LimiteX, 150), 1.4f));
                                        Eventos[90] = true;
                                    }
                                } break;
                            case 91:
                                {
                                    if (Eventos[91] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 350), 1.2f));
                                        Eventos[91] = true;
                                    }
                                } break;
                            case 93:
                                {
                                    if (Eventos[93] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.5f));
                                        Eventos[93] = true;
                                    }
                                } break;
                            case 99:
                                {
                                    if (Eventos[99] == false)
                                    {
                                        Fase.GM.TelaGaroto.Boy.LoadParte("mouth", "1");
                                        Fase.GM.TelaGaroto.Boy.HabilitarParte("nose");
                                        Encerrada = true;
                                        Eventos[99] = true;
                                    }
                                }
                                break;


                        }

                    } break;
                #endregion
                #region Fase 9 : Nariz
                case "nose":
                    {
                        switch (NumCodons)
                        {
                            case 1:
                                {
                                    if (Eventos[1] == false)
                                    {
                                        Fase.BarraInterf.AtivarBotao("FeixeATP");
                                        Fase.BarraInterf.AtivarBotao("Lisossomo");
                                        Fase.BarraInterf.AtivarBotao("Ressonante");
                                        Fase.BarraInterf.AtivarBotao("Pilula");
                                    }
                                } break;
                            //5 especiais em 10
                            case 13:
                                {
                                    if (Eventos[13] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.3f));
                                        Eventos[13] = true;
                                    }
                                } break;
                            case 15:
                                {
                                    if (Eventos[15] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 150), 1.4f));
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[15] = true;
                                    }
                                } break;
                            case 17:
                                {
                                    if (Eventos[17] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 350), 1.2f));
                                        Eventos[17] = true;
                                    }
                                } break;
                            case 19:
                                {
                                    if (Eventos[19] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.5f));
                                        Eventos[19] = true;
                                    }
                                } break;
                            case 25:
                                {
                                    if (Eventos[25] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 150), 1.2f));
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 350)));
                                        Eventos[25] = true;
                                    }
                                } break;
                            case 30:
                                {
                                    if (Eventos[30] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 500), 0.6f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 250), 0.9f));
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[30] = true;
                                    }
                                } break;
                            case 32:
                                {
                                    if (Eventos[32] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 350), new Vector2(0, 250), 1.0f));
                                        Eventos[32] = true;
                                    }
                                } break;
                            case 33:
                                {
                                    if (Eventos[33] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 0.7f));
                                        Eventos[33] = true;
                                    }
                                } break;
                            case 35:
                                {
                                    if (Eventos[35] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), new Vector2(0, 300), 0.8f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.1f));
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[35] = true;
                                    }
                                } break;
                            case 45:
                                {
                                    if (Eventos[45] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 250), 0.9f));
                                        Eventos[45] = true;
                                    }
                                } break;
                            case 50:
                                {
                                    if (Eventos[22] == false)
                                    {
                                        NovoObstaculo(new Virus(Fase, new Vector2(LimiteX, 500)));
                                        Fase.Messenger.AdicionarMensagem("Eis o Vírus.", 7000f, 1.0f);
                                        Eventos[22] = true;
                                    }
                                } break;
                            case 55:
                                {
                                    if (Eventos[55] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 300)));
                                        Eventos[55] = true;
                                    }
                                } break;
                            //5 especiais em 60
                            case 70:
                                {
                                    if (Eventos[70] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 200), 1.1f));
                                        NovoObstaculo(new Virus(Fase, new Vector2(LimiteX, 400)));
                                        Eventos[70] = true;
                                    }
                                } break;
                            case 73:
                                {
                                    if (Eventos[73] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 0.8f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 200), 0.9f));
                                        Eventos[73] = true;
                                    }
                                } break;
                            case 75:
                                {
                                    if (Eventos[75] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(0, 300), new Vector2(LimiteX, 400), 1.1f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 100), 1.2f));
                                        Eventos[75] = true;
                                    }
                                } break;
                            case 76:
                                {
                                    if (Eventos[76] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 300), 1.6f));
                                        Eventos[76] = true;
                                    }
                                } break;
                            case 80:
                                {
                                    if (Eventos[80] == false)
                                    {
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 200)));
                                        Eventos[80] = true;
                                    }
                                } break;
                            case 85:
                                {
                                    if (Eventos[85] == false)
                                    {
                                        NovoObstaculo(new Virus(Fase, new Vector2(LimiteX, 250)));
                                        Eventos[85] = true;
                                    }
                                } break;

                            case 95:
                                {
                                    if (Eventos[95] == false)
                                    {
                                        Fase.GM.TelaGaroto.Boy.LoadParte("nose", "1");
                                        Fase.GM.TelaGaroto.Boy.HabilitarParte("eyes");
                                        Encerrada = true;
                                        Eventos[95] = true;
                                    }
                                }
                                break;

                        }

                    } break;
                #endregion
                #region Fase 10 : Olhos
                case "eyes":
                    {
                        switch (NumCodons)
                        {
                            case 1:
                                {
                                    if (Eventos[1] == false)
                                    {
                                        Fase.BarraInterf.AtivarBotao("FeixeATP");
                                        Fase.BarraInterf.AtivarBotao("Lisossomo");
                                        Fase.BarraInterf.AtivarBotao("Ressonante");
                                        Fase.BarraInterf.AtivarBotao("Pilula");
                                    }
                                } break;
                            case 3:
                                {
                                    if (Eventos[3] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 250), 1.1f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 100), 1.3f));
                                        Eventos[3] = true;
                                    }
                                } break;
                            case 4:
                                {
                                    if (Eventos[4] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(250, 0), 1.2f));
                                        Eventos[4] = true;
                                    }
                                } break;
                            case 5:
                                {
                                    if (Eventos[5] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.1f));
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 450)));
                                        Eventos[5] = true;
                                    }
                                } break;
                            case 10:
                                {
                                    if (Eventos[10] == false)
                                    {
                                        Fase.BarraInterf.AtivarBotao("Imunizador");
                                        Fase.Messenger.AdicionarMensagem("Agora você pode usar o Imunizador\npara curar sua síntese do Vírus!", 5000f, 1.0f);
                                        Eventos[10] = true;
                                    }
                                } break;
                            //10 especiais em 15
                            case 15:
                                {
                                    if (Eventos[15] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 250), 1.3f));
                                        Eventos[15] = true;
                                    }
                                } break;
                            case 18:
                                {
                                    if (Eventos[18] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.3f));
                                        Eventos[18] = true;
                                    }
                                } break;
                            case 20:
                                {
                                    if (Eventos[20] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.3f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(0, 450), new Vector2(LimiteX, 150), 1.4f));
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 200), 0.7f));
                                        Eventos[20] = true;
                                    }
                                } break;
                            case 21:
                                {
                                    if (Eventos[21] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 350), 1.2f));
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[21] = true;
                                    }
                                } break;
                            case 23:
                                {
                                    if (Eventos[23] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.5f));
                                        Eventos[23] = true;
                                    }
                                } break;
                            case 25:
                                {
                                    if (Eventos[25] == false)
                                    {
                                        NovoObstaculo(new Virus(Fase, new Vector2(LimiteX, 500)));
                                        Eventos[25] = true;
                                    }
                                } break;
                            case 32:
                                {
                                    if (Eventos[32] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.3f));
                                        Eventos[32] = true;
                                    }
                                } break;
                            case 33:
                                {
                                    if (Eventos[33] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(0, 300), new Vector2(LimiteX, 200), 1.1f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(0, 450), new Vector2(LimiteX, 100), 1.4f));
                                        Eventos[33] = true;
                                    }
                                } break;
                            case 34:
                                {
                                    if (Eventos[34] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(0, 150), 0.9f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(0, 350), 1.0f));
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[34] = true;
                                    }
                                } break;
                            case 35:
                                {
                                    if (Eventos[35] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(0, 150), new Vector2(LimiteX, 400), 1.4f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 300), 1.4f));
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[35] = true;
                                    }
                                } break;
                            case 36:
                                {
                                    if (Eventos[36] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.3f));
                                        Eventos[36] = true;
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                    }
                                } break;
                            case 40:
                                {
                                    if (Eventos[40] == false)
                                    {
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 250)));
                                        NovoObstaculo(new Virus(Fase, new Vector2(LimiteX, 500)));
                                        Eventos[40] = true;
                                    }
                                } break;
                            case 45:
                                {
                                    if (Eventos[45] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 250), 1.3f));
                                        Eventos[45] = true;
                                    }
                                } break;
                            case 49:
                                {
                                    if (Eventos[49] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), 1.2f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 450), new Vector2(0, 200), 1.0f));
                                        Eventos[49] = true;
                                    }
                                } break;
                            case 50:
                                {
                                    if (Eventos[50] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 150), 0.9f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.2f));
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 250)));
                                        Eventos[50] = true;
                                    }
                                } break;
                            case 53:
                                {
                                    if (Eventos[53] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 300), new Vector2(0, 400), 1.1f));
                                        Eventos[53] = true;
                                    }
                                } break;
                            //5 especiais em 55
                            case 60:
                                {
                                    if (Eventos[60] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        NovoObstaculo(new Virus(Fase, new Vector2(LimiteX, 300)));
                                        Eventos[60] = true;
                                    }
                                } break;
                            case 65:
                                {
                                    if (Eventos[65] == false)
                                    {
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 200), 0.8f));
                                        Eventos[65] = true;
                                    }
                                } break;
                            case 75:
                                {
                                    if (Eventos[75] == false)
                                    {
                                        NovoObstaculo(new Enzima(Fase, new Vector2(LimiteX, 350), 1.3f));
                                        Eventos[75] = true;
                                    }
                                } break;
                            case 80:
                                {
                                    if (Eventos[80] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 200), new Vector2(0, 500), 0.6f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 250), 0.9f));
                                        NovoObstaculo(new Virus(Fase, new Vector2(LimiteX, 400)));
                                        Eventos[80] = true;
                                    }
                                } break;
                            case 82:
                                {
                                    if (Eventos[82] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 350), new Vector2(0, 250), 1.0f));
                                        Eventos[82] = true;
                                    }
                                } break;
                            case 83:
                                {
                                    if (Eventos[83] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 0.7f));
                                        Eventos[83] = true;
                                    }
                                } break;
                            //5 especiais em 85
                            case 90:
                                {
                                    if (Eventos[90] == false)
                                    {
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), new Vector2(0, 300), 0.8f));
                                        NovoObstaculo(new Ameba(Fase, new Vector2(LimiteX, 400), 1.1f));
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[90] = true;
                                    }
                                } break;
                            case 92:
                                {
                                    if (Eventos[92] == false)
                                    {
                                        NovoObstaculo(new Parasita(Fase, Fase.Codons.Last<Codon>()));
                                        Eventos[92] = true;
                                    }
                                } break;
                            case 97:
                                {
                                    if (Eventos[97] == false)
                                    {
                                        NovoObstaculo(new Radion(Fase, new Vector2(LimiteX, 150), 1.5f));
                                        Eventos[97] = true;
                                    }
                                } break;

                            case 105:
                                {
                                    if (Eventos[105] == false)
                                    {
                                        Fase.GM.TelaGaroto.Boy.LoadParte("eyes", "1");
                                        Encerrada = true;
                                        Eventos[105] = true;
                                    }
                                }
                                break;


                        }

                    } break;
                #endregion
              

            }
        }

      /// <summary>
      /// Cria um novo Codon.
      /// </summary>
      /// <param name="osCodons">A lista de Codons da fase.</param>
      /// <param name="dist">A distância entre cada Codon.</param>
        public void NovoCodon(List<Codon> osCodons,int dist)
        {
            // Cria um novo Codon se houver a distância determinada entre o último Codon e o limite da tela.
            if (osCodons.Count>0)
            {
                if ((LimiteX - osCodons.Last<Codon>().Retangulo.Right) > dist)
                {
                    Fase.GerarCodon();
                    NumCodons += 1;
                }
            }
            else
            {
                Fase.GerarCodon();
                NumCodons += 1;
            }
        }

        /// <summary>
        /// Adiciona um obstáculo à fase.
        /// </summary>
        /// <param name="obstaculo">O obstáculo a ser adicionado</param>
        public void NovoObstaculo(ObjetoMovel obstaculo)
        {
            Fase.Obstaculos.Add(obstaculo);
        }
        #endregion
    }
}
