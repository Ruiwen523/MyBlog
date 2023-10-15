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

### Dapper - Query:
1. Query
2. QueryFirstOrDefault 

### Dapper - Execute​
執行Insert、Update、Delete、Stored Procedure時使用。  
- Execute

### 資料型態 (DbTyoe)
參數一定要指定型態，否則會讓Dapper轉換為預設型態，導致查詢效能變差。  
ref: 
1. https://learn.microsoft.com/zh-tw/dotnet/api/system.data.dbtype?view=net-7.0#system-data-dbtype-string  
2. https://igouist.github.io/post/2021/05/newbie-3-dapper/
3. https://dotblogs.com.tw/OldNick/2018/01/15/Dapper#Dapper%20-%20Transaction

- DbType.AnsiString
  - Varchar
- DbType.String
  - NVarchar
- DbType.AnsiStringFixedLength
  - Char
- DbType.StringFixedLength
  - NChar

## Memo  
Insert資料可改用Query語法執行，僅須完整 Insert SQL 語句的最後一行加上`@@IDENTITY` 或 `LAST_INSERT_ID()`，以及改用 int 變數接回`conn.Query<int>(sql, param)`結果即可實作

後續再來閱讀 BeginTransaction 開啟 SQL Transaction 交易包裝，可Try Catch包起來，若於Commit()發生例外可即時Rollback()