Use Ctrl+T quick search file and then use Alt+\ search membert method
ex: 搜尋"Starup.cs"然後輸入"ConfigureServices"

1.安裝Swagger套件
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

2.安裝EFCore SqlServer & Tools
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
	dotnet ef dbcontext scaffold "Server=(localdb)\MSSQLLocalDB;Database=Blog;Trusted_Connection=True;MultipleActiveResultSets=true" Microsoft.EntityFrameworkCore.SqlServer -o Models
	dotnet build

3.安裝Postman
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

4.