Use Ctrl+T quick search file and then use Alt+\ search membert method
ex: 搜尋"Starup.cs"然後輸入"ConfigureServices"

# 安裝 Swagger套件
ref:   
 1. https://github.com/domaindrivendev/Swashbuckle.AspNetCore  
 2. https://dotblogs.com.tw/xinyikao/2021/07/04/183326  
 3. https://dotblogs.com.tw/yc421206/2022/03/12/via_swashbuckle_write_swagger_doc_in_asp_net_core_web_api  
 4. https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/1422

T: Startup.cs  
M: ConfigureServices()  
	add: services.AddSwaggerGen(); 以及定義 c.SwaggerDoc();swagger文件  
	add: 安裝Swashbuckle.AspNetCore.Newtonsoft //services.AddSwaggerGenNewtonsoftSupport();   

啟用瀏覽器並查看(預設使用以下url)  
/swagger/{documentName}/swagger.json => "v1"  
/swagger/  

命名空間中找到 System.ComponentModel.DataAnnotations 的屬性標記模型，以協助驅動 Swagger UI 元件。  
ex: [Required]、[DefaultValue(false)] 
Model DataAnnotations  
[Column(TypeName = "nvarchar(24)")]  



## Error Exception :

- Failed to load API definition.
	- 解決方法 
      1. 檢查所有Action是否有明確指定[HttpGet]、[Http Post]
      2. 檢查是否有重複的return回傳物件   
- SwaggerGeneratorException: Actions require a unique method/path combination for Swagger/OpenAPI 3.0.
	- 解決方法
    	1. [Route("FullPath")]
    	2. [HttpGet("ActionName")]
    	3. [HttpPatch("GetTestBlog/")]
    	4. [Route("api/[controller]/[action]")]
- Deserialization of reference types without parameterless constructor is not supported. Type 
  - 解決方法
    - `Startup.cs` 加入 `services.AddControllers().AddNewtonsoftJson();`

# 安裝 EFCore SqlServer & Tools
安裝 dotnet-ef 全域工具 (.NET CLI Global Tool)  
	dotnet tool update -g dotnet-ef  
安裝 Entity Framework Core 相關套件  
	dotnet add package Microsoft.EntityFrameworkCore.SqlServer  
	dotnet add package Microsoft.EntityFrameworkCore.Tools  

正向工程 {Code First}  
	Add-Migration InitialCreate (Power Shell) 會建立 MyContextModelSnapshot.cs (目前 Model 的快照)  
	Update-Database  
	Remove-Migration  

逆向工程 {DB First}  
	1. dotnet tool install --global dotnet-ef  
	2. dotnet add package Microsoft.EntityFrameworkCore.Design  
	3. dotnet ef dbcontext scaffold <connection-string> <provider> --data-annotations 產出包含Annotations的Model  
	4. dotnet ef dbcontext scaffold "Server=(localdb)\MSSQLLocalDB;Database=Blog;Trusted_Connection=True;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer -o Models  
	5. dotnet build  

# 安裝 Postman
	建立Blog.cs、Post.cs模型，並使用Scaffold產出EFCore的API控制器，
	將Blog控制器的PostBlog動作方法return 替換為CreatedAtRoute方法(CreatedAtRoute(nameof(GetBlog), new { id = blog.BlogId }, blog);)
	並將GetBlog加上標籤[HttpGet("{id}", Name = nameof(GetBlog))]  
	
	隨後開啟Postman
	POST:http://localhost:{port}/api/Blogs 並輸入參數
	{
	  "url": "https://www.google.com.tw/",
	  "rating": 1
	}
	可以在Response的Headers處看見回應Location跳轉位址為http://localhost:{port}/api/Blogs/{id}
	並將該位址複製起來貼至新的Get請求網址，並Send看見剛剛輸入的那筆Post結果

# 安裝 Dapper 

PM> NuGet\Install-Package Dapper -Version 2.1.11

ref:  
- https://igouist.github.io/post/2021/05/newbie-3-dapper/
- https://dotblogs.com.tw/OldNick/2018/01/15/Dapper#Dapper%20-%20Transaction

### Dapper - Query:
1. Query
2. QueryFirstOrDefault 

### Dapper - Execute​
執行Insert、Update、Delete、Stored Procedure時使用。  
- Execute

>除上述列出三個常用方法外，其餘全部可參考Dapper.dll裡面的SqlMapper.cs主程式

### 資料型態 (DbTyoe)
參數一定要指定型態，否則會讓Dapper轉換為預設型態，導致查詢效能變差。  
ref: 
- https://learn.microsoft.com/zh-tw/dotnet/api/system.data.dbtype?view=net-7.0#system-data-dbtype-string  
- DbType.AnsiString
  - Varchar
- DbType.String
  - NVarchar
- DbType.AnsiStringFixedLength
  - Char
- DbType.StringFixedLength
  - NChar

### Memo  
Insert資料可改用Query語法執行，僅須完整 Insert SQL 語句的最後一行加上`@@IDENTITY` 或 `LAST_INSERT_ID()`，以及改用 int 變數接回`conn.Query<int>(sql, param)`結果即可實作

後續再來閱讀 
- BeginTransaction 開啟 SQL Transaction 交易包裝，可Try Catch包起來，若於Commit()發生例外可即時Rollback()
- 於Dapper實作批次新增/更新/刪除作業
  - 大量新增可用: 
    1. `StringBuilder`搭配`Foreach`將`List<Model>`組成純SQL語句後送出至`conn.Execute​(sql);`
    2. 也可這樣用但也許有效能影響 `conn.Execute​(sql, List<Model>);`
    3. 解決辦法將`List<Model>`物件用進 `List<DynamicParameters>` 裡面再送至`conn.Execute​(sql, new { id = List<DynamicParameters> });`即可
  - 大量更新/刪除可用:
    1. `conn.Execute​(sql, new { id = Array[] });` 等同於sql語句中的`where id in`寫法
    2. 若條件不僅有ID，還有其他欄位篩選且為大量資料，則`conn.Execute​(sql, new { id = List<DynamicParameters> });`
