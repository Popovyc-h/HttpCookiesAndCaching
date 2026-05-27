using System.Net.Http;
using System.Net;

class CookieExample
{
    static async Task Main()
    {
        // 1. Створити CookieContainer для збереження cookies
        var cookieContainer = new CookieContainer();

        // 2. Створити HttpClientHandler з cookie container
        var handler = new HttpClientHandler { CookieContainer = cookieContainer };

        // 3. Створити HttpClient з handler
        using var client = new HttpClient(handler);

        // 4. Перший запит — встановити cookies
        var response1 = await client.GetAsync("https://httpbin.org/cookies/set?user=student&token=xyz789");
        Console.WriteLine($"Першого запиту статус: {response1.StatusCode}");

        // 5. Вивести cookies що були встановлені
        var cookies = cookieContainer.GetCookies(new Uri("https://httpbin.org"));
        Console.WriteLine($"\nЗбережені cookies ({cookies.Count}):");
        foreach (Cookie cookie in cookies)
        {
            Console.WriteLine("Cookie:");
            Console.WriteLine($"  Name   : {cookie.Name}");
            Console.WriteLine($"  Value  : {cookie.Value}");
            Console.WriteLine($"  Domain : {cookie.Domain}");
            Console.WriteLine($"  Path   : {cookie.Path}");
            Console.WriteLine();
        }

        // 6. Другий запит — відправити cookies
        var response2 = await client.GetAsync("https://httpbin.org/cookies");
        var body = await response2.Content.ReadAsStringAsync();
        Console.WriteLine($"\nВідповідь сервера (cookies, які він отримав):\n{body}");
    }
}

/*
Порівняйте cookies що були встановлені з тими, що сервер повернув у другому запиті
- cookies що були встановлені і ті що сервер повернув у другому запиті ідентичні нічого не зникло все на місці

Спробуйте без CookieContainer — вони будуть відправлені?
- Без CookieContainer cookies не зберігаються і не відправляються автоматично між запитами.
*/
