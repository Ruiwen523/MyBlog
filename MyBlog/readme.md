Use Ctrl+T quick search file and then use Alt+\ search membert method
ex: 搜尋"Starup.cs"然後輸入"ConfigureServices"

# 安裝 Swagger套件
ref:   
 a. https://github.com/domaindrivendev/Swashbuckle.AspNetCore  
 b. https://dotblogs.com.tw/xinyikao/2021/07/04/183326  
 c. https://dotblogs.com.tw/yc421206/2022/03/12/via_swashbuckle_write_swagger_doc_in_asp_net_core_web_api  

T: Startup.cs  
M: ConfigureServices()  
	add: services.AddSwaggerGen(); 以及定義 c.SwaggerDoc();swagger文件  
	add: 安裝Swashbuckle.AspNetCore.Newtonsoft //services.AddSwaggerGenNewtonsoftSupport(); //   

啟用瀏覽器並查看(預設使用以下url)  
/swagger/{documentName}/swagger.json => "v1"  
/swagger/  

命名空間中找到 System.ComponentModel.DataAnnotations 的屬性標記模型，以協助驅動 Swagger UI 元件。  
ex: [Required]、[DefaultValue(false)] 
Model DataAnnotations  
[Column(TypeName = "nvarchar(24)")]  



## Error Msg :

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

2. 簡單創建一個`ResponseBox.cs`模型，裡面屬性放`Header`與`Body`
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

若要再Razor View前台的JS使用，須搭配`@Html.Raw(jsonStr);`表示不做HTML編碼，即可再JS正常使用它。

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
6. 看看別人怎麼做: https://www.youtube.com/watch?v=DQxGDFZn_6Y&list=PLneJIGUTIItsqHp_8AbKWb7gyWDZ6pQyz&index=55

[1]: <https://learn.microsoft.com/zh-tw/aspnet/core/mvc/controllers/dependency-injection?view=aspnetcore-3.1> "ASP.NET Core 中的選項模式"
[2]: <https://oldmo860617.medium.com/%E6%9C%9D%E6%9B%B4%E5%A5%BD%E7%9A%84-ooc-%E8%B5%B0%E5%8E%BB-ioc-%E6%8E%A7%E5%88%B6%E5%8F%8D%E8%BD%89%E8%88%87-di-%E4%BE%9D%E8%B3%B4%E6%B3%A8%E5%85%A5-b7fed15ff058> "SOLID"

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

## 壓力測試 (待研究)

建好之後來玩  

ref: 
- https://blog.twjoin.com/%E6%8B%93%E5%B1%95%E6%8A%80%E8%83%BD%E6%A8%B9%E4%B9%8B%E5%A3%93%E5%8A%9B%E6%B8%AC%E8%A9%A6-stress-test-%E7%AF%87-59b3d184b804


## Middleware 中介軟體
ref:
  1. https://learn.microsoft.com/zh-tw/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1


Lading... 


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
實作依據環境變數讀取不同的Server環境變數設定檔，最直接是Log Level與ConnectionString，一些API的設定也會不同等。

Lading... 


## ActionFilter 應用實作
1. 如果今天有人知道 Get/Post等 路由除了經過Token等有效驗證外，應還要檢查是否有該Controller/Action權限。
2. 假使今天有個控制器都是在做查詢類的情境，那可能會有完全共用的輸出(Component View)，其所需的Response Model物件肯定也一樣，此時就可以使用，但這總感覺不太對[Filter]味，待研究`[ActionFilter]`實務上究竟是如還運用。

Lading... 

## 自定義擴充[ValidationAttribute] 應用實作

1. 可重用的Regex驗證 比如整數+小數位數 對應DB`decimal(8, 4)`長度 
2. 字串的英數字編碼、加上Regex與相關邏輯去把非中文擷取出來比對後，僅將 屬性成員中非中文部分進行編碼

Lading... 

## 找找DbContext的Base層大家會怎麼去實作並應用 (後面再研究)
1. 有可能在Query或其他SaveChange()時，跑去寫一行Log或將所執行的動作回寫至DB特定的紀錄表 這類應用

Lading... 

## 效能驗證方式
1. Dapper 查詢
2. EFCore 新增/刪除/修改

> 得要找一些三方套件或是人家怎麼去驗證自己寫的程式效能好不好的方式

Lading... 