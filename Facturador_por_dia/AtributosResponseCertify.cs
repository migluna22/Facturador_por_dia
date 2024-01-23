using Newtonsoft.Json;

namespace Facturador_por_dia
{
    public partial class AtributosResponseCertify
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        [JsonProperty("invoice")]
        public Invoice Invoice { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }
        [JsonProperty("error_description")]
        public string ErrorDescription { get; set; }
    }

    public partial class Invoice
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("client_name")]
        public string ClientName { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("pdf")]
        public Pdf Pdf { get; set; }

        [JsonProperty("xml")]
        public Pdf Xml { get; set; }

        [JsonProperty("invoice_id")]
        public long InvoiceId { get; set; }

        [JsonProperty("cfdi_payment_receipt_id")]
        public string CfdiPaymentReceiptId { get; set; }

        [JsonProperty("withholding_id")]
        public string WithholdingId { get; set; }
    }

    public partial class Pdf
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("filename")]
        public string Filename { get; set; }
    }
}