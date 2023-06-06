using Newtonsoft.Json; 

namespace TraderAzFunctions.InputDtos
{
    internal class TradeRecommendations
    {
        [JsonProperty("no_of_comments")]
        public int NumberOfComments { get; set; }

        [JsonProperty("sentiment")]
        public string Sentiment { get; set; }

        [JsonProperty("sentiment_score")]
        public double SentimentScore { get; set; }

        [JsonProperty("ticker")]
        public string Ticker { get; set; }
    }
}
