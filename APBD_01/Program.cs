using System.Text.RegularExpressions;

namespace s22271_APBD
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {
                    if (args.Length == 0 || args == null)
                    {
                        throw new ArgumentNullException("Parametr nie został przekazany!");
                    }

                    string websiteUrl = args[0];
                    HttpResponseMessage httpResponseMessage = await httpClient.GetAsync(websiteUrl);
                    if (httpResponseMessage.IsSuccessStatusCode == false)
                    {
                        throw new Exception("Błąd w czasie pobierania strony!");
                    }
                    else
                    {
                        string pageContent = await httpResponseMessage.Content.ReadAsStringAsync();
                        List<string> list = ExtractEmail(pageContent);
                        if (list.Count == 0)
                            throw new Exception("Nie znaleziono adresów email!");
                        else
                        {
                            foreach (var v in list)
                            {
                                Console.WriteLine(v);
                            }
                        }
                    }

                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine("Niewłaściwy adres URL!");
                }
                finally
                {
                    httpClient.Dispose();
                }
            }

        }
        public static List<string> ExtractEmail(string pageContent)
        {
            Regex regex = new Regex(@"[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.+]+");
            Match match;
            List<string> result = new List<string>();
            for (match = regex.Match(pageContent); match.Success; match = match.NextMatch())
            {
                if (result.Contains(match.Value) == false)
                {
                    result.Add(match.Value);
                }
            }
            return result;
        }

    }
}