- 搭配Dapper使用LINQPad 快速產生相對映 SQL Command 查詢結果的 Model.cs 類別
  - https://kevintsengtw.blogspot.com/2015/10/dapper-linqpad-sql-command.html
  

# 加入NLog

ref: 
- 官網Github教學: https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-3
- 配置更多詳盡解說: https://github.com/NLog/NLog/wiki/Configuration-file
- 使用appsetting.json來讀取NLog配置: https://github.com/NLog/NLog.Extensions.Logging/wiki/NLog-configuration-with-appsettings.json
  
1. Nuget安裝 
   1. NLog.Web.AspNetCore
   2. NLog.Extensions.Logging
2. 在專案的根目錄中建立 nlog.config（全部小寫）檔案
   1. 修改靜態nlog.config屬性 建置動作為"有更新才複製"
   2. 修正其中的Log輸出目錄標籤`internalLogFile`至專案目錄
   3. 修改`<target>`標籤中的fileName屬性為`專案目錄\log\{fileName}.log`
3. 更新Program.cs
   1. using NLog;
   2. using NLog.Web;
4. 調整appsettings.json中的"LogLevel"裡面的"default"為"Trace" 
   - 否則logger.SetMinimumLevel()中的最低追蹤等級會被覆寫 
   - 包含appsettings中的 .dev/st/uat/prod 各環境有各種紀錄層級


- 符合規則條件(roles)的都將寫入(writeTo)相應的目標(target)
- 規則將由上至下按順序讀取至駔後一個 
- 於本例中 最後一條Role為`*`也就是:全部的`{NameSpace.ClassName}`產生的log都將符合條件
  
``` config
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance">

	<!--自定義變數-->
	<variable name="logDirectory" value="logs/${shortdate}"/>

	<targets>
		<!--輸出目標為實體檔案-->
		<target name="logfile" xsi:type="File" fileName="C:\Projects\MyBlog\${logDirectory}\MyBlog.txt"/>

		<!--輸出目標為控制台-->
		<target name="logconsole" xsi:type="Console" />
	</targets>
	
	<rules>
		<!--忽略所有非關鍵的Microsoft紀錄-->
		<logger name="Microsoft.*" maxlevel="Info" final="true" />

		<logger name="*" minlevel="Debug" writeTo="logfile" />
		<logger name="*" minlevel="Debug" writeTo="logconsole" />
	</rules>

</nlog> 
```

### Memo 
使用appsetting.json來讀取NLog配置，將放在後續回頭重構時再來實作

# 寫個BaseController把Response包成Headel和Body物件格式

- 可以直接物件出去讓前端自由操作
- 也可以Json出去由前端自行轉譯成JsObject: `JSON.parse('string')`

ref:
- Enum的描述`[Description("OK")]`屬性 https://www.ruyut.com/2022/09/csharp-enum-description.html

1. 先創建空的`BaseController.cs` 然後看是API控制器就繼承`ControllerBase`，若為MVC控制器就繼承`Controller`  
2. HTTP網路基礎: https://miahsuwork.medium.com/%E7%AC%AC%E5%85%AD%E9%80%B1-%E7%B6%B2%E8%B7%AF%E5%9F%BA%E7%A4%8E-http-request-response-7d7e0cb88ed8
3. API 基礎: https://miahsuwork.medium.com/%E7%AC%AC%E5%85%AD%E9%80%B1-api-%E5%9F%BA%E7%A4%8E-json-restful-curl-%E6%8C%87%E4%BB%A4-28670813764e
4. 包裝成統一格式實作參考: https://fullstackladder.dev/blog/2015/10/20/web-api-unify-response-message/
5. 包裝成統一格式實作參考: https://dotblogs.com.tw/jasonyah/2013/12/06/how-to-handle-webapi-response-and-handle-exception
6. 待研究1: https://dotblogs.azurewebsites.net/wellwind/2015/10/20/web-api-customize-response-1
7. 待研究2: https://www.dotblogs.com.tw/wellwind/2015/10/21/web-api-customize-response-2
8. 待研究3: https://www.dotblogs.com.tw/wellwind/2015/11/13/web-api-customize-response-3
9. 待研究4: https://dotblogs.com.tw/wellwind/2016/02/24/web-api-customize-response-4
10. 待研究5: https://dotblogs.com.tw/wellwind/2016/02/25/web-api-customize-response-5
11. JAVA的統一格式: https://www.tpisoftware.com/tpu/articleDetails/1717
12. C#的統一格式: https://ronsun.github.io/content/20190317-webapi-response-data-format.html
13. 簡單創建一個`ResponseBox.cs`模型，裡面屬性放`Header`與`Body`
   - 其中`Header`要再創建一個類別，屬性放`Message`和`StateCode`
   - `StateCode`型別為`enum` 結合上述ref: 提供的擷取`Attribute`的擴充方法，取得描述上的文字  
  
### ResponseBox.cs
``` C#
using MyBlog.Common.EnumExtenstion;
using static MyBlog.Common.Enums.BlogEnum;

namespace MyBlog.Models.Common
{
    public class ResponseBox<T>
    {
        public ResponseBox(T body, StateCode code = StateCode.OK)
        {
            Header.StateCode = code;
            Header.Message = code.GetDescription();
            Body = body;
        }

        public Header Header { get; set; } = new Header();

        public T Body { get; set; }
    }
}
```

### Header.cs
``` C#
using static MyBlog.Common.Enums.BlogEnum;

namespace MyBlog.Models.Common
{
    public class Header
    {
        public string Message { get; set; }
        public StateCode StateCode { get; set; }
    }
}
```
### StateCode.cs
``` C#
using System.ComponentModel;

namespace MyBlog.Common.Enums
{
    public class BlogEnum
    {
        public enum StateCode 
        {
            [Description("成功拿到資料拉")]
            OK = 200,

            [Description("發送失敗拉")]
            Fail = 404
        }
    }
}
```

### BaseController.cs
``` C#
using MyBlog.Models.Common;
using static MyBlog.Common.Enums.BlogEnum;

namespace MyBlog.Controllers
{    
    [ApiController]
    public class BaseController : ControllerBase
    {
        [NonAction]
        public ResponseBox<T> Done<T>(T body, StateCode code = StateCode.OK) 
        {
            return new ResponseBox<T>(body, code);
        }
    }
}

```

