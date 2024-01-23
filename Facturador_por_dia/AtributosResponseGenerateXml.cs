using Newtonsoft.Json;

namespace Facturador_por_dia
{
    public partial class AtributosResponseGenerateXml
    {
        [JsonProperty("xml")]
        public string Xml { get; set; }

        [JsonProperty("original_chain")]
        public string OriginalChain { get; set; }
    }
}