# MyBlog

打算從無到有，使用 NETCore 後端 API 搭配 Vue 建置。

預計使用:

- 後端
  - 使用 NET Core Web Api 3.1 專案
- 前端
  - 使用 Vue.js 3.x
- ORM: 
  - Entity Framework Core
  - Dapper
- 前端套件
  - Bootstrap 套版
  - jQuery
  - height.js
- 資料庫
  - 使用 SQLServer LocalDB
- IDE:
  - 使用 Visual Studio 2022
  - 搭配 Visual Studio Code
- 版控工具
  - 使用 GitHub 
  - 搭配 SourceTree 進行開發維護

## 內容大致為:  
未來 Blog 填入內容預計為開發或者維運上，學習到的相關知識內容及經驗。

> 選用參考工具書 來自於天瓏書局  
> 後端選用: ASP.NET Core 3.x MVC 跨平台範例實戰演練  
> 前端選用: 008天絕對看不完的Vue.js3指南


1. 目前正在練習Vue概念中預計到後面筆記做好後，搭配官網 document api 和參考大量部落客前輩文章 並開始實作
2. 待辦清單；同時只要想到後端該添加什麼東西進來，就會跑回來實作一下  
   1. 比方說後面的DI 實作的`Services.AddScoped<IServices, 實作的Services>()`
   2. 最基礎的`CORS MiddleWare`、`NLog`套件
   3. `ActionFilter`的權限檢核(Menu 關聯 Role、User物件null檢核(閒置太久等))
   4. 後端`Return`的`ActionFilter`還可以做成統一格式，比如`Return`的Json格式一定要`Handel` `Body`組合
   5. `Data Annotation`等驗證屬性；自定義可重用的擴充標註
   6. `SSO方法規則`(搭配IV key等編碼邏輯撰寫之Token)
   7. 通用package類別庫的撰寫
   8. 資安防範等
   9. 要去研究一下如何實作DI介面方法註冊，以因應不同時期下同一支方法的抽換
   10. 要研究一下 什麼是 程式碼品質的管控
       1.  除git版本控管外、Codeviewer外、Clean Code外、應該是架構還有單元測試
   11. 將DbContext抽離至方案中其他專案底下
       1.  在那之前先去了解一下DbContext的cache、如何去寫DbContext的Base層
   12. 大目標哪一些類型應當做分層就該分層，不單是在同一專案下的分層，還有切開成不同專案下分層
       1.  首要目標應當是: 高度可維護
       2.  先K書 Clean Code、Clean Architecture (還有一本C#的)
   13. 實作JWT、登錄auth(Token)驗證的`Middleware`
       -  Response出去前也可以用`[ActionFilter]`再檢查是否過期
       -  但若這是純後端API則要注意 使用者的狀態管理是做在前端自行管理還是後端Session記憶體中
   14. 閱讀並理解 「測試驅動開發（TDD）、DDD、CQRS架構理念」，於專案完成後實作並重構專案
       - 文章中有句話說得很好；每個程式設計員都應該努力掌握編寫程式碼的基本能力
       - 透過使用版本控制和頻繁提交，您可以確保將乾淨的程式碼實踐整合到開發過程中，防止程式碼庫惡化並促進開發人員之間的協作。
       - https://medium.com/@nile.bits/clean-code-in-c-a-guide-to-writing-elegant-net-applications-db8698c8c731
   15. 有多的時間把這個屬於Web且對目前有幫助的部份學完 IBM SkillsBuild 免費線上學習資源 Web Dev:  https://skillsbuild.org/zh-TW/adult-learners/explore-learning/web-developer
   16. 微軟也有一些訓練 https://learn.microsoft.com/zh-tw/training/browse/

> 但有一個大前提 全部都要從頭來過 從無到有 化為自己的經驗 所以到整個專案完成 要花很長時間去大量閱讀，因此一切不會搞得那麼多，會先將部落格「清單頁面/內容頁面」刻好，套版套好，我能自己撰寫Json透過後端API匯入或者Post上去 所需的後端+前端建設完成後再回頭添加這些東西。

