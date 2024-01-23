using Newtonsoft.Json;

namespace Facturador_por_dia
{
    public partial class AtributosResponseGenerateSignUsingPrivateKey
    {
        [JsonProperty("signature")]
        public string Signature { get; set; }
    }
}