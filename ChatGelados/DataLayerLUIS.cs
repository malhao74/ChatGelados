using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Configuration;

namespace ChatGelados
{
    class DataLayerLUIS
    {
        #region Declaracao de variaveis
        private readonly string link = "https://westus.api.cognitive.microsoft.com/luis/v2.0/apps/2eaa44b9-4338-47b6-9214-9b38fc9afd05?subscription-key=d9f61ba079f54dde8870efc7c2de6482&spellCheck=true&bing-spell-check-subscription-key={YOUR_BING_KEY_HERE}&verbose=true&timezoneOffset=0&q=";

        public readonly string mensagem;

        public delegate void Got_Data(bool correuTudoBem, LUIS luis);
        public event Got_Data DataLayer_GotData;
        #endregion

        public DataLayerLUIS(string mensagemNew)
        {
            mensagem = mensagemNew;
        }

        public async Task GetResposta()
        {
            HttpClient client = new HttpClient();
            string requestUri = $"{link}{mensagem}";
            HttpResponseMessage response = await client.GetAsync(requestUri);
            client.Dispose();

            if (response.IsSuccessStatusCode == false)
            {
                DataLayer_GotData(false, null);
                return;
            }
            string resposta = await response.Content.ReadAsStringAsync();
            Console.WriteLine(resposta);

            LUIS luis = JsonConvert.DeserializeObject<LUIS>(resposta);
            DataLayer_GotData(true, luis);
        }
    }
}