### BlogController.cs
``` C#
using MyBlog.Models.Common;
using static MyBlog.Common.Enums.BlogEnum;
// GET: api/Blogs
[HttpGet]
public ActionResult<ResponseBox<List<Blog>>> GetBlogs()
{
	_logger.LogInformation("Hello, this is the BlogList!");
	var service = new BlogService(_conn.ConnectionString);
	var blogs = service.GetBlogs();

	return Done(blogs, StateCode.OK);
}
```
## 前端HTML接收應用

``` JavaScript
() => {
	$.ajax({
		type: 'POST',
		url: '@Url.Action("Action","Controller")',
		data: $.param({
			'name':'value',
			'name':'value'
		}),
		success: result => {
			if (result.header.StateCode === '200')
				something...
			else
				alert(result.header.Message);
		}
	})
}
```

### Memo
在一些前端$.post中傳遞的data多為物件格式，其中又有陣列等型別，此時就要用`jQuery.param()`建立適合在Post傳遞中的JQuery物件，否則會傳遞成奇形怪狀的字串傳遞。  
  官方文檔: https://api.jquery.com/jquery.param/

MVC中的JSON編/解碼器
1. System.Text.Json
   - 內建 ASP.NET Core 3以上推出；編碼效能較`Newtonsoft.Json`好，但功能卻較少。
2. Newtonsoft.Json
   - 內建
3. Utf8.Json
   - 第三方高效能套件 於編/解碼效能高於`System.Text.Json` 與`jil`相比編碼勝過它。
4. jil
   - 第三方高效能套件 於編/解碼效能高於`System.Text.Json` 與`Utf8.Json`相比解碼勝過它。
5. Controller.Json()
   - 僅Controller
6. IJsonHelper
   - 僅Razor View

若要再Razor View前台的JS使用，須搭配`@Html.Raw(jsonStr);`表示做HTML編碼，即可再JS正常使用它。

## 建立DI註冊器

1. 一般註冊模式
2. 將相關註冊移至擴充方法註冊
3. 套件Autofac 
   - https://blog.darkthread.net/blog/autofac-notes-1/ 
4. 檔名尾巴Service
5. 屬性?

ref:
- 架構原則 這篇要讀: https://learn.microsoft.com/zh-tw/dotnet/architecture/modern-web-apps-azure/architectural-principles#dependency-inversion
- 看看別人怎麼做: https://blog.darkthread.net/blog/aspnet-core-di-notes/
- 看看別人怎麼做: https://mao-code.github.io/posts/3588979794/
- 看看Steve Gordon的解釋: https://www.stevejgordon.co.uk/aspnet-core-dependency-injection-what-is-the-iservicecollection 
- 有教學: https://www.youtube.com/watch?v=g5YixtianHo

### 基本認識

1. Singleton(單例): 
   - **從程式開始到結束**，只建立一個實體，每次都重複利用同一個，直到程式被終止。  
      - 優: 適合不做變動的配置檔註冊
      - 缺: 不適合處理併發請求
2. Scoped(作用域): 
   - **每次的Request**，都建立一個新的實體，同一個Request下，重複利用同一個實體 (這裡的Request 常指Http Request)。  
3. Transient(瞬態)  : 
   - **每次注入時**，都建立一個新的實體。
     - 優: 適合處理併發請求
