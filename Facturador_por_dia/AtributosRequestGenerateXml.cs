using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Facturador_por_dia
{
    public partial class AtributosRequestGenerateXml
    {

    }
    public partial class AtributosRequestGenerateXml
    {

        [JsonProperty("series")]
        public string Series { get; set; }

        [JsonProperty("folio")]
        public string Folio { get; set; }

        [JsonProperty("issued_on")]
        public DateTimeOffset IssuedOn { get; set; }

        [JsonProperty("issued_at")]
        public string IssuedAt { get; set; }

        [JsonProperty("payment_form")]
        public string PaymentForm { get; set; }

        [JsonProperty("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonProperty("comments")]
        public string Comments { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("exchange_rate")]
        public string ExchangeRate { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonProperty("reimbursement")]
        public bool Reimbursement { get; set; }

        [JsonProperty("issuer")]
        public Issuer Issuer { get; set; }

        [JsonProperty("global_info")]
        public Global_info Global_info { get; set; }

        [JsonProperty("receiver")]
        public Receiver Receiver { get; set; }

        [JsonProperty("items")]
        public List<Items_1> Items_1 { get; set; }

        [JsonProperty("taxes")]
        public List<Taxes_2> Taxes_2 { get; set; }

        [JsonProperty("export_type")]
        public string Export_type { get; set; }

        [JsonProperty("related_cfdis_type")]
        public string RelatedCfdisType { get; set; }

        [JsonProperty("related_cfdis")]
        public List<RelatedCfdis> RelatedCfdis { get; set; }

        [JsonProperty("addendas")]
        public Addendas Addendas { get; set; }

    }
    
    public partial class Addendas
    {
    }

    public partial class Taxes_2
    {

    }

    public partial class RelatedCfdis
    {
        [JsonProperty("uuids")]
        public List<string> Uuids { get; set; }
        [JsonProperty("relationship_type")]
        public string Relationship_type { get; set; }
    }

    public partial class Issuer
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("rfc")]
        public string Rfc { get; set; }

        [JsonProperty("rfc_name")]
        public string RfcName { get; set; }

        [JsonProperty("tax_regime")]
        public string TaxRegime { get; set; }
    }

    public partial class Global_info
    {
        [JsonProperty("periodicity")]
        public string Periodicity { get; set; }

        [JsonProperty("months")]
        public string Months { get; set; }

        [JsonProperty("year")]
        public string Year { get; set; }

    }

    public partial class Items_1
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("unitary_amount")]
        public string UnitaryAmount { get; set; }

        [JsonProperty("product_code")]
        public string ProductCode { get; set; }

        [JsonProperty("units_code")]
        public string UnitsCode { get; set; }

        [JsonProperty("units")]
        public string Units { get; set; }

        [JsonProperty("taxes")]
        public Taxes_1 Taxes_1 { get; set; }

        [JsonProperty("taxation_type")]
        public string Items_taxation_type { get; set; }

        [JsonProperty("discount")]
        public string Items_discount { get; set; }
    }

    public partial class Taxes_1
    {
        [JsonProperty("pass_through")]
        public List<PassThrough> PassThrough { get; set; }
    }
   
    public partial class PassThrough
    {
        [JsonProperty("base")]
        public string Base { get; set; }

        [JsonProperty("amount")]
        public string Amount { get; set; }

        [JsonProperty("rate")]
        public string Rate { get; set; }

        [JsonProperty("tax")]
        public string Tax { get; set; }
    }

    public partial class Receiver
    {
        [JsonProperty("rfc")]
        public string Rfc { get; set; }

        [JsonProperty("rfc_name")]
        public string RfcName { get; set; }

        [JsonProperty("intended_use")]
        public string IntendedUse { get; set; }

        [JsonProperty("tax_regime")]
        public string TaxRegime { get; set; }

        [JsonProperty("address")]
        public Address_1 Address { get; set; }
    }

    public partial class Address_1
    {

        [JsonProperty("postal_code")]
        public string PostalCode { get; set; }
    }
}