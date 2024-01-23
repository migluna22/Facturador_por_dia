using Facturador_por_dia;
using MySqlConnector;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Text;

namespace Facturador_por_dia
{
    public class ModelListoFac
    {
        public bool _SendFacListo(DataTable data, string cus_id)
            
         //  (string sale_order_id, string reference_order_number, string cus_id, string seller_sku, string quantity, string unit_price_tax_inc, string product_id, string coment, string receiver_rfc, string receiver_rfc_name, string receiver_intended_use, string receiver_address_postal_code, string receiver_tax_regime, string payment_form, string payment_method, double iva_produc, string related_cfdis, string product_name, string category_id, string descuento)
        {
            var log = new clsLog();
            try
            {
                
                // var quantity = "";
                double iva_produc = 0;

                string product_id = "";
                string sale_order_id = "";
                string reference_order_number = "";
                string coment = "";
                string related_cfdis = "";
                string category_id = "";
                string product_name = "";


                var string_request_GenerateXML = "";
                var string_response_GenerateXML = "";
                var json_request = new AtributosRequestGenerateXml();
                var series = "";
                var folio = "";
                var issued_on = DateTime.Now;
                var issued_at = "04010"; // codigo postal de mercadazo
                           
                var comments = "";
                var currency = "MXN";
                var exchange_rate = "1";
                var version = "4.0";
                var reimbursement = false;
                var issuer_id = "0";
                //var issuer_rfc = "EKU9003173C9";
                //var issuer_rfc_name = "Escuela Kemper Urgate";
                var issuer_rfc = "TME120322EQ6";
                var issuer_rfc_name = "TIENDA MERCADAZO";
                var issuer_tax_regime = "601";                            
                var items_taxation_type = "";
                var export_type = "";

                var periodicidad = "01";
                var mes_periodo = DateTime.Now.Month;
                var año_perido = DateTime.Now.Year;
                // **********************
                var payment_form = "PUE";
                var payment_method = "31";

                var receiver_rfc = "XAXX010101000";
                var receiver_rfc_name = "PUBLICO EN GENERAL";
                var receiver_address_postal_code = "04010";
                var receiver_intended_use = "S01";
                var receiver_tax_regime = "616";

                // ****************
                var items_id = "";
                var items_description = "";
                var items_quantity = "";
                double items_unitary_amount = 0;
                var items_product_code = "";
                var items_units_code = "";
                var items_units = "";
                double items_taxes_pass_through_base = 0;
                double items_taxes_pass_through_amount = 0;
                var items_taxes_pass_through_rate = "";
                var items_taxes_pass_through_tax = "";

                var status = "";
                var invoice_id = "";
                var invoice_client_name = "";
                var invoice_currency = "";
                double invoice_subtotal = 0;
                var invoice_total = "";
                var invoice_pdf_ulr = "";
                var invoice_pdf_filename = "";
                var invoice_xml_ulr = "";
                var invoice_xml_filename = "";
                var invoice_invoice_id = "";
                var invoice_cfdi_payment_receipt_id = "";
                var invoice_withholding_id = "";



                var cn = new clsConexion();

                //var dt = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_RFC_ID_DES'", new MySqlParameter[] { }, CommandType.Text);
                var dt = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_RFC_ID_PROD'", new MySqlParameter[] { }, CommandType.Text);
                var dr = dt.Rows[0];
                issuer_id = dr["value"].ToString();

                //obtener folios de listo 
                var query = cn.TraeDataTable("SELECT serie,folio FROM sale_order_folio_listo WHERE invoice_type_id=1 AND id_customer=@cus_id and estatus=1 order by folio limit 1",
                      new[] {
                        new MySqlParameter("@cus_id", cus_id)
                      }, CommandType.Text);

                var series_folio = query.Rows.Count;
                if (query != null)
                {
                    if (series_folio > 0)
                    {
                        series = query.Rows[0]["serie"].ToString();
                        folio = query.Rows[0]["folio"].ToString();
                    }
                    else
                    {
                        var query2 = cn.TraeDataTable("SELECT serie FROM sale_order_folio_listo WHERE invoice_type_id=1 AND id_customer=@cus_id order by folio limit 1",
                            new[] {
                            new MySqlParameter("@cus_id", cus_id)
                            }, CommandType.Text);
                        series_folio = query2.Rows.Count;


                        if (series_folio > 0)
                        {
                            series = query2.Rows[0]["serie"].ToString();
                        }


                    }
                }
                if (folio == null || folio == "" || folio == "0")
                {
                    var query3 = cn.TraeDataTable("call SP_Insert_Nex_Folio('" + series + "', " + cus_id + ",1)", new MySqlParameter[] { }, CommandType.Text);
                    folio = query3.Rows[0][0].ToString();
                    //coment = "No existen folios para terminar el proceso de facturación";
                    //return false;

                }
                var json_request_items_1 = new List<Items_1>();
               

                foreach (DataRow order in data.Rows)                
                {
                    var json_request_item_taxes_1 = new Taxes_1();

                    var so = order["sale_order_id"].ToString();
                    reference_order_number = order["reference_order_number"].ToString();
                    int customer_Id = int.Parse(order["customer_id"].ToString());
                    var customer_name = order["customer_name"].ToString();
                    var sellersku = order["seller_sku"].ToString();
                    string quantity = order["quantity"].ToString();
                    string unit_price_tax_inc = order["unit_price_tax_inc"].ToString();
                    product_id = order["product_id"].ToString();


                    items_taxes_pass_through_base = 0;
                    items_taxes_pass_through_amount = 0;
                    items_unitary_amount = 0;

                    items_taxes_pass_through_base = Math.Round((double.Parse(unit_price_tax_inc) / 1.16), 2) * (Int32.Parse(quantity)); //obtener precio

                    items_taxes_pass_through_amount = (Math.Round((double.Parse(unit_price_tax_inc) / 1.16), 2) * 0.16) * (Int32.Parse(quantity));

                    items_taxes_pass_through_rate = "16.00000";
                    items_taxes_pass_through_tax = "IVA";

                    //para cada item primero creamos el impuesto, siempre es una arreglo de 1

                    var json_request_item_taxes_passthroughs = new List<PassThrough>();
                    var json_request_item_taxes_passthroug = new PassThrough()
                    {
                        Base = items_taxes_pass_through_base.ToString(),
                        Amount = items_taxes_pass_through_amount.ToString(),
                        Rate = items_taxes_pass_through_rate,
                        Tax = items_taxes_pass_through_tax
                    };

                    json_request_item_taxes_passthroughs.Add(json_request_item_taxes_passthroug);
                    json_request_item_taxes_1.PassThrough = json_request_item_taxes_passthroughs;

                    items_id = product_id;
                    items_description = so.ToString() + "_" + reference_order_number + "_VENTA";
                    items_description = items_description.Replace(",", "");
                    items_description = items_description.Replace("/", "");
                    items_description = items_description.Replace("'", "");
                    items_quantity = "1";
                    items_unitary_amount = Math.Round((double.Parse(unit_price_tax_inc) / 1.16), 2) * (Int32.Parse(quantity));
                    items_product_code = "01010101";
                    items_units_code = "ACT";
                    items_units = "ACT";
                    items_taxation_type = "02";

                    var json_request_item_1 = new Items_1()
                    {
                        Id = items_id,
                        Description = items_description,
                        Quantity = items_quantity,
                        UnitaryAmount = items_unitary_amount.ToString(),
                        ProductCode = items_product_code,
                        UnitsCode = items_units_code,
                        Units = items_units,
                        Taxes_1 = json_request_item_taxes_1,
                        Items_taxation_type = items_taxation_type
                    };
                    json_request_items_1.Add(json_request_item_1);

                }

                var json_request_taxes_2 = new List<Taxes_2>();
                var json_request_taxe_2 = new Taxes_2();
                json_request_taxes_2.Add(json_request_taxe_2);

                var json_request_related_cfdis = new List<RelatedCfdis>();
                var json_request_related_cfdis_uuids = new List<string>();
                json_request_related_cfdis_uuids.Add("");
                var related_cfdis_type = "";
                var json_request_related_cfdi = new RelatedCfdis()
                {
                    Relationship_type = related_cfdis_type,
                    Uuids = json_request_related_cfdis_uuids
                };

                json_request_related_cfdis.Add(json_request_related_cfdi);
                var json_request_issuer = new Issuer()
                {
                    Id = issuer_id,
                    Rfc = issuer_rfc,
                    RfcName = issuer_rfc_name,
                    TaxRegime = issuer_tax_regime
                };

                var json_request_global_info = new Global_info()
                {
                    Periodicity = periodicidad,
                    Months = mes_periodo.ToString("00"),
                    Year = año_perido.ToString(),
                };

                var json_request_receiver_address = new Address_1();
                json_request_receiver_address.PostalCode = receiver_address_postal_code;
                var json_request_receiver = new Receiver()
                {
                    Rfc = receiver_rfc,
                    RfcName = receiver_rfc_name,
                    IntendedUse = receiver_intended_use,
                    TaxRegime = receiver_tax_regime,
                    Address = json_request_receiver_address
                };

                var json_request_addendas = new Addendas();
                export_type = "01";
                json_request.Series = series;
                json_request.Folio = folio;
                json_request.IssuedOn = issued_on;
                json_request.IssuedAt = issued_at;
                json_request.PaymentForm = payment_form;
                json_request.PaymentMethod = payment_method;
                json_request.Comments = comments;
                json_request.Currency = currency;
                json_request.ExchangeRate = exchange_rate;
                json_request.Version = version;
                json_request.Reimbursement = reimbursement;
                json_request.Issuer = json_request_issuer;
                json_request.Global_info = json_request_global_info;
                json_request.Receiver = json_request_receiver;
                json_request.Export_type = export_type;
                json_request.Items_1 = json_request_items_1;
                json_request.Taxes_2 = json_request_taxes_2;
                json_request.RelatedCfdisType = related_cfdis_type;
                json_request.RelatedCfdis = json_request_related_cfdis;
                json_request.Addendas = json_request_addendas;

                string_request_GenerateXML = JsonConvert.SerializeObject(json_request);
                string_request_GenerateXML = "[" + string_request_GenerateXML + "]";
                string_request_GenerateXML = string_request_GenerateXML.Replace("[{}]", "[]");
                string_response_GenerateXML = GenerateXML(string_request_GenerateXML, sale_order_id);

                AtributosResponseGenerateSignUsingPrivateKey responseSignature;
                AtributosResponseCertify responseCertify;

                if (string_response_GenerateXML != "")
                {
                    var message = string_response_GenerateXML;
                    //message = message.Replace("00000000000000000000", "30001000000400002434"); // Desarrollo
                    message = message.Replace("00000000000000000000", "00001000000509454375"); // Produccion 
                    responseSignature = Generate_sing_using_private_key(message, sale_order_id);

                    if (responseSignature.Signature != "")
                    {
                        message = message.Substring(9, message.Length - 11);
                        String[] Datosenvio = message.Split(",");
                        String certify_xml = Datosenvio[0];
                        var certify_data = string_request_GenerateXML;
                        var certify_signature = responseSignature.Signature;
                        
                        responseCertify = GenerateCertify(certify_xml, certify_data, certify_signature, sale_order_id);
                        
                        if (responseCertify.Status == "error")
                        {
                            coment = responseCertify.ErrorDescription;
                            return false;
                        }

                        status = responseCertify.Status;
                        invoice_id = responseCertify.Invoice.Id.ToString();
                        invoice_client_name = responseCertify.Invoice.ClientName;
                        invoice_currency = responseCertify.Invoice.Currency;
                        var tot = responseCertify.Invoice.Total.ToString();
                        invoice_subtotal = Math.Round((double.Parse(tot) / 1.16), 4);
                        var iva = responseCertify.Invoice.Total - invoice_subtotal;
                        var invoice_iva = Math.Round(double.Parse(iva.ToString()), 4);
                        invoice_total = responseCertify.Invoice.Total.ToString();
                        invoice_pdf_ulr = responseCertify.Invoice.Pdf.Url;
                        invoice_pdf_filename = responseCertify.Invoice.Pdf.Filename;
                        invoice_xml_ulr = responseCertify.Invoice.Xml.Url;
                        invoice_xml_filename = responseCertify.Invoice.Xml.Filename;
                        invoice_invoice_id = responseCertify.Invoice.InvoiceId.ToString();
                        invoice_cfdi_payment_receipt_id = "";
                        invoice_withholding_id = "";

                        cn.EjecutarConsulta("UPDATE sale_order_folio_listo SET estatus = 0 WHERE serie= @series and folio =@folio",
                            new[]
                            {
                                new MySqlParameter("@series", series),
                                new MySqlParameter("@folio", folio)
                            }, CommandType.Text);

                        var string_response_certify = JsonConvert.SerializeObject(responseCertify);
                        var uuid = invoice_pdf_filename.Replace(".pdf", "");
                        var user = "1";

                        foreach (DataRow order1 in data.Rows)
                        {

                            cn.EjecutarConsulta("insert into listo_request (sale_order_id, reference_order_number, request, response, invoice_id," +
                                       "invoice_client_name, invoice_currency, invoice_subtotal, invoice_iva, invoice_total, invoice_pdf_url, invoice_pdf_filename," +
                                       "invoice_xml_url, invoice_xml_filename, invoice_invoice_id, invoice_cfdi_payment_receipt_id," +
                                       "invoice_withholding_id, UUID, date_created, user_id, serie, folio)" +
                                       "values(@sale_order_id,@reference_order_number,@string_request_GenerateXML," +
                                       "@string_reponse_certify,@invoice_id,@invoice_client_name," +
                                       "@invoice_currency,@invoice_subtotal,@invoice_iva,@invoice_total,@invoice_pdf_ulr,@invoice_pdf_filename,@invoice_xml_ulr, " +
                                       "@invoice_xml_filename,@invoice_invoice_id,@invoice_cfdi_payment_receipt_id,@invoice_withholding_id,@uuid,@data_create,@user,@series,@folio)", new[]
                                       {
                                            new MySqlParameter("@sale_order_id", order1["sale_order_id"].ToString()),
                                            new MySqlParameter("@reference_order_number", order1["reference_order_number"].ToString()),
                                            new MySqlParameter("@string_request_GenerateXML", ""), //string_request_GenerateXML
                                            new MySqlParameter("@string_reponse_certify", ""), //string_response_certify
                                            new MySqlParameter("@invoice_id", invoice_id),
                                            new MySqlParameter("@invoice_client_name",invoice_client_name),
                                            new MySqlParameter("@invoice_currency", invoice_currency),
                                            new MySqlParameter("@invoice_subtotal", invoice_subtotal),
                                            new MySqlParameter("@invoice_iva", invoice_iva),
                                            new MySqlParameter("@invoice_total", invoice_total),
                                            new MySqlParameter("@invoice_pdf_ulr", invoice_pdf_ulr),
                                            new MySqlParameter("@invoice_pdf_filename", invoice_pdf_filename),
                                            new MySqlParameter("@invoice_xml_ulr", invoice_xml_ulr),
                                            new MySqlParameter("@invoice_xml_filename", invoice_xml_filename),
                                            new MySqlParameter("@invoice_invoice_id", invoice_invoice_id),
                                            new MySqlParameter("@invoice_cfdi_payment_receipt_id", invoice_cfdi_payment_receipt_id),
                                            new MySqlParameter("@invoice_withholding_id", invoice_withholding_id),
                                            new MySqlParameter("@uuid", uuid),
                                            new MySqlParameter("@data_create",DateTime.Now),
                                            new MySqlParameter("@user", user),
                                            new MySqlParameter("@series", series),
                                            new MySqlParameter("@folio", folio)
                                       }, CommandType.Text);




                        }

                           


                        //var source = "OT";
                        //var response_id = "0";
                        //var request_str = "";
                        //var response_str = "OK: ENVIADO A LISTO ASIGNACION AUTOMATICA";

                        //Proceso para marcar los items seleccionados como enviadios a listo 
                        //cn.EjecutarConsulta("INSERT INTO autofactura_request (sale_order_id,reference_order_number,source,response_id,request_str,response_str,date_created) " +
                        //    "VALUES (@sale_order_id,@reference_order_number,@source,@response_id,@request_str,@response_str,@date_created)",
                        //    new[]
                        //{
                        //                    new MySqlParameter("@sale_order_id", sale_order_id),
                        //                    new MySqlParameter("@reference_order_number", reference_order_number),
                        //                    new MySqlParameter("@source", source),
                        //                    new MySqlParameter("@response_id", response_id),
                        //                    new MySqlParameter("@request_str", request_str),
                        //                    new MySqlParameter("@response_str", response_str),
                        //                    new MySqlParameter("@date_created", DateTime.Now)
                        //}, CommandType.Text);


                        log.EscribirLog("Pedido Facturado con éxito:" + sale_order_id);

                    }
                    else
                    {
                        coment = "No se puede obtner la firma";
                        return false;
                    }
                }
                else
                {
                    coment = "No se puede generarl el XML";
                    return false;
                }




                coment = "Éxito";
                return false;

            }
            catch (Exception ex)
            {
                log.EscribirLog("Error: " + ex.Message + " " + ex.StackTrace);
                // coment = ex.Message;
                return false;
            }
        }

