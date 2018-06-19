using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatGelados
{
    class LUIS
    {
        public string query;
        public Intent topScoringIntent;
        public Intent[] intents;
        public Entitie[] entities;

        public LUIS(string query, Intent topScoringIntent, Intent[] intents, Entitie[] entities)
        {
            this.query = query;
            this.topScoringIntent = topScoringIntent;
            this.intents = intents;
            this.entities = entities;
        }
    }

    class Intent
    {
        public string intent;
        public double score;

        public Intent(string intent, double score)
        {
            this.intent = intent;
            this.score = score;
        }
    }
    class Entitie
    {
        public string entity;
        public string type;
        public int startIndex;
        public int endIndex;
        public double score;

        public Entitie(string entity, string type, int startIndex, int endIndex, double score)
        {
            this.entity = entity;
            this.type = type;
            this.startIndex = startIndex;
            this.endIndex = endIndex;
            this.score = score;
        }
    }
}
