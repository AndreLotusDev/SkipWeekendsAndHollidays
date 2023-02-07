using System.Net;
using Newtonsoft.Json;
using SkipWeekendsAndHollidays.Classes;

List<Event> hollidays = new();

var todayDate = DateTime.Now.Date;

if (todayDate.DayOfWeek == DayOfWeek.Saturday || todayDate.DayOfWeek == DayOfWeek.Sunday)
{
    Console.WriteLine("Today is a weekend");
}

string jsonString = string.Empty;
string url = @"https://www.gov.uk/bank-holidays.json";

HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
using (Stream stream = response.GetResponseStream())
using (StreamReader reader = new StreamReader(stream))
{
    jsonString = reader.ReadToEnd();
    var bankHolidays = JsonConvert.DeserializeObject<HolidaysUk>(jsonString);
    hollidays = bankHolidays
        .englandandwales.events
        .Concat(bankHolidays.scotland.events)
        .Concat(bankHolidays.northernireland.events)
        .ToList();
}

if (hollidays.Any(a => DateTime.Parse(a.date).Date == todayDate))
{
    Console.WriteLine("Today is a holliday");
}