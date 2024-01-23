using Newtonsoft.Json;
using RestSharp;
using System;
using System.Diagnostics;
using System.IO;
using Facturador_por_dia;

namespace Facturador_por_dia
{
    public class clsListo
    {

        public partial class ListoSendMail
        {
            [JsonProperty("to")]
            public string To { get; set; }

            [JsonProperty("from")]
            public string From { get; set; }

            [JsonProperty("subject")]
            public string Subject { get; set; }

        }

        public partial class ListoCancellation
        {
            [JsonProperty("rfc_id")]
            public string RfcId { get; set; }

            [JsonProperty("key_id")]
            public string KeyId { get; set; }

            [JsonProperty("message")]
            public string Message { get; set; }

            [JsonProperty("reason")]
            public string Reason { get; set; }

            [JsonProperty("substituted_by_uuid")]
            public string SubstitutedByUuid { get; set; }
        }


        public class ClienteListo
        {

            public string NameCust { get; set; }
            public string Rfc { get; set; }
            public string mail { get; set; }
            public string Address { get; set; }
            public string CpCustomer { get; set; }
            public string reg_fis_customer { get; set; }
            public string f_forma_pago { get; set; }
            public string f_metodo_pago { get; set; }
            public string f_uso_cfdi { get; set; }
            public string f_comentario { get; set; }


        }

        public AtributosResponseGenerateSignUsingPrivateKey Generatesignusingprivatekey(string api_url, string token, string rfc_id, string key_id, string Message)
        {
            AtributosResponseGenerateSignUsingPrivateKey Signature;
            var log = new clsLog();
            var respuesta = "";
            try
            {
                api_url = api_url + "invoicing/sign_using_private_key";
                Message = Message.Substring(1, Message.Length - 2);
                var json = JsonConvert.DeserializeObject<AtributosResponseGenerateXml>(Message);

                var OriginalChain = json.OriginalChain;
                var xml = json.Xml;

                var client = new RestClient(api_url);
                client.Timeout = 15000;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Token " + token);
                request.AlwaysMultipartFormData = true;
                request.AddParameter("rfc_id", rfc_id);
                request.AddParameter("key_id", key_id);
                request.AddParameter("message", OriginalChain);
                var response = client.Execute(request);
                respuesta = response.Content;
                Signature = JsonConvert.DeserializeObject<AtributosResponseGenerateSignUsingPrivateKey>(respuesta);
                return Signature;
            }
            catch (Exception ex)
            {
                log.EscribirLog("Error Private_key: " + ex.Message + " " + ex.StackTrace);
                Signature = new AtributosResponseGenerateSignUsingPrivateKey
                {
                    Signature = ""
                };
                return Signature;
            }
        }

        public AtributosResponseCertify Certify(string api_url, string token, string xml, string data, string certificate_num, string signature, string certificate)
        {
            AtributosResponseCertify invoice_certify;
            var log = new clsLog();
            var respuesta = "";
            try
            {
                api_url = api_url + "invoicing/certify_xml";
                var client = new RestClient(api_url);
                client.Timeout = 20000;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Token " + token);
                //request.AlwaysMultipartFormData = true;
                ;
                data = data.Substring(1, data.Length - 2);

                //xml = xml.Replace("\n", "");
                //xml = xml.Replace("\r", "");


                var strbody = "{";
                strbody += "\"data\": " + data + ",\n";
                strbody += "\"xml\": " + "\"" + xml + ",";
                strbody += "\"signature\": " + "\"" + signature + "\"" + ",";
                strbody += "\"certificate_num\": " + "\"" + certificate_num + "\"" + ",";
                strbody += "\"certificate\": " + "\"" + certificate + "\"";
                strbody += "}";
                request.AddParameter("application/json", strbody, ParameterType.RequestBody);
                var response = client.Execute(request);
                respuesta = response.Content;
                invoice_certify = JsonConvert.DeserializeObject<AtributosResponseCertify>(respuesta);
                return invoice_certify;
            }
            catch (Exception ex)
            {
                log.EscribirLog("Error Certify: " + ex.Message + " " + ex.StackTrace);
                invoice_certify = new AtributosResponseCertify
                {
                    Status = "Error"
                };
                return invoice_certify;
            }
        }

        public void ObtieneFacturaListo(string api_url, string token, string tipo, string IdDocumento)
        {

            try
            {
                api_url = api_url + "documents/full/" + IdDocumento;
                var client = new RestClient(api_url);
                client.Timeout = -1;
                var request = new RestRequest(Method.GET);
                request.AddHeader("Authorization", "Token " + token);
                var bytes = client.DownloadData(request);

                if (tipo == "PDF")
                {
                    File.WriteAllBytes(@"C:\MerchantExport\Factura-" + IdDocumento + ".pdf", bytes);
                    Process.Start(@"C:\MerchantExport\Factura-" + IdDocumento + ".pdf");
                }
                else
                {
                    File.WriteAllBytes(@"C:\MerchantExport\Factura-" + IdDocumento + ".xml", bytes);
                    Process.Start(@"C:\MerchantExport\Factura-" + IdDocumento + ".xml");
                }

            }
            catch (Exception ex)
            {

            }
        }


        public string cancelacion(string api_url, string token, string sale_order_id, string json_request, int invoice_id)
        {

            var respuesta = "";
            try
            {
                api_url = api_url + "invoicing/cancel_using_vault_private_key/invoice/" + invoice_id + "/";
                var client = new RestClient(api_url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Token " + token);
                var strbody = json_request;
                request.AddParameter("application/json", strbody, ParameterType.RequestBody);
                var response = client.Execute(request);
                respuesta = response.Content;
                return respuesta;
            }
            catch (Exception ex)
            {

                return ex.Message.ToString();
            }
        }



        public string sendMail(string api_url, string token, string mail_to, int invoice_id)
        {
            var respuesta = "";
            try
            {
                api_url = api_url + "invoicing/invoices/" + invoice_id + "/email";
                var client = new RestClient(api_url);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Authorization", "Token " + token);
                request.AlwaysMultipartFormData = true;
                request.AddParameter("to", mail_to);
                request.AddParameter("from", "facturacion@mercadazo.com.mx");
                request.AddParameter("subject", "Tu factura de Mercadazo esta lista");
                var response = client.Execute(request);
                respuesta = response.Content;
                return respuesta;
            }
            catch (Exception ex)
            {

                return ex.Message.ToString();
            }
        }


    }
}