        private AtributosResponseCertify GenerateCertify(string xml, string data, string signature, string sale_order_id)
        {
            var log = new clsLog();
            var myListo = new clsListo();
            var cn = new clsConexion();
            var rawResponse = "";
            var strParams = "";
            var api_url = "";

            //var query = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_API_URL_DES';", new MySqlParameter[] { }, CommandType.Text);
            var query = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_API_URL_PROD';", new MySqlParameter[] { }, CommandType.Text);
            var dr = query.Rows[0];
            api_url = dr["value"].ToString();

            var toke = "";
            //var query2 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_TOKEN_DES';", new MySqlParameter[] { }, CommandType.Text); // desarrollo 
            var query2 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_TOKEN_PROD';", new MySqlParameter[] { }, CommandType.Text); // desarrollo 
            var drToke = query2.Rows[0];
            toke = drToke["value"].ToString();


            var vault_private_keys_certificate_num = "";
            //var query3 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_VAULT_PRIVATE_KEYS_CERTIFICATE_NUM_DES';", new MySqlParameter[] { }, CommandType.Text); //desarrollo 
            var query3 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_VAULT_PRIVATE_KEYS_CERTIFICATE_NUM_PROD';", new MySqlParameter[] { }, CommandType.Text); // produccion  
            var drkey = query3.Rows[0];
            vault_private_keys_certificate_num = drkey["value"].ToString();


            var certificado_base64 = "";
            //var query4 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_CERTIFICADO_BASE64_DES';", new MySqlParameter[] { }, CommandType.Text); //desarrollo 
            var query4 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_CERTIFICADO_BASE64_PROD';", new MySqlParameter[] { }, CommandType.Text); //produccion
            var drBase64 = query4.Rows[0];
            certificado_base64 = drBase64["value"].ToString();

            var json = new AtributosResponseCertify();
            try
            {
                json = myListo.Certify(api_url, toke, xml, data, vault_private_keys_certificate_num, signature, certificado_base64);
                log.EscribirLog("json: " + json.ToString());
                log.EscribirLog("json s: " + json.Status);
                log.EscribirLog("json ec: " + json.ErrorCode);
                log.EscribirLog("json ed: " + json.ErrorDescription);

                rawResponse = json.Status.ToString();
                log.AgregarPCRequest(strParams, rawResponse, "0", sale_order_id, "Facturador Automatico Listo");
                return json;

            }
            catch (Exception ex)
            {
                
                log.EscribirLog("Error Certify:" + ex.Message + " " + ex.StackTrace); 
                rawResponse = ex.Message;
                log.AgregarPCRequest(strParams, rawResponse, "0", sale_order_id, "Error Facturador Automatico Listo");
                json = null;
                return json;
            }

        }



