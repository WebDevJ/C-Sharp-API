DAY-005
convert node API to C#


------

```
# JavaScript → C# API Conversion Guide

This project demonstrates how to convert common **JavaScript API patterns** into **C# equivalents**.

The example fetches movie data from the HackerRank mock API:
```

https://jsonmock.hackerrank.com/api/moviesdata/search/?Title=dark

```
It then:

1. Retrieves the data from the API
2. Extracts movie titles
3. Sorts them alphabetically
4. Prints both sorted and original titles

---

# Architecture Overview
```

+-------------+
 |   Client    |
 |  (C# App)   |
 +------+------+
 |
 | HTTP GET
 |
 +------v-----------------------------+
 | HackerRank Mock API                |
 | jsonmock.hackerrank.com            |
 | /moviesdata/search?Title=dark      |
 +----------------+-------------------+
 |
 | JSON Response
 |
 +-------v--------+
 | JSON Parser    |
 | System.Text.Json|
 +-------+--------+
 |
 |
 +---------v---------+
 | LINQ Processing   |
 | Select()          |
 | OrderBy()         |
 +---------+---------+
 |
 |
 +--------v--------+
 | Console Output  |
 | Sorted Titles   |
 | Original Titles |
 +-----------------+

```
---

# Request Lifecycle
```

Program Start
 |
 v
 Create HttpClient
 |
 v
 Send GET Request
 |
 v
 Receive JSON Response
 |
 v
 Deserialize JSON → C# Objects
 |
 v
 Extract Titles (LINQ Select)
 |
 v
 Sort Titles (LINQ OrderBy)
 |
 v
 Print Results to Console

```
---

# Example API Response

```json
{
  "page":1,
  "per_page":10,
  "data":[
     { "Title":"Dark City" },
     { "Title":"The Dark Knight" }
  ]
}
```

------

# JavaScript vs C# Comparison

| JavaScript        | C# Equivalent                        |
| ----------------- | ------------------------------------ |
| axios.get()       | HttpClient.GetAsync()                |
| response.data     | response.Content.ReadAsStringAsync() |
| JSON auto parsing | JsonSerializer.Deserialize()         |
| map()             | Select()                             |
| filter()          | Where()                              |
| sort()            | OrderBy()                            |
| console.log()     | Console.WriteLine()                  |
| Promise           | Task                                 |
| Dynamic typing    | Strong typing                        |

------

# 1️⃣ Axios → HttpClient

### JavaScript

```
const axios = require("axios");
const response = await axios.get(url);
```

### C#

```
HttpClient client = new HttpClient();
HttpResponseMessage response = await client.GetAsync(url);
```

### Why?

C# does not use Axios.

Instead, HTTP requests are performed using:

```
System.Net.Http.HttpClient
```

Steps:

1. Create client
2. Send request
3. Read response

------

# 2️⃣ Reading the Response Body

### JavaScript

```
response.data
```

### C#

```
string json = await response.Content.ReadAsStringAsync();
```

### Why?

C# separates:

- response metadata
- response body

The body must be **read manually**.

------

# 3️⃣ JSON Parsing

### JavaScript

Axios automatically parses JSON.

```
response.data.data
```

### C#

We must deserialize JSON manually.

```
ApiResponse result = JsonSerializer.Deserialize<ApiResponse>(json);
```

Requires:

```
using System.Text.Json;
```

------

# 4️⃣ Creating Classes to Match the API

Because C# is **strongly typed**, the JSON structure must match a C# class.

### API Response

```
{
  "page":1,
  "per_page":10,
  "data":[
     { "Title":"Dark City" }
  ]
}
```

### C# Classes

```
public class ApiResponse
{
    public List<Movie> data { get; set; }
}

public class Movie
{
    public string Title { get; set; }
}
```

------

# 5️⃣ map() → Select()

### JavaScript

```
.map(movie => movie.Title)
```

### C#

```
.Select(movie => movie.Title)
```

This comes from **LINQ**.

Import:

```
using System.Linq;
```

------

# 6️⃣ sort() → OrderBy()

### JavaScript

```
.sort((a, b) => a.localeCompare(b));
```

### C#

```
.OrderBy(title => title)
```

LINQ handles sorting operations.

------

# 7️⃣ forEach() → foreach

### JavaScript

```
response.data.data.forEach(e => {
    console.log(e.Title,"ORIGINAL TITLE")
})
```

### C#

```
foreach (var movie in result.data)
{
    Console.WriteLine($"{movie.Title} ORIGINAL TITLE");
}
```

------

# 8️⃣ console.log → Console.WriteLine

### JavaScript

```
console.log()
```

### C#

```
Console.WriteLine()
```

------

# 9️⃣ Async Function Differences

### JavaScript

```
async function getMovieTitles()
```

### C#

```
static async Task Main()
```

In C#:

- async functions return **Task**
- the application entry point can be async

------

# 🔟 Error Handling

### JavaScript

```
catch (error) {
  console.error(error.message)
}
```

### C#

```
catch (Exception ex)
{
   Console.WriteLine(ex.Message);
}
```

------

# 1️⃣1️⃣ Required Libraries

```
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
```

------

# 1️⃣2️⃣ Compilation

### Using .NET CLI

```
dotnet new console
dotnet run
```

### Using C# Compiler

```
csc Program.cs
```

------

# 1️⃣3️⃣ Output Example

```
Sorted Titles:
Dark
Dark City
The Dark Knight

Original Titles:
Dark ORIGINAL TITLE
The Dark Knight ORIGINAL TITLE
Dark City ORIGINAL TITLE
```

------

# Full Example C# Program

```
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

class Program
{
    static async Task Main()
    {
        string url = "https://jsonmock.hackerrank.com/api/moviesdata/search/?Title=dark";

        using HttpClient client = new HttpClient();

        try
        {
            HttpResponseMessage response = await client.GetAsync(url);
            string json = await response.Content.ReadAsStringAsync();

            ApiResponse result = JsonSerializer.Deserialize<ApiResponse>(json);

            List<string> titles = result.data
                .Select(movie => movie.Title)
                .OrderBy(title => title)
                .ToList();

            Console.WriteLine("Sorted Titles:");

            foreach (var title in titles)
            {
                Console.WriteLine(title);
            }

            Console.WriteLine("\nOriginal Titles:");

            foreach (var movie in result.data)
            {
                Console.WriteLine($"{movie.Title} ORIGINAL TITLE");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching movies: {ex.Message}");
        }
    }
}

public class ApiResponse
{
    public List<Movie> data { get; set; }
}

public class Movie
{
    public string Title { get; set; }
}
```

------

# Tech Stack

| Component       | Technology          |
| --------------- | ------------------- |
| Language        | C#                  |
| Framework       | .NET                |
| HTTP Client     | HttpClient          |
| JSON Parser     | System.Text.Json    |
| Data Processing | LINQ                |
| API             | HackerRank Mock API |

------

# Key Takeaway

JavaScript is **dynamic and lightweight**, while C# is **strongly typed and structured**.

Understanding the mapping between:

```
JS functional patterns
↓
C# LINQ equivalents
```

is critical when transitioning between the two languages.