4. 從控制器存取應用程式或組態設定是常見的模式。 [ASP.NET Core 選項模式][1]中所述的選項模式是管理設定的慣用方法。 一般而言，不要將 IConfiguration 直接插入至控制器。 
5. [SOLID][2] 原則參考1
6. SOLID 原則Config的選項模式實作參考 [https://juldhais.net/implementing-options-pattern-to-read-the-configuration-in-appsettings-json-file-e80fa5bdc111]
7. 看看別人怎麼做: https://www.youtube.com/watch?v=DQxGDFZn_6Y&list=PLneJIGUTIItsqHp_8AbKWb7gyWDZ6pQyz&index=55

> 有時間可多看看 `lock`方法與`volatile`。

### 將相關註冊移至擴充方法註冊

於專案開發中後期會有越來越多的`Service`會需要在`StartUp.cs`中注入，為了保持`StartUp.cs`的整潔會需要將大量服務註冊的`Function`移至擴充方法註冊

於專案中Create Folder "Extensions" 目錄

ref:  
1. 官方文檔: https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-3.1

RegisterDIConfig.cs
``` C#
public static class RegisterDIConfig
{
    public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
    {
        services.Configure<AppSettingsOptions>(config.GetSection("AppSettings"));

        return services;
    }

    public static IServiceCollection AddRegisterDIConfig(this IServiceCollection services) 
    {
        /* 這裡要注意 當商業邏輯相依於服務底層時，生命週期長的不可相依短的，
          * 儘管真的發生實際在執行時，短的也會變為跟長的相同的生命週期。*/
        services.AddScoped<IServicesBase, ServicesBase>();
        services.AddScoped<IBlogService, BlogService>();
        services.AddScoped<IPay, PayMoney1Service>();
        services.AddScoped<IPay, PayMoney2Service>();

        // 嘗試註冊至DI Container如未存在則Create，反之忽略。
        // services.TryAddSingleton<IBlogService, BlogService>();

        return services;
    }
}
```

Startup.cs
``` C#
public void ConfigureServices(IServiceCollection services)
{
    // DI 相關註冊群組移至擴充方法
    services.AddConfig(Configuration)
            .AddRegisterDIConfig();
    ...
}
```

### 使用批次註冊

這邊要注意的是我使用的是類別名稱`Name.Contains("Service")`包含特定字串判斷，而非使用`Name.EndsWith("Service")`結尾判斷，所以要小心檔名命名問題。

ref:
1. 官方文檔1: https://learn.microsoft.com/zh-tw/dotnet/api/system.reflection.assembly?view=net-7.0
2. 官方文檔2: https://learn.microsoft.com/zh-tw/dotnet/api/system.reflection.assembly.getexecutingassembly?view=net-7.0
3. 以及其他網路上教學

``` C#
public static IServiceCollection AddBatchRegisterDIConfig(this IServiceCollection services)
{
    var Types = Assembly.GetExecutingAssembly().GetTypes().ToList();
    var Services = Types.Where(m => m.IsPublic && m.IsClass && !m.IsAbstract && m.Name.Contains("Service"));
    
    foreach (var Service in Services) 
    {
        Type IService = Service.GetInterfaces().FirstOrDefault();
        services.AddScoped(IService, Service);
    }

    return services;
}
```


### 在同一個interface注入多個Service並依照Request時傳入不同的Type決定實作哪一個Service.cs

Startup.cs
``` C#
public void ConfigureServices(IServiceCollection services)
{    
    services.AddSingleton<IPay, PayMoney1Service>();
    services.AddSingleton<IPay, PayMoney2Service>();
}
```
PayMoney1Service.cs
``` C#
public class PayMoney1Service : IPay
{
    public string Type => "I Am Money One.";

    public PayMoney Pay()
    {
        var obj = new PayMoney() 
        {
            Cost = 123,
            PayOwner = "One"
        };            

        return obj;
    }
}
```

PayMoney2Service.cs
``` C#
public class PayMoney2Service : IPay
{
    public string Type => "I Am Money Two.";

    public PayMoney Pay()
    {
        var obj = new PayMoney()
        {
            Cost = 321,
            PayOwner = "Two"
        };
        return obj;
    }
}
```

PayController.cs
``` C#
protected IEnumerable<IPay> _pay { get; set; }

public PayController(IEnumerable<IPay> pay) 
{
    _pay = pay;            
}

// POST: api/PayMoney
[HttpPost]
public ActionResult<ResponseBox<PayMoney>> PayMoney(string PayMode)
{
    var PostNeedCost = new PayMoney();
    if (PayMode == "I Am Money One.")
    {
        PostNeedCost = _pay.Where(m => m.Type.Equals(PayMode)).Single().Pay();
    }
    else
    {
        PostNeedCost = _pay.Where(m => m.Type.Equals("I Am Money Two.")).Single().Pay();
    }

    return Done(PostNeedCost);
}
```
成功依照所傳入Type取回資料
``` Json
{
  "header": {
    "message": "成功拿到資料拉",
    "stateCode": 200
  },
  "body": {
    "cost": 123,
    "payOwner": "One"
  }
}
```
第二種Type
``` Json
{
  "header": {
    "message": "成功拿到資料拉",
    "stateCode": 200
  },
  "body": {
    "cost": 321,
    "payOwner": "Two"
  }
}
```

### 避免實例化相依性注入 (待研究)

待閱文章:  
1. Resolving instances with ASP.NET Core DI from within ConfigureServices

ref:  
- https://stackoverflow.com/questions/32459670/resolving-instances-with-asp-net-core-di-from-within-configureservices


> 其餘後面再來研究

## 加入IConfigService.cs
透過它來讀取Appsetting.json以及其他ConnectionString，或者如郵件SMTP設定或特殊功能的規格屬性等。

假使今天專案不僅連線一個資料庫且連線提供者也不同，則我們可以加入一個自訂介面服務再開一些屬性，加入`Enum` 用來判斷傳入SQL要使用什麼連線提供者，然後搭配`IDbConnection`，並修改底層實作出對不同資料庫連線進行CRUD動作。

appsettings.json
``` Json
"ConnectionStrings": {
  "BloggingContext": "Server=(localdb)\\mssqllocaldb; Database=Blog; Trusted_Connection=True; MultipleActiveResultSets=true",
  "DB2": "Provider=OraOLEDB.DB2;Data Source=MyDB2DB;User Id=myUsername;Password=myPassword"
},
"AppSettings": {
  "Author": "Ruiwen",
  "WebSite": "https://github.com/Ruiwen523/MyBlog"
}
```
ConfigService.cs
```C# 
public class ConfigService : IConfigService
{
    private readonly IConfiguration _configuration;
    private readonly AppSettings _options;

    public ConfigService(IConfiguration configuration,
                          IOptions<AppSettings> options)
    {
        _configuration = configuration;
        _options = options.Value;
    }

    public string SqlServerBlog { get => _configuration.GetConnectionString("BloggingContext"); }

    public string DB2Blog { get => _configuration.GetConnectionString("DB2"); }

    //public AppSettings appSettings { get => _configuration.GetValue<AppSettings>("AppSettings"); }

    public AppSettings appSettings { get => _options; }
}
```

DbSource.cs
``` C#
public enum DbSource 
{
    SQLServer,
    DB2
}
```
BlogService.cs
``` C#
/// <summary>
/// 取得資料表清單
/// </summary>
public List<Blog> GetBlogs()
{
    var sql = @"select * from Blogs";

    return _services.Query<Blog>(sql, null, DbSource.SQLServer);
}
```

ServicesBase.cs
``` C#
IDbConnection ChoseDbSorce(DbSource dbSource) => 
dbSource switch
{
    DbSource.SQLServer => new SqlConnection(_config.SqlServerBlog),
    _ => new OleDbConnection(_config.DB2Blog)

};

/// <summary>
/// 查詢清單資料
/// </summary>
public List<T> Query<T>(string sql, DynamicParameters dynamic = null, DbSource dbSource = DbSource.SQLServer)
{
    using (IDbConnection conn = ChoseDbSorce(dbSource))
    {
        return conn.Query<T>(sql, dynamic, null, true, 30, CommandType.Text).ToList();
    }
}
```

## Appsetting.{environment}.json

.NET Core 會依據`ASPNETCORE_ENVIRONMENT`設定的環境變數不同，而讀取不同的Server環境變數設定檔，最直接是Log Level與ConnectionString，一些API的設定也會不同等。  
ref:
1. 黑暗大這篇應該就夠了: https://blog.darkthread.net/blog/appsetting-by-environment/
2. https://marcus116.blogspot.com/2019/04/netcore-aspnet-core-appsettingsjson.html
3. https://stackoverflow.com/questions/52085806/what-is-the-difference-between-iwebhost-webhostbuilder-buildwebhost
4. https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1#providers

## 加入CORS 跨來源資源共用
ref:
1. MDN文件: https://developer.mozilla.org/zh-TW/docs/Web/HTTP/CORS
2. 官方文件: https://learn.microsoft.com/zh-tw/aspnet/core/security/cors?view=aspnetcore-3.1

Lading...

## ASP.NET Core 中的路由
ref:
1. https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/routing?view=aspnetcore-3.1
2. https://learn.microsoft.com/zh-tw/aspnet/core/mvc/controllers/routing?view=aspnetcore-3.1
3. https://coolmandiary.blogspot.com/2021/09/net-core-web-api09web-api.html
4. https://jovepater.com/article/asp-net-core-5-ep-12-routing-controller/
5. https://ithelp.ithome.com.tw/articles/10243332

## 身分驗證篇
ref:
1. 官方文件概觀: https://learn.microsoft.com/zh-tw/aspnet/core/security/?view=aspnetcore-3.1
2. ASP.NET Core 上的 Identity 簡介: https://learn.microsoft.com/zh-tw/aspnet/core/security/authentication/identity?view=aspnetcore-3.1&tabs=visual-studio
3. Cookie: https://learn.microsoft.com/zh-tw/aspnet/core/security/authentication/cookie?view=aspnetcore-3.1
4. 這邊有人家的實作: https://www.youtube.com/watch?v=7vhKul20pbs&list=PLneJIGUTIItsqHp_8AbKWb7gyWDZ6pQyz&index=66&t=49s
5. 認識 OAuth 2.0 : https://medium.com/%E9%BA%A5%E5%85%8B%E7%9A%84%E5%8D%8A%E8%B7%AF%E5%87%BA%E5%AE%B6%E7%AD%86%E8%A8%98/%E7%AD%86%E8%A8%98-%E8%AA%8D%E8%AD%98-oauth-2-0-%E4%B8%80%E6%AC%A1%E4%BA%86%E8%A7%A3%E5%90%84%E8%A7%92%E8%89%B2-%E5%90%84%E9%A1%9E%E5%9E%8B%E6%B5%81%E7%A8%8B%E7%9A%84%E5%B7%AE%E7%95%B0-c42da83a6015
6. 認識 Token-based Authentication: https://vicxu.medium.com/%E6%B7%BA%E8%AB%87-authentication-%E4%B8%AD%E9%9B%86-token-based-authentication-90139fbcb897
7. 認識 Token Authentication : https://ms0680146.medium.com/jwt-introduction-b81cb99c93db 
8. 保哥JWT: https://blog.miniasp.com/post/2022/02/13/How-to-use-JWT-token-based-auth-in-aspnet-core-60

### Cookie驗證
1. 在`ConfigureServices`方法中，使用`AddAuthentication`和`AddCookie`方法來建立驗證中介軟體服務：
2. 在`Configure`方法中，使用`UseAuthentication`和`UseAuthorization`方法來使用身分驗證與授權中介軟體服務：

Startup.cs  
```C#
using Microsoft.AspNetCore.Authentication.Cookies;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        #region 建立Cookie驗證Identity
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(option => 
                {
                    option.ExpireTimeSpan = TimeSpan.FromMinutes(20); // 過期時間
                    option.LoginPath = new PathString("/api/Login/NoLogin"); // 未登入時導頁
                });
        #endregion
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        ...

        // 使用身分驗證
        app.UseAuthentication();
        // 使用身分授權
        app.UseAuthorization();
        // 必須添加在使用端點之前
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
```

3. 於`LoginController`新增`Login`、`Logout`、`NoLogin`、`NeedLogin`等Action動作方法
4. 並於`Login`方法的驗證成功後 宣告`ClaimsIdentity`物件設定後，指派給`HttpContext.SignInAsync(參數1, 2...)`進行登入動作
5. 驗證相關屬性設定可於`Startup.cs`中的`AddCookie(option => { option...});`內進行全域設定，否則會以`AuthenticationProperties`類別相同設定取代`Startup`設定，例如使用SSO登入過期時間15分鐘，使用系統登入20分鐘。
6. 於想要進行驗證的相關`Action`或`Controller`層級加上`[Authorize]`標籤，並引入命名空間 `Microsoft.AspNetCore.Authorization;` 

LoginController.cs
```C#
[HttpPost]
public async Task<ActionResult<ResponseBox<Empty>>> LoginAsync(RequestBox<LoginUser> userData)
{
    var user = _dbContext.Users.AsNoTracking()
                                .ToListAsync().Result
                                .SingleOrDefault(m => m.Account.Equals(userData.Body.UserId) && m.Password.Equals(userData.Body.Mima));
    if (user == null)
    {
        return Content("帳號密碼錯誤");
    }
    else 
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Account),
            new Claim("FullName", user.Name),
            new Claim(ClaimTypes.Role, "Administrator"),
            //new Claim("LastChanged", {Database Value})
        };

        var claimsIdentity = new ClaimsIdentity(
            claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // 驗證相關屬性設定
        // var authProperties = new AuthenticationProperties
        // {
        //     允許刷新身份驗證會話(Session)。
        //     AllowRefresh = true,
        //     身份驗證票證過期的時間。 此處設定的值會覆寫使用 AddCookie 設定的 CookieAuthenticationOptions 的 ExpireTimeSpan 選項。
        //     ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(20),
        //     身份驗證會話是否在多個請求中持續存在。 與 cookie 一起使用時，控制 cookie 的生命週期是絕對的（與身分驗證票證的生命週期相符）還是基於會話。
        //     IsPersistent = true,
        //     核發身分驗證票證的時間
        //     IssuedUtc = DateTimeOffset.UtcNow.AddMinutes(10),
        //     用作HTTP的完整路徑重定向轉址
        //     RedirectUri = "http://www.google.com.tw/"
        // };

        // SignInAsync 會建立加密的 cookie ，並將它新增至目前的回應。
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
        // await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
    }

    return Done<Empty>(null, StateCode.OK);
}

[HttpPost]
public async Task<ActionResult<ResponseBox<Empty>>> Logout()
{
    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

    return Done<Empty>(null, StateCode.OK);
}


[HttpGet]
public string NoLogin()
{
    return "未登入";
}

[Authorize]
[HttpGet]
public string NeedLogin()
{
    return "需要登入才能回傳否則失敗";
}
```

7. 或者全部都需要驗證則至`Startup.cs`中添加`services.AddMvc()`，並於`Login`的`Action`上添加`[AllowAnonymous]`允許匿名登入，表示該動作方法不需驗證。

Startup.cs
``` C#
public void ConfigureServices(IServiceCollection services)
{
    services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(option =>
        {
            option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            option.LoginPath = new PathString("/api/Login/NoLogin"); // 未登入時導頁
        });

    // 添加全域Filter驗證
    services.AddMvc(options =>
    {
        options.Filters.Add(new AuthorizeFilter());
    });
}
```

LoginController.cs
``` C#
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class LoginController : BaseController
    {
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<ResponseBox<Empty>>> LoginAsync(RequestBox<LoginUser> userData)
        {
            var user = _dbContext.Users.AsNoTracking()
                                       .ToListAsync().Result
                                       .SingleOrDefault(m => m.Account.Equals(userData.Body.UserId) && m.Password.Equals(userData.Body.Mima));
            if (user == null)
            {
                return Content("帳號密碼錯誤");
            }
            else 
            {
              ...驗證方法放在這
            }

            return Done<Empty>(null, StateCode.OK);
        }
    }
```

8. 結合`Roles`設定來驗證是否有該`Action`權限
    1. 假定已於Startup.cs添加全域權限驗證並於相關`Action`方法進行`Roles`測試是否能成功取得回傳資料。
    2. 修改Login驗證方法，將登入`Account`關聯的`Role`全部撈出，並於登入`SignInAsync()`前指派給`ClaimsIdentity`物件
    3. 並於DB新增`User`與關聯`Role`資料表

LoginController.cs
``` C#

[Route("api/[controller]/[action]")]
[ApiController]
public class LoginController : BaseController
{
    [AllowAnonymous]
    [HttpPost]
    public async Task<ActionResult<ResponseBox<Empty>>> LoginAsync(RequestBox<LoginUser> userData)
    {
        var user = _dbContext.Users.AsNoTracking()
                                    .ToListAsync().Result
                                    .SingleOrDefault(m => m.Account.Equals(userData.Body.UserId) && m.Password.Equals(userData.Body.Mima));
        if (user == null)
        {
            return Content("帳號密碼錯誤");
        }
        else 
        {
            ...

            var role = from r in _dbContext.roleRlAccounts
                       where r.Account == user.Account
                       select r.RoleCode;

            foreach (var r in role) 
            {
                claims.Add(new Claim(ClaimTypes.Role, r));
            }

            ...
        }

        return Done<Empty>(null, StateCode.OK);
    }


    [HttpGet]
    [AllowAnonymous]
    public string NoLogin()
    {
        return "未登入";
    }

    [HttpGet]
    public string NeedLogin()
    {
        return "登入成功";
    }

    [HttpGet]
    public ActionResult<ResponseBox<Empty>> NoAccess()
    {
        return Done<Empty>(null, StateCode.NoAccess);
    }

    [HttpGet]
    [Authorize(Roles = "Access")]
    public string NeedAccess() 
    {
        return "有登入且有授權";
    }

    [HttpGet]
    [Authorize(Roles = "Access2")]
    public string NeedAccess2()
    {
        return "有登入且有授權";
    }
}
```
使用者資料表 及 對應關連角色 表  
![使用者資料表](./截圖/使用者資料表1.png)
![圖片介紹](./截圖/關聯角色資料表.png)


9. 依據官方文件 [存取HttpContext][4]介紹，從`Service`取得`HttpContext`，並取出當前登入使用者的`Account`作為後續，CRUD資料時的變更人員。  
   1.  `Startup`注入`services.AddHttpContextAccessor();`相依性，後端即可於`Controller`以外建構式注入後取用。
   2.  調整`User`資料表加入創建者、創建日期、修改者、上次修改日期等欄位 ![使用者資料表](./截圖/使用者資料表2.png)  
   3.  從前台登入並Post資料至  
       ![創建使用者Request](./截圖/創建使用者Request.png)
   4.  從Service取得IHttpContextAccessor，並取出登入使用者資料
       
SystemController.cs
``` C#
public class SystemController : BaseController
{
    private protected BloggingContext _context;
    private protected ILogger _logger;
    private protected ISystemService _systemService;

    public SystemController(BloggingContext bloggingContext, ILogger<SystemController> logger, ISystemService systemService) 
    {
        _context = bloggingContext;
        _logger = logger;
        _systemService = systemService;
    }

    /// <summary>
    /// 新增使用者
    /// </summary>
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public ActionResult<ResponseBox<User>> CreateUser(User user)
    {
        var model = _systemService.AddUser(user);

        if (model != null)
            _logger.LogInformation(string.Format("寫入User成功 名稱: {0}", user.Name));

        return Done(model, StateCode.OK);
    }
}
```

SystemService.cs
``` C#
public class SystemService : ISystemService
{
    private readonly BloggingContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SystemService(BloggingContext context,
                            IHttpContextAccessor httpContextAccessor) 
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public User AddUser(User user)
    {   
        // var name = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(m => m.Type.Equals("FullName"))?.Value;     
        user.Creator = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(m => m.Type.Equals(ClaimTypes.Name))?.Value;
        user.CreateDate = DateTime.Now;
        user.LastModifyDate = DateTime.Now;
        user.LastModifior = "";

        _context.Users.Add(user);
        _context.SaveChanges();

        return _context.Users.SingleOrDefault(m => m.Account.Equals(user.Account));
    }

    public User GetLoginUser()
    {
        var Account = _httpContextAccessor.HttpContext.User.Claims.SingleOrDefault(u => u.Type == ClaimTypes.Name)?.Value;
        var user = _context.Users.SingleOrDefault(m => m.Account.Equals(Account));

        return user;
    }    
}
```

> IsPersistent = true, // 要測一下該屬性是否為；瀏覽器全部關閉才觸發事件，而只關閉索引頁籤也不會執行瀏覽器關閉事件(登出)。  
> 後續有空可嘗試複寫其驗證方法[參考官方文檔 覆寫CookieAuthenticationEvents][3]



## JWT驗證

ref:  
   1. 保哥文章: https://blog.miniasp.com/post/2022/02/13/How-to-use-JWT-token-based-auth-in-aspnet-core-60
   2. 有教學: https://www.youtube.com/watch?v=3LV0JAZbORQ&list=PLneJIGUTIItsqHp_8AbKWb7gyWDZ6pQyz&index=69
   3. 麥克半路出家: https://medium.com/%E9%BA%A5%E5%85%8B%E7%9A%84%E5%8D%8A%E8%B7%AF%E5%87%BA%E5%AE%B6%E7%AD%86%E8%A8%98/%E7%AD%86%E8%A8%98-%E9%80%8F%E9%81%8E-jwt-%E5%AF%A6%E4%BD%9C%E9%A9%97%E8%AD%89%E6%A9%9F%E5%88%B6-2e64d72594f8

### Error Exception :
   - JWT error IDX10634: Unable to create the SignatureProvider C#
      1.  https://stackoverflow.com/questions/49875167/jwt-error-idx10634-unable-to-create-the-signatureprovider-c-sharp
      2.  須為對稱加密演算法
      3.  密鑰必須至少有32 個字元。 
   -  www-authenticate: Bearer error="invaild_token", error_description="The audience 'empty' is invalid"
      1. 如果遇到該Error則檢查`Startup.cs`中的驗證項目設定，並檢查`LoginController.cs`中的`JwtSecurityToken`物件是否有設定好
      2. stack overflow: https://stackoverflow.com/questions/64498872/net-core-3-1-bearer-error-invalid-token-error-description-the-audience-emp

1. 添加 JWT 基本配置
2. 安裝套件`Microsoft.AspNetCore.Authentication.JwtBearer Version="3.1.32"`
3. 於`Startup.cs` 加入 JWT 驗證機制，並獨立至`RegisterDIConfig.cs`擴充方法中。

AppSetting.json
``` Json
  "Security": {
    "JWT": {
      "KEY": "BE8DF1F28C0ABC85A0ED0C6860E5D832", // MD5 {Blog}
      "Issur": "Ruiwen", // 發行者
      "Audience": "Other" // 發給誰
    } 
  }
```

Startup.cs & RegisterDIConfig.cs
``` C#
public void ConfigureServices(IServiceCollection services)
{
    // 構建 JWT 驗證設定
    services.AddJwtAuthorize(Configuration);
}

public static class RegisterDIConfig
{
    /// <summary>
    /// 這是 JWT 驗證方法
    /// </summary>
    public static IServiceCollection AddJwtAuthorize(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true, // 需要驗證發行者
                        ValidIssuer = config["Security:JWT:Issur"],
                        ValidateAudience = true, // 需要驗證發給誰
                        ValidAudience = config["Security:JWT:Audience"],
                        ValidateLifetime = true, // 驗證生命週期 (預設本身就是True)
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Security:JWT:KEY"]))
                    };
                });

        return services;
    }
}
```

LoginController.cs
``` C#
[AllowAnonymous]
[HttpPost]
public async Task<ActionResult<ResponseBox<Token>>> JWTLogin(RequestBox<LoginUser> userData)
{
    var user = (await _dbContext.Users.AsNoTracking().ToListAsync())
                                        .SingleOrDefault(m => m.Account.Equals(userData.Body.UserId) && m.Password.Equals(userData.Body.Mima));            

    if (user == null)
    {
        return Content("帳號密碼錯誤");
    }
    else
    {
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Email, user.Account),
            new Claim("FullName", user.Name),
            new Claim(JwtRegisteredClaimNames.NameId, user.Name),
        };

        // 若今日登入帳號具有多角色權限則，則撈取後同時添加進來
        var role = from r in _dbContext.roleRlAccounts
                    where r.Account == user.Account
                    select r.RoleCode;

        foreach (var r in role)
        {
            claims.Add(new Claim(ClaimTypes.Role, r));
        }

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        // 安全金鑰
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configService.Security.JWT.KEY));

        // JWT設定
        var jwt = new JwtSecurityToken
        (
            issuer: _configService.Security.JWT.Issur,
            audience: _configService.Security.JWT.Audience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(30), // 過期時限

            // 對稱加密演算法:   SecurityAlgorithms.HmacSha256
            // 非對稱加密演算法: new RsaSecurityKey(_rsa), SecurityAlgorithms.RsaSha256Signature
            signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
        );

        // 產生Token (依照 JWT 設定產出相應Token)
        var token = new JwtSecurityTokenHandler().WriteToken(jwt);

        var Result = new Token() { token = token };
        
        return Done(Result, StateCode.Login);
    }
}
```

Result Token
``` Json
{
  "header": {
    "message": "登入成功",
    "stateCode": "Login"
  },
  "body": {
    "token": "eyJhbGciOiJodHRwOi8vd3d3LnczLm9yZy8yMDAxLzA0L3htbGRzaWctbW9yZSNobWFjLXNoYTI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6IlJ1aXdlbjUyM0BnbWFpbC5jb20iLCJGdWxlbiIsIm5hbWVpZCI6IlJ1aXdlbiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJBZG1pbiIsIkFjY2VzcyIsIkFjY2VzczIiXSwiZXhwIjoxNjk4ODUwOTIwLCJpc3MiOiJCRThERjFGMjhDMEFCQzg1QTBFRDBDNjg2MEU1RDgzMiJ9.tjD1wKwNRHEY9P8dPw5xde_9iOHoVsEbHGeNdfG8srM"
  }
}
```

Postman  
![JWTLogin](./截圖/Postman1.png)  

於Headers 加入 Bearer {Token} 即可常是呼叫相關`Action`
![NeedLogin](./截圖/NeedLogin.png)

或者在旁邊的`Authorization`選擇Type: Bearer Token 緊接著貼上{Token}即可查詢
![NeedLogin2](./截圖/NeedLogin2.png)


LoginController.cs 成功登入後的取值方法 `GetJWT_Type()`
``` C# 
[HttpGet]
[Authorize(Roles = "Access")]
public string GetJWT_Type()
{
    var Claims = HttpContext.User.Claims.ToList();
    var FullName1 = Claims.Where(m => m.Type.Equals("FullName")).First().Value;
    var FullName2 = HttpContext.User.FindFirstValue("FullName");
    var Email = HttpContext.User.FindFirstValue(ClaimTypes.Email);

    return FullName2;
}
```

可將 Token 放置到該 [JWT.IO](https://jwt.io/) 解析出對應 Value 玩玩看原理 
![JWT_IO_WEB](截圖/JWT_IO2.png)

### Memo
> `var claims = new List<Claim>()` 宣告出來的屬性`Type`可在後續程式運作中從 HttpContext.User 物件中取出使用。  
> `var jwt = new JwtSecurityToken()`這裡會影響到產出的`Token`值，後續驗證Token時，若有將`ValidateIssuer`與`ValidateAudience`設為True，則該方法必填入發行者(`issuer: _configService.Security.JWT.Issur`)&授予者(`audience: _configService.Security.JWT.Audience`)  
> `ClockSkew = TimeSpan.Zero`用來設定`expires: DateTime.Now.AddMinutes(30)`，一旦超過時間過期就直接回傳401而非偏移還能繼續使用。
> `appsetting.json`金鑰絕不能洩漏，否則會被竄改直接添加Role，使駭客有權限讀取伺服器的任意Action。
> ![JWT_IO](./截圖/JWT_IO.png)  
> JWT沒有辦法做登入登出動作，僅能依靠設定的期限到期才算結束。


未完!!!  
Lading...

## 非同步觀念

Lading...

## 自定義擴充[ValidationAttribute] 應用實作

1. 可重用的Regex驗證 比如整數+小數位數 對應DB`decimal(8, 4)`長度 
2. 字串的英數字編碼、加上Regex與相關邏輯去把非中文擷取出來比對後，僅將 屬性成員中非中文部分進行編碼

Lading... 

## ActionFilter 應用實作
1. 如果今天有人知道 Get/Post等 路由除了經過Token等有效驗證外，應還要檢查是否有該Controller/Action權限。
2. 假使今天有個控制器都是在做查詢類的情境，那可能會有完全共用的輸出(Component View)，其所需的Response Model物件肯定也一樣，此時就可以使用，但這總感覺不太對[Filter]味，待研究`[ActionFilter]`實務上究竟是如還運用。
3. 資料前處理(檢核)
4. 後處理應該是包裝送出?

> 註冊全域Filter Mvc.Add(Factory)  
> 可嘗試複寫其驗證方法 `CookieAuthenticationEvents`


Lading... 

## Middleware 中介軟體
ref:
  1. https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1

Lading... 

## 找找DbContext的Base層大家會怎麼去實作並應用 (後面再研究)
1. 有可能在Query或其他SaveChange()時，跑去寫一行Log或將所執行的動作回寫至DB特定的紀錄表 這類應用

Lading... 

## API的Body於Request時，會夾帶 FileInfo/物件 等請求 (待研究一下)

Lading... 


## 效能驗證方式
1. Dapper 查詢
2. EFCore 新增/刪除/修改

> 得要找一些三方套件或是人家怎麼去驗證自己寫的程式效能好不好的方式

Lading... 

## 快取功能 (待研究)
研究 Redis

ref: 
1. https://learn.microsoft.com/zh-tw/aspnet/core/performance/caching/memory?view=aspnetcore-3.1

## 壓力測試 (待研究)

建好之後來玩  

ref: 
- https://learn.microsoft.com/zh-tw/aspnet/core/test/load-tests?view=aspnetcore-3.1
- https://blog.twjoin.com/%E6%8B%93%E5%B1%95%E6%8A%80%E8%83%BD%E6%A8%B9%E4%B9%8B%E5%A3%93%E5%8A%9B%E6%B8%AC%E8%A9%A6-stress-test-%E7%AF%87-59b3d184b804

## Clean Code、Clean Architecture
ref:
- https://juldhais.net/clean-architecture-in-asp-net-core-web-api-4e5ef0b96f99
- https://medium.com/@nile.bits/clean-code-in-c-a-guide-to-writing-elegant-net-applications-db8698c8c731


## C# 物件導向 (OO) (待研究)
ref :
1. https://medium.com/@newpage0720/c-%E5%AD%B8%E7%BF%92%E7%AD%86%E8%A8%98-%E7%89%A9%E4%BB%B6%E5%B0%8E%E5%90%91%E5%AD%B8%E7%BF%92-73bd4ddb0fa8
2. https://coreychen71.github.io/posts/2019-10/oop/
3. https://www.w3schools.com/cs/cs_oop.php
4. https://break0344.medium.com/principles-of-object-oriented-design-d6f126d57acc
5. 

Lading... 

## 設計模式 (待研究)

1. DDD
2. TDD
3. CQRS 
4. https://medium.com/@gustavorestani/the-most-used-design-patterns-in-net-development-80d76f9fb6b

Lading... 

## 單元xUnit測試 (待研究)

Lading... 

## 其餘前端相關補充

未來移至CSS學習篇:
1. CSS相關: https://www.youtube.com/watch?v=s3ifqDB3dLc

Lading... 



## 延伸閱讀
### GIT
- Cherry-Pick 挑Commit提交
- Rebase 修改特定Commit描述
- 合併多Commit成一個Commit

Lading... 

### C#
- C#基礎教學 https://xianlee.gitbooks.io/csharp-basic/content/index.html#

### 其他 MVC 教學
- https://blog.hungwin.com.tw/aspnet-core-mvc-webapi-client/











[1]: <https://learn.microsoft.com/zh-tw/aspnet/core/mvc/controllers/dependency-injection?view=aspnetcore-3.1> "ASP.NET Core 中的選項模式"
[2]: <https://oldmo860617.medium.com/%E6%9C%9D%E6%9B%B4%E5%A5%BD%E7%9A%84-ooc-%E8%B5%B0%E5%8E%BB-ioc-%E6%8E%A7%E5%88%B6%E5%8F%8D%E8%BD%89%E8%88%87-di-%E4%BE%9D%E8%B3%B4%E6%B3%A8%E5%85%A5-b7fed15ff058> "SOLID"
[3]: https://learn.microsoft.com/zh-tw/aspnet/core/security/authentication/cookie?view=aspnetcore-3.1
[4]: https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/http-context?view=aspnetcore-3.1