        private AtributosResponseGenerateSignUsingPrivateKey Generate_sing_using_private_key(string message, string sale_order_id)
        {
            var log = new clsLog();
            var myListo = new clsListo();
            var rawResponse = "";
            var strParams = "";
            var api_url = "";

            var cn = new clsConexion();
            //var query = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_API_URL_DES';", new MySqlParameter[] { }, CommandType.Text); // desarrollo 
            var query = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_API_URL_PROD';", new MySqlParameter[] { }, CommandType.Text); // produccion  
            var dr = query.Rows[0];
            api_url = dr["value"].ToString();

            var token = "";
            //var query2 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_TOKEN_DES';", new MySqlParameter[] { }, CommandType.Text); // desarrollo 
            var query2 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_TOKEN_PROD';", new MySqlParameter[] { }, CommandType.Text); // produccion  
            var dr2 = query2.Rows[0];
            token = dr2["value"].ToString();

            var vault_private_keys_id = "";
            //var query3 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_VAULT_PRIVATE_KEYS_ID_DES';", new MySqlParameter[] { }, CommandType.Text); // desarrollo 
            var query3 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_VAULT_PRIVATE_KEYS_ID_PROD';", new MySqlParameter[] { }, CommandType.Text); // producción 
            var dr3 = query3.Rows[0];
            vault_private_keys_id = dr3["value"].ToString();

            var rfc_id = "";
            //var query4 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_RFC_ID_DES';", new MySqlParameter[] { }, CommandType.Text); // desarrollo 
            var query4 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_RFC_ID_PROD';", new MySqlParameter[] { }, CommandType.Text); // producción 
            var dr4 = query4.Rows[0];
            rfc_id = dr4["value"].ToString();

            var json = new AtributosResponseGenerateSignUsingPrivateKey();
            strParams = message;
            rawResponse = "";
            try
            {
                json = myListo.Generatesignusingprivatekey(api_url, token, rfc_id, vault_private_keys_id, message);
                rawResponse = json.Signature;
                log.AgregarPCRequest(strParams, rawResponse, "0", sale_order_id, "Facrurador Automatico Listo");
                return json;

            }
            catch (Exception ex)
            {
                log.EscribirLog("Error Private_key: " + ex.Message + " " + ex.StackTrace);
                rawResponse = ex.Message;
                log.AgregarPCRequest(strParams, rawResponse, "0", sale_order_id, "Error Facturador Automatico Listo");
                json = null;
                return json;
            }

        }

