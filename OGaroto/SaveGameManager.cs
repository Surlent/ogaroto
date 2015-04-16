#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using System.IO;
using System.Xml.Serialization;
#endregion

namespace OGaroto.Save
{
    /// <summary>
    /// Dados a serem armazenados no arquivo de save.
    /// </summary>
    [Serializable]
    public struct SaveData
    {
        public List<string> PartesAtivadas;
        public List<string> PartesAlteradas;
        //TODO: Adicionar outros dados a serem salvos.
    }

    /// <summary>
    /// Controla as operações de letura e gravação do jogo.
    /// </summary>
    class SaveGameManager
    {
        #region Atributos
        private string pasta;
        private string nomeArquivo;
        #endregion

        #region Métodos
        /// <summary>
        /// Controi o SaveGameManager definindo a pasta de conteiner e o nome do arquivo
        /// de save.
        /// </summary>
        /// <param name="aPasta">Conteiner</param>
        /// <param name="oNomeDoArquivo">Arquivo</param>
        public SaveGameManager(string aPasta, string oNomeDoArquivo)
        {
            pasta = aPasta;
            nomeArquivo = oNomeDoArquivo;
        }
        /// <summary>
        /// Salva os dados do jogo para um arquivo XML.
        /// </summary>
        /// <param name="osDados">Dados a serem salvos</param>
        /// <param name="pasta">O nome do conteiner dos dados do jogo.</param>
        /// <param name="nomeArquivo">Arquivo de save.</param>
        public void Salvar(SaveData osDados)
        {
            //Obtem o dispositivo de armazenamento
            IAsyncResult resultAsync = Guide.BeginShowStorageDeviceSelector(PlayerIndex.One, null, null);
            StorageDevice dispositivo = Guide.EndShowStorageDeviceSelector(resultAsync);
            //Abre a pasta de saves
            StorageContainer container = dispositivo.OpenContainer(pasta);
            //Gera caminho do arquivo de save
            string caminhoArquivo = Path.Combine(container.Path, nomeArquivo);
            //Abre ou cria o arquivo de save
            FileStream stream = File.Open(caminhoArquivo, FileMode.Create);
            //Converte o objeto SaveData para o formato XML
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));

            //Processa a lógica de salvar o arquivo
            serializer.Serialize(stream, osDados);
            stream.Close();
            container.Dispose();
        }

        public SaveData Carregar()
        {
            SaveData dados = new SaveData();
            //Obtem o dispositovo de armazenamento
            IAsyncResult resultAsync = Guide.BeginShowStorageDeviceSelector(PlayerIndex.One, null, null);
            StorageDevice dispositivo = Guide.EndShowStorageDeviceSelector(resultAsync);            //Abre a pasta
            StorageContainer container = dispositivo.OpenContainer(pasta);
            //Acha o caminho do arquivo de save.
            string caminho = Path.Combine(container.Path, nomeArquivo);

            //Se o arquivo não existir, retorna dados vazios.
            if (!File.Exists(caminho))
                return dados;

            //Abre o arquivo
            FileStream stream = File.Open(caminho, FileMode.Open, FileAccess.Read);
            //Lê os dados
            XmlSerializer serializer = new XmlSerializer(typeof(SaveData));
            dados = (SaveData)serializer.Deserialize(stream);
            //Entrega os dados.
            return dados;
        }
        #endregion
    }
}
