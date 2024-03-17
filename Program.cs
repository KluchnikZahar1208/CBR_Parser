using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Linq;

class Program
{
    static async Task Main(string[] args)
    {
        Console.Write("Введите код валюты (например, USD): ");
        string currencyCode = Console.ReadLine().ToUpper();

        string quote = await GetCurrencyQuote(currencyCode);
        Console.WriteLine(quote);
    }

    static async Task<string> GetCurrencyQuote(string currencyCode)
    {
        using (HttpClient client = new HttpClient())
        {
            string url = $"https://www.cbr-xml-daily.ru/daily_eng_utf8.xml";
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                XDocument xmlDoc = XDocument.Parse(responseContent);

                var currencyElement = xmlDoc.Descendants("Valute")
                    .FirstOrDefault(e => e.Element("CharCode").Value == currencyCode);

                if (currencyElement != null)
                {
                    string name = currencyElement.Element("Name").Value;
                    string value = currencyElement.Element("Value").Value;
                    return $"{name}: {value} RUB";
                }
                else
                {
                    return "Валюта не найдена";
                }
            }
            else
            {
                return "Ошибка при получении данных";
            }
        }
    }
}