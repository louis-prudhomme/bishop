using System;
using System.Globalization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Bishop.Commands.CardGame
{
    public class CardGame
    {
        public CardGame(string name, string gifter)
        {
            Name = name;
            Date = DateTime.Now.ToString(CultureInfo.CreateSpecificCulture("fr-FR"));
            Gifter = gifter;
        }
        
        // Properties must have both accessors, as Mongo will need both to get or set values.
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public string Id { get; set; }
        public string Name { get; set; }
        public string Gifter { get; set; }
        public string Date { get; set; }
        

        public override string ToString()
        {
            return $"*{Name}*, offered by **{Gifter}** on the {Date}";
        }
    }
    
}