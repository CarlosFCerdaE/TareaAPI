using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;
using WebCRUDapi.Models;
using Newtonsoft.Json;
using System.Text;

namespace WebCRUDapi.Controllers
{
    public class CreditCardController : Controller
    {
        private string baseURL = "http://localhost:53494/";
        // GET: CreditCard
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Lista()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.GetAsync("/api/creditcard").Result;
            string data = response.Content.ReadAsStringAsync().Result;
            List<CreditCardCLS> tarjetas = JsonConvert.DeserializeObject<List<CreditCardCLS>>(data);

            return Json(
                new
                {
                    success = true,
                    data = tarjetas,
                    message = "done"
                },
                JsonRequestBehavior.AllowGet
                );
        }

        public JsonResult Guardar(int CreditCardID, string CardType, string CardNumber, int ExpMonth, int ExpYear)
        {
            try
            {
                CreditCardCLS tarjeta = new CreditCardCLS();
                tarjeta.CreditCardID = CreditCardID;
                tarjeta.CardType = CardType;
                tarjeta.CardNumber = CardNumber;
                tarjeta.ExpMonth = ExpMonth;
                tarjeta.ExpYear = ExpYear;
                tarjeta.ModifiedDate = DateTime.Now;

                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri(baseURL);
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                string tarjetaJson = JsonConvert.SerializeObject(tarjeta);
                HttpContent body = new StringContent(tarjetaJson, Encoding.UTF8, "application/json");

                if (CreditCardID == 0)
                {
                    HttpResponseMessage response = httpClient.PostAsync("/api/creditcard", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Tarjeta creada satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    HttpResponseMessage response = httpClient.PutAsync($"/api/creditcard/{CreditCardID}", body).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        return Json(
                            new
                            {
                                success = true,
                                message = "Tarjeta modificada satisfactoriamente"
                            }, JsonRequestBehavior.AllowGet);
                    }
                }
                throw new Exception("Error al guardar");
            }

            catch (Exception ex)
            {
                return Json(
                    new
                    {
                        success = false,
                        message = ex.InnerException
                    }, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult Eliminar(int CreditCardID)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(baseURL);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = httpClient.DeleteAsync($"/api/creditcard/{CreditCardID}").Result;

            if (response.IsSuccessStatusCode)
            {
                return Json(
                    new
                    {
                        success = true,
                        message = "Tarjeta eliminada satisfactoriamente"
                    }, JsonRequestBehavior.AllowGet);
            }

            throw new Exception("Error al eliminar");
        }
    }
}