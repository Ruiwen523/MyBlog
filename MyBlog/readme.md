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
    	- 

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
2. 批次註冊 
3. 套件Autofac 
4. 檔名尾巴Service
5. 屬性?

ref:
- https://www.stevejgordon.co.uk/aspnet-core-dependency-injection-what-is-the-iservicecollection 
- https://www.youtube.com/@TalllKaiCoding/about


### 在同一個interface注入多個Service並依照傳入不同的Type決定實作哪一個Service.cs
``` C#

// Startup.cs
public void ConfigureServices(IServiceCollection services)
{    
    services.AddSingleton<IPay, PayMoney1Service>();
    services.AddSingleton<IPay, PayMoney2Service>();
}

// PayMoney1Service.cs
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

// PayMoney2Service.cs
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

// PayController.cs
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
// 第一種Type
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

// 第二種Type
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


Lading... 