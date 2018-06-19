using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Device.Location;

namespace ChatGelados
{
    class Bot
    {
        #region Delaracao de variaveis
        public string pergunta;
        public List<string> perguntas;
        public List<string> respostas;
        public List<string> ultimaRespostas;

        public delegate void Got_Resposta(List<string> respostas);
        public event Got_Resposta Bot_GotResposta;

        private double latitude;
        private double longitude;

        private string saudacao;
        #endregion

        #region Metodos publicos
        public Bot(string perguntaNew = "")
        {           
            pergunta = perguntaNew;
            perguntas = new List<string>();
            respostas = new List<string>();

            

            if (pergunta == "")
            {
                AddRespostas(
                    new List<string>(){
                        "Bom dia!",
                        "Em que posso ser util?"
                });   
            }
            else
            {
                perguntas.Add(pergunta);
            }
        }

        public async Task ProcuraResposta(string perguntaNew = "")
        {
            pergunta = perguntaNew;

            if (pergunta == "")
            {
                return;
            }

            perguntas.Add(pergunta);
            DataLayerLUIS dataLayer = new DataLayerLUIS(pergunta);
            dataLayer.DataLayer_GotData += DataLayer_GotData;

            await dataLayer.GetResposta();

        }
        #endregion

        #region Metodos privados
        private void AddRespostas(List<String> respostasNew)
        {
            ultimaRespostas = new List<string>();
            ultimaRespostas.AddRange(respostasNew);
            respostas.AddRange(respostasNew);
        }

        private void DataLayer_GotData(bool correuBem, LUIS luis)
        {
            if (correuBem == false && luis.topScoringIntent.intent != "GetGelado")
            {
                AddRespostas(
                    new List<string>(){
                        "Peço desculpa, mas eu é mais gelados...",
                        "Para outros temas, por favor contacte outro BOT que não eu."
                });
                Bot_GotResposta(ultimaRespostas);
                return;
            }

            string resposta = "Obrigado pela sua encomenda de gelado";
            foreach (Entitie entity in luis.entities)
            {
                switch (entity.type)
                {
                    case "quantidade":
                        resposta += $" com {entity.entity} bolas";
                        break;
                    case "sabor":
                        resposta += $" de sabor a {entity.entity}";
                        break;
                    default:
                        break;
                }
            }
            AddRespostas(
                new List<string>(){
                        resposta
            });
            Bot_GotResposta(ultimaRespostas);
            return;
        }
        #endregion
    }
}