        public string GenerateXML(string json_request, string sale_order_id)
        {
            var log = new clsLog();
            var rawResponse = "";
            var strParams = "";
            var api_url = "";
            var cn = new clsConexion();

            //var query = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_API_URL_DES';", new MySqlParameter[] { }, CommandType.Text); // desarrollo 
            var query = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_API_URL_PROD';", new MySqlParameter[] { }, CommandType.Text); // produccion  
            var dr = query.Rows[0];
            api_url = dr["value"].ToString();

            var token = "";
            //var query2 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_TOKEN_DES';", new MySqlParameter[] { }, CommandType.Text); // desarrollo 
            var query2 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_TOKEN_PROD';", new MySqlParameter[] { }, CommandType.Text); // produccion  
            var dr2 = query2.Rows[0];
            token = dr2["value"].ToString();

            var vault_private_keys_id = "";
            //var query3= cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_VAULT_PRIVATE_KEYS_ID_DES';", new MySqlParameter[] { }, CommandType.Text); // desarrollo 
            var query3 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_VAULT_PRIVATE_KEYS_ID_PROD';", new MySqlParameter[] { }, CommandType.Text); // producción 
            var dr3 = query3.Rows[0];
            vault_private_keys_id = dr3["value"].ToString();

            var vault_private_keys_certificate_num = "";
            //var query4 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_VAULT_PRIVATE_KEYS_CERTIFICATE_NUM_DES';", new MySqlParameter[] { }, CommandType.Text); // desarrollo 
            var query4 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_VAULT_PRIVATE_KEYS_CERTIFICATE_NUM_PROD';", new MySqlParameter[] { }, CommandType.Text); // producción 
            var dr4 = query4.Rows[0];
            vault_private_keys_certificate_num = dr4["value"].ToString();

            var rfc_id = "";
            //var query5 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_RFC_ID_DES';", new MySqlParameter[] { }, CommandType.Text); // desarrollo 
            var query5 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_RFC_ID_PROD';", new MySqlParameter[] { }, CommandType.Text); // producción 
            var dr5 = query5.Rows[0];
            rfc_id = dr5["value"].ToString();

            var certificado_base64 = "";
            //var query6 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_CERTIFICADO_BASE64_DES';", new MySqlParameter[] { }, CommandType.Text); // desarrollo 
            var query6 = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'ENDPOINT_LISTO_CERTIFICADO_BASE64_PROD';", new MySqlParameter[] { }, CommandType.Text); // producción 
            var dr6 = query6.Rows[0];
            certificado_base64 = dr6["value"].ToString();

            try
            {
                api_url = api_url + "invoicing/generate_xml";
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(api_url);
                myReq.Headers.Add("Authorization", "Token " + token);
                myReq.Headers.Add("cache-control", "no-cache");
                myReq.Method = "POST";
                myReq.ContentType = "application/json";
                strParams = json_request;
                var stream = myReq.GetRequestStream();
                stream.Write(Encoding.UTF8.GetBytes(strParams), 0, strParams.Length);
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();

                var myreader = new StreamReader(myResp.GetResponseStream());
                rawResponse = myreader.ReadToEnd();

                var json_response = rawResponse;
                if (json_response.Length > 100)
                {
                    log.AgregarPCRequest(strParams, rawResponse, "0", sale_order_id, "Facturador Automatico Listo");
                    return json_response;

                }
                else
                {
                    throw new Exception(json_response);
                    json_response = null;
                    return json_response;
                }
            }

            catch (Exception ex)
            {
                log.EscribirLog("Error XML: " + ex.Message + " " + ex.StackTrace);
                rawResponse = ex.Message;
                log.AgregarPCRequest(strParams, rawResponse, "0", sale_order_id, "Error Facturador Automatico Listo");
                var json_response = "";
                return json_response;
            }
        }

