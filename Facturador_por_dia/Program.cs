using MySqlConnector;
using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.ServiceModel;

namespace Facturador_por_dia
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var log = new clsLog(); // se hace el llamdo a la clase Log
            // Inicio de proceso facturacion
            try
            {
                var query = ConfigurationManager.AppSettings["SPConsulta_Shine"];
                var cn = new clsConexion();
                var data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                log.EscribirLog("---------- INICIO ----------");
                log.EscribirLog("-----FACTURACION SHEIN -----");
                
                log.EscribirLog(data.Rows.Count + " pedidos a facturar");
               
                if (data.Rows.Count > 0)
                {
                    int veces_fac = 0;
                    if (data.Rows.Count == 700)
                    {
                        veces_fac = 1;
                    }
                    else {
                        decimal paso = (decimal)(data.Rows.Count / 700) + 1;
                        veces_fac = (int)Math.Ceiling(paso);
                    }                
                    var modelListo = new ModelListoFac();

                    log.EscribirLog("en " +veces_fac +" facturas");
                    for (int i = 0; i < veces_fac; i++)
                    { 
                        modelListo._SendFacListo(data, "127");
                    }
                }
                log.EscribirLog("-------- FIN SHEIN ---------");

                
                //  ************ WALMART
                query = ConfigurationManager.AppSettings["SPConsulta_Walmart_T"];
                data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                log.EscribirLog("----------------------------");
                log.EscribirLog("--- FACTURACION WALMART ----");
                log.EscribirLog(data.Rows.Count + " pedidos a facturar");

                if (data.Rows.Count > 0)
                {
                    int veces_fac = 0;
                    if (data.Rows.Count == 700)
                    {
                        veces_fac = 1;
                    }
                    else
                    {
                        decimal paso = (decimal)(data.Rows.Count / 700) + 1;
                        veces_fac = (int)Math.Ceiling(paso);
                    }
                    var modelListo = new ModelListoFac();

                    log.EscribirLog("en " + veces_fac + " facturas");
                    for (int i = 0; i < veces_fac; i++)
                    {
                        query = ConfigurationManager.AppSettings["SPConsulta_Walmart"];
                        data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                        int pp = data.Rows.Count;
                        modelListo._SendFacListo(data, "81");
                    }
                }
                log.EscribirLog("------- FIN WALMART --------");



                //  ************ LIVERPOOL
                query = ConfigurationManager.AppSettings["SPConsulta_Liverpool_T"];
                data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                log.EscribirLog("----------------------------");
                log.EscribirLog("---FACTURACION LIVERPOOL ---");
                log.EscribirLog(data.Rows.Count + " pedidos a facturar");

                if (data.Rows.Count > 0)
                {
                    int veces_fac = 0;
                    if (data.Rows.Count == 700)
                    {
                        veces_fac = 1;
                    }
                    else
                    {
                        decimal paso = (decimal)(data.Rows.Count / 700) + 1;
                        veces_fac = (int)Math.Ceiling(paso);
                    }
                    var modelListo = new ModelListoFac();

                    log.EscribirLog("en " + veces_fac + " facturas");
                    for (int i = 0; i < veces_fac; i++)
                    {
                        query = ConfigurationManager.AppSettings["SPConsulta_Liverpool"];
                        data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                        int pp = data.Rows.Count;
                        modelListo._SendFacListo(data, "100");
                    }
                }
                log.EscribirLog("------- FIN LIVERPOOL ------");



                //  ************ ELEKTRA
                query = ConfigurationManager.AppSettings["SPConsulta_Elektra"];
                data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                log.EscribirLog("----------------------------");
                log.EscribirLog("--- FACTURACION ELEKTRA ----");               
                log.EscribirLog(data.Rows.Count + " pedidos a facturar");

                if (data.Rows.Count > 0)
                {
                    int veces_fac = 0;
                    if (data.Rows.Count == 700)
                    {
                        veces_fac = 1;
                    }
                    else
                    {
                        decimal paso = (decimal)(data.Rows.Count / 700) + 1;
                        veces_fac = (int)Math.Ceiling(paso);
                    }
                    var modelListo = new ModelListoFac();

                    log.EscribirLog("en " + veces_fac + " facturas");
                    for (int i = 0; i < veces_fac; i++)
                    {
                        modelListo._SendFacListo(data, "96");
                    }
                }
                log.EscribirLog("------- FIN ELEKTRA --------");



                //  ************ COPPEL
                query = ConfigurationManager.AppSettings["SPConsulta_Coppel"];
                data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                log.EscribirLog("----------------------------");
                log.EscribirLog("---- FACTURACION COPPEL ----");      
                log.EscribirLog(data.Rows.Count + " pedidos a facturar");

                if (data.Rows.Count > 0)
                {
                    int veces_fac = 0;
                    if (data.Rows.Count == 700)
                    {
                        veces_fac = 1;
                    }
                    else
                    {
                        decimal paso = (decimal)(data.Rows.Count / 700) + 1;
                        veces_fac = (int)Math.Ceiling(paso);
                    }
                    var modelListo = new ModelListoFac();

                    log.EscribirLog("en " + veces_fac + " facturas");
                    for (int i = 0; i < veces_fac; i++)
                    {
                        modelListo._SendFacListo(data, "102");
                    }
                }

                log.EscribirLog("-------- FIN COPPEL --------");


                //  ************ JARDIMEX
                query = ConfigurationManager.AppSettings["SPConsulta_Jardimex"];
                data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                log.EscribirLog("----------------------------");
                log.EscribirLog("--- FACTURACION JARDIMEX ---");
                log.EscribirLog(data.Rows.Count + " pedidos a facturar");

                if (data.Rows.Count > 0)
                {
                    int veces_fac = 0;
                    if (data.Rows.Count == 700)
                    {
                        veces_fac = 1;
                    }
                    else
                    {
                        decimal paso = (decimal)(data.Rows.Count / 700) + 1;
                        veces_fac = (int)Math.Ceiling(paso);
                    }
                    var modelListo = new ModelListoFac();

                    log.EscribirLog("en " + veces_fac + " facturas");
                    for (int i = 0; i < veces_fac; i++)
                    {
                        modelListo._SendFacListo(data, "98");
                    }
                }
                log.EscribirLog("------- FIN JARDIMEX -------");


                //  ************ CENTURFIT
                query = ConfigurationManager.AppSettings["SPConsulta_Centurfit"];
                data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                log.EscribirLog("----------------------------");
                log.EscribirLog("--- FACTURACION CENTURFIT---");
                log.EscribirLog(data.Rows.Count + " pedidos a facturar");

                if (data.Rows.Count > 0)
                {
                    int veces_fac = 0;
                    if (data.Rows.Count == 700)
                    {
                        veces_fac = 1;
                    }
                    else
                    {
                        decimal paso = (decimal)(data.Rows.Count / 700) + 1;
                        veces_fac = (int)Math.Ceiling(paso);
                    }
                    var modelListo = new ModelListoFac();

                    log.EscribirLog("en " + veces_fac + " facturas");
                    for (int i = 0; i < veces_fac; i++)
                    {
                        modelListo._SendFacListo(data, "107");
                    }
                }
                log.EscribirLog("------- FIN CENTURFIT ------");


                //  ************ GUTSTARK
                query = ConfigurationManager.AppSettings["SPConsulta_Gutstark"];
                data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                log.EscribirLog("----------------------------");
                log.EscribirLog("--- FACTURACION GUTSTARK ---");
                log.EscribirLog(data.Rows.Count + " pedidos a facturar");

                if (data.Rows.Count > 0)
                {
                    int veces_fac = 0;
                    if (data.Rows.Count == 700)
                    {
                        veces_fac = 1;
                    }
                    else
                    {
                        decimal paso = (decimal)(data.Rows.Count / 700) + 1;
                        veces_fac = (int)Math.Ceiling(paso);
                    }
                    var modelListo = new ModelListoFac();

                    log.EscribirLog("en " + veces_fac + " facturas");
                    for (int i = 0; i < veces_fac; i++)
                    {
                        modelListo._SendFacListo(data, "108");
                    }
                }
                log.EscribirLog("------- FIN GUTSTARK -------");


                //  ************ NANOFORT
                query = ConfigurationManager.AppSettings["SPConsulta_Nanofort"];
                data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                log.EscribirLog("----------------------------");
                log.EscribirLog("--- FACTURACION NANOFORT ---");
                log.EscribirLog(data.Rows.Count + " pedidos a facturar");

                if (data.Rows.Count > 0)
                {
                    int veces_fac = 0;
                    if (data.Rows.Count == 700)
                    {
                        veces_fac = 1;
                    }
                    else
                    {
                        decimal paso = (decimal)(data.Rows.Count / 700) + 1;
                        veces_fac = (int)Math.Ceiling(paso);
                    }
                    var modelListo = new ModelListoFac();

                    log.EscribirLog("en " + veces_fac + " facturas");
                    for (int i = 0; i < veces_fac; i++)
                    {
                        modelListo._SendFacListo(data, "110");
                    }
                }
                log.EscribirLog("------- FIN NANOFORT -------");


                //  ************ AUDIOTEK
                query = ConfigurationManager.AppSettings["SPConsulta_Audiotek"];
                data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                log.EscribirLog("----------------------------");
                log.EscribirLog("--- FACTURACION AUDIOTEK ---");
                log.EscribirLog(data.Rows.Count + " pedidos a facturar");

                if (data.Rows.Count > 0)
                {
                    int veces_fac = 0;
                    if (data.Rows.Count == 700)
                    {
                        veces_fac = 1;
                    }
                    else
                    {
                        decimal paso = (decimal)(data.Rows.Count / 700) + 1;
                        veces_fac = (int)Math.Ceiling(paso);
                    }
                    var modelListo = new ModelListoFac();

                    log.EscribirLog("en " + veces_fac + " facturas");
                    for (int i = 0; i < veces_fac; i++)
                    {
                        modelListo._SendFacListo(data, "111");
                    }
                }
                log.EscribirLog("------- FIN AUDIOTEK -------");


                //  ************ LITTLE MONKEY
                query = ConfigurationManager.AppSettings["SPConsulta_Little_Monkey"];
                data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                log.EscribirLog("----------------------------");
                log.EscribirLog("- FACTURACION LITTLE MONKEY-");
                log.EscribirLog(data.Rows.Count + " pedidos a facturar");

                if (data.Rows.Count > 0)
                {
                    int veces_fac = 0;
                    if (data.Rows.Count == 700)
                    {
                        veces_fac = 1;
                    }
                    else
                    {
                        decimal paso = (decimal)(data.Rows.Count / 700) + 1;
                        veces_fac = (int)Math.Ceiling(paso);
                    }
                    var modelListo = new ModelListoFac();

                    log.EscribirLog("en " + veces_fac + " facturas");
                    for (int i = 0; i < veces_fac; i++)
                    {
                        modelListo._SendFacListo(data, "112");
                    }
                }
                log.EscribirLog("---- FIN LITTLE MONKEY -----");


                //  ************ HOUZER
                query = ConfigurationManager.AppSettings["SPConsulta_Houzer"];
                data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                log.EscribirLog("----------------------------");
                log.EscribirLog("---- FACTURACION HOUZER ----");
                log.EscribirLog(data.Rows.Count + " pedidos a facturar");

                if (data.Rows.Count > 0)
                {
                    int veces_fac = 0;
                    if (data.Rows.Count == 700)
                    {
                        veces_fac = 1;
                    }
                    else
                    {
                        decimal paso = (decimal)(data.Rows.Count / 700) + 1;
                        veces_fac = (int)Math.Ceiling(paso);
                    }
                    var modelListo = new ModelListoFac();

                    log.EscribirLog("en " + veces_fac + " facturas");
                    for (int i = 0; i < veces_fac; i++)
                    {
                        modelListo._SendFacListo(data, "116");
                    }
                }
                log.EscribirLog("-------- FIN HOUZER --------");


                //  ************ MERCADAZO.COM
                query = ConfigurationManager.AppSettings["SPConsulta_Mercadazo"];
                data = cn.TraeDataTable(query, new MySqlParameter[] { }, CommandType.StoredProcedure);
                log.EscribirLog("----------------------------");
                log.EscribirLog("--- FACTURACION MERCADAZO --");
                log.EscribirLog(data.Rows.Count + " pedidos a facturar");

                if (data.Rows.Count > 0)
                {
                    int veces_fac = 0;
                    if (data.Rows.Count == 700)
                    {
                        veces_fac = 1;
                    }
                    else
                    {
                        decimal paso = (decimal)(data.Rows.Count / 700) + 1;
                        veces_fac = (int)Math.Ceiling(paso);
                    }
                    var modelListo = new ModelListoFac();

                    log.EscribirLog("en " + veces_fac + " facturas");
                    for (int i = 0; i < veces_fac; i++)
                    {
                        modelListo._SendFacListo(data, "62");
                    }
                }

                log.EscribirLog("------ FIN MERCADAZO -------");


                //foreach (DataRow order in data.Rows)
                //{
                //    cont++;
                //    // log.EscribirLog("-----------------------------");
                //    var so = order["sale_order_id"].ToString();
                //    var reference_order_number = order["reference_order_number"].ToString();
                //    int customer_Id = 81;
                //    var customer_name = "Walmart";
                //    var sellersku = order["seller_sku"].ToString();
                //    float qty = float.Parse(order["Concepto_Cantidad"].ToString());
                //    float price = float.Parse(order["Concepto_ValorUnitario"].ToString());
                //    int product_id = int.Parse(order["product_id"].ToString());
                //    string receiver_rfc = order["Receptor_Rfc"].ToString();
                //    string receiver_rfc_name = order["Receptor_Nombre"].ToString();
                //    string receiver_intended_use = order["Receptor_UsoCFDI"].ToString();
                //    string receiver_postal_code = order["Receptor_DomicilioFiscal"].ToString();
                //    string receiver_tax_regime = order["Receptor_RegimenFiscal"].ToString();
                //    string payment_form = order["MetodoPago"].ToString();
                //    string payment_method = order["FormaPago"].ToString();
                //    float iva = float.Parse(order["IVA"].ToString());
                //    string uuid = order["UUID_RELACIONADO"].ToString();
                //    string product_name = order["product_name"].ToString();
                //    string categoria = order["category_id"].ToString();
                //    string descuento = order["Descuento"].ToString();
                //    float price_producto = float.Parse(order["unit_price_tax_inc"].ToString());

                //    log.EscribirLog("Ped. " + cont + "/" + data.Rows.Count);
                //    log.EscribirLog("soid: " + so);
                //    log.EscribirLog("customer: " + customer_name);
                //    //log.EscribirLog("price: " + price);

                //    //var modelListo = new ModelListoFac();

                //    var valida_custom = modelListo.Valida_Cust_Id_Fac(customer_Id);
                //    if (valida_custom != false)
                //    {
                //        //validar si existe factura creada
                //        var valida_fac = modelListo.Valida_Fact_Anterior(so);
                //        if (valida_fac == false)
                //        {
                //           // modelListo._SendFacListo(so, reference_order_number, customer_Id.ToString(), sellersku, qty.ToString(), price.ToString(), product_id.ToString(), "Facturador_APOLO", receiver_rfc, receiver_rfc_name, receiver_intended_use, receiver_postal_code, receiver_tax_regime, payment_form, payment_method, iva, uuid, product_name, categoria, descuento);
                //        }
                //        else
                //        {
                //            log.EscribirLog("El pedido ya ontiene una factura:" + order["sale_order_id"].ToString());
                //        }
                //    }
                //    else
                //    {
                //        //log.EscribirLog("Customer no valido" + order["sale_order_id"].ToString());
                //    }

                //}
                log.EscribirLog("------------- FIN -------------");

            }
            catch (FaultException fex)
            {
                log.EscribirLog("Error:" + fex.Message + " " + fex.Reason + " " + fex.StackTrace);
            }
            catch (WebException wex)
            {
                log.EscribirLog("Error: " + wex.Message + " " + wex.Response);
            }
            catch (Exception ex)
            {
                log.EscribirLog("Error: " + ex.Message + " " + ex.StackTrace);
            }
        }
    }
}
