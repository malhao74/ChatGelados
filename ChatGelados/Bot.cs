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
        #region Declaracao de variaveis
        public string pergunta;
        public List<string> perguntas;
        public List<string> respostas;
        public List<string> ultimaRespostas;

        public delegate void Got_Resposta(List<string> respostas);
        public event Got_Resposta Bot_GotResposta;

        private string saudacao;
        #endregion

        #region Metodos publicos
        public Bot()
        {           
            pergunta = "";
            perguntas = new List<string>();
            respostas = new List<string>();

            switch (DateTime.Now.Hour)
            {
                case int hora when hora < 6:
                    saudacao = "Boa noite!";
                    break;
                case int hora when hora < 12:
                    saudacao = "Bom dia!";
                    break;
                case int hora when hora < 20:
                    saudacao = "Boa tarde!";
                    break;
                default:
                    saudacao = "Boa noite!";
                    break;
            }

            AddRespostas(
                new List<string>(){
                    saudacao,
                    "Em que posso ser util?"
            });
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
            if (correuBem == false || luis.topScoringIntent.intent != "GetGelado" || luis.entities.Count() == 0)
            {
                AddRespostas(
                    new List<string>(){
                        "Peço desculpa, mas eu é mais gelados...",
                        "Para outros temas, por favor contacte um BOT que não eu."
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