        public bool Valida_Cust_Id_Fac(int cust_id)
        {
            var cn = new clsConexion();
            var dt = cn.TraeDataTable("SELECT value FROM m_configuration WHERE name = 'LISTO_CUST_ID_ACTIVE_FACTURADOR';", new MySqlParameter[] { }, CommandType.Text);
            var dr = dt.Rows[0];
            var lista_cust_id = dr["value"].ToString();

            string[] lista = lista_cust_id.Split('|', '.');

            foreach (var lista_split in lista)
            {

                if (int.Parse(lista_split) == cust_id)
                {
                    return true;

                }
            }
            return false;
        }

        public bool Valida_Fact_Anterior(string sale_order_id)
        {

            var fac_existe = false;
            var cn = new clsConexion();
            var dt = cn.TraeDataTable("SELECT sale_order_id FROM listo_request WHERE sale_order_id =" + sale_order_id + " order by sale_order_id", new MySqlParameter[] { }, CommandType.Text);

            if (dt.Rows.Count > 0)
            {
                var dr = dt.Rows[0];
                var fac_conteo = dr["sale_order_id"].ToString();
                if (fac_conteo != null || fac_conteo != "")
                {
                    fac_existe = true;
                }
                else
                {
                    fac_existe = false;
                }
            }

            return fac_existe;
        }
    }
}