# .Net Core 背景知識

### UseRouting & UseEndPoints
___

1. UseRouting
    * Find endpoints
        - Adds route matching to the middleware pipeline.
        - This middleware looks at the set of endpoints defined in the app, and selects the best match based on the request.

1. UseEndPoints
    * Execute endpoints
        - Adds endpoint execution to the middelware pipeline.
        - It runs the delegate associated with the selected endpoint.

1. 注意一下:
    * Any middelware that appears **after** the **UseRouting()** call will know which endpoints will run eventually.
    * Any middelware that appears **before** the **UseRouting()** call won't know which endpoints will run eventually.


<br>
<br>
<br>


# Basic Authorize
基本要知道的四件事
1. Middleware vs Authorize
1. Authentication vs .Net Core
1. Authorize vs Action
1. Authentication vs Identity

<br>

### Middleware vs Authorize
___
* 有三個重點
    1. 我們必須先確認使用者的身分,接著確認使用者有無權利造訪 Action
    1. 確認權限必須在已知道 Routing 的情況下才能執行,所以必須在 UseRouting 之後
    1. 確認身分必須在確認權限之前,因為不知道身分怎麼會知道權限是否核准

* 大概 Middleware 的配置
    ```c#
    app.UseRouting();

    app.UseAuthentication();      // 這個 Middleware 會幫助確認使用者身分
    app.UseAuthorization();       // 這個 Middleware 會決定使用者有無權利造訪 Action

    app.UseEndpoints(endpoints =>
    {
        endpoints.MapDefaultControllerRoute();
    });
    ```

### Authentication vs .Net Core
___
* 在 Authentication Middleware 可子使用前,要先 register authentication 要使用什麼 schema
    * .Net Core Authentication 大概有兩種方式, cookie-session & JWT
    * 以下是 register cookie 的非常基本範例
        ```c#
        services.AddAuthentication("cookies")
        .AddCookie("cookies", config => 
        {
            config.Cookie.Name = "Grandmas.cookie";
            config.LoginPath = "/Home/Authenticate";
        });
        ``` 



### Authorize vs Action
___

1. 基本上如果想要在 Authentication Middleware 確認 run action 的權限, 就是在 action 上面加上一個 [Authorize] 的 action attribute
    ```c#
    // This Action Atttirbute will ask a question that are you allow to run this action??
    [Authorize] 
    public IActionResult Secret()
    {
        return View();
    }
    ```

### Authentication vs Identity
___

* 基本上有以下幾步
    1. 建立 Claims [身分信息欄位]
    1. 建立 ClaimIdentity(利用以上建立的 Claims) [身分證件]
    1. 建立 ClaimPrincipal(利用以上建立的 ClaimIdentities) [使用者]
    1. 運用 ClaimPrincipal 進行 Authenticate (例如把 身分信息{claims}存在cookie裡)
* Basic code
    ```c#
    // Step 1
    var grandmaClaims = new List<Claim>()
    { 
        new Claim(ClaimTypes.Name,  "Po"),
        new Claim(ClaimTypes.Email, "brian71742@gmail.com"),
        new Claim("Grandma.Says","Very Nice Boy")
    };
    // Step 2
    var claimIdentity = new ClaimsIdentity(grandmaClaims, "Grandma Identity");
    // Step 3
    var claimPrincipal = new ClaimsPrincipal(new[] { claimIdentity });
    // Step 4
    await HttpContext.SignInAsync(claimPrincipal);
    ```