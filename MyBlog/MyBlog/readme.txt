



Use Ctrl+T quick search file and then use Alt+\ search membert method
ex: 搜尋"Starup.cs"然後輸入"ConfigureServices"

1.安裝EFCore SqlServer & Tools

2.安裝Swagger套件
ref: 
 a. https://github.com/domaindrivendev/Swashbuckle.AspNetCore
 b. https://dotblogs.com.tw/xinyikao/2021/07/04/183326
 c. https://dotblogs.com.tw/yc421206/2022/03/12/via_swashbuckle_write_swagger_doc_in_asp_net_core_web_api

T: Startup.cs
M: ConfigureServices()
	add: services.AddSwaggerGen(); 以及定義 c.SwaggerDoc();swagger文件
	add: 安裝Swashbuckle.AspNetCore.Newtonsoft //services.AddSwaggerGenNewtonsoftSupport(); // 

啟用瀏覽器並查看預設使用以下url 
/swagger/{documentName}/swagger.json => "v1"
/swagger/
