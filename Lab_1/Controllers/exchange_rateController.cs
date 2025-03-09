using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Lab_1.Controllers
{
     public class ExchangeRateController : Controller
     {
          private static readonly Dictionary<string, Dictionary<string, double>> exchangeRates =
              new Dictionary<string, Dictionary<string, double>>()
              {
                { "USD", new Dictionary<string, double> { { "USD", 1 }, { "EUR", 0.92 }, { "RUB", 90.5 }, { "RON", 4.58 }, { "UAH", 36.2 }, { "GBP", 0.78 }, { "MDL", 18.3 } } },
                { "EUR", new Dictionary<string, double> { { "USD", 1.08 }, { "EUR", 1 }, { "RUB", 98.1 }, { "RON", 4.94 }, { "UAH", 39.6 }, { "GBP", 0.85 }, { "MDL", 19.6 } } },
                { "RUB", new Dictionary<string, double> { { "USD", 0.011 }, { "EUR", 0.0102 }, { "RUB", 1 }, { "RON", 0.050 }, { "UAH", 0.40 }, { "GBP", 0.0085 }, { "MDL", 0.20 } } },
                { "RON", new Dictionary<string, double> { { "USD", 0.22 }, { "EUR", 0.20 }, { "RUB", 19.8 }, { "RON", 1 }, { "UAH", 8.0 }, { "GBP", 0.17 }, { "MDL", 4.0 } } },
                { "UAH", new Dictionary<string, double> { { "USD", 0.028 }, { "EUR", 0.025 }, { "RUB", 2.45 }, { "RON", 0.125 }, { "UAH", 1 }, { "GBP", 0.021 }, { "MDL", 0.50 } } },
                { "GBP", new Dictionary<string, double> { { "USD", 1.28 }, { "EUR", 1.18 }, { "RUB", 117.5 }, { "RON", 5.86 }, { "UAH", 47.9 }, { "GBP", 1 }, { "MDL", 23.1 } } },
                { "MDL", new Dictionary<string, double> { { "USD", 0.054 }, { "EUR", 0.051 }, { "RUB", 5.2 }, { "RON", 0.25 }, { "UAH", 2.0 }, { "GBP", 0.043 }, { "MDL", 1 } } }
              };

          public ActionResult Index()
          {
               return View();
          }

          [HttpPost]
          public JsonResult ConvertCurrency(double amount, string fromCurrency, string toCurrency)
          {
               if (exchangeRates.ContainsKey(fromCurrency) && exchangeRates[fromCurrency].ContainsKey(toCurrency))
               {
                    double rate = exchangeRates[fromCurrency][toCurrency];
                    double convertedAmount = amount * rate;
                    return Json(new { success = true, convertedAmount = Math.Round(convertedAmount, 2) });
               }
               return Json(new { success = false, message = "Conversie invalidă!" });
          }
     }
}
