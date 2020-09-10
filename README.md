## 專案架構 ##
*   資料層:DB層、DTO層、DAL層(DataAccess)
*   商業層:BO(Business)層、Service層、CommonHelper層
*   表層:Controller層、VO層(ViewModel)、View層
### 架構優點 ###
*   各分層職責明確劃分明確
*   因為IOC DI劃分，各分層耦合性低
*   方便撰寫單元測試
*   專案可規劃CI CD機制
*   由於Controller與前端劃分明確，便於串接各種前端框架，例如:Angular、React、Vue…等
*   由於Service與controller的劃分，專案容易改成DMZ的模式讓Controller透過API跨網段呼叫Service，增加網站的安全性
### 架構缺點 ###
*   若非多人專案開發的話，製作起來相對費工
## 功能 ##
*   註冊功能
*   登入功能
*   角色新增、修改、刪除功能
*   角色使用者控管功能
*   功能新增、修改、刪除
*   角色功能控管功能
*   對頁面、按鈕控管功能
*   單元測試
*   密碼加密機制
## 未完成的地方 ##
*   Error Message整合
*   ExecuteResult 物件 IsSuccess Message
*   Log機制
*   驗證機制(新增、修改欄位驗證)
*   修改密碼機制
*   會員資料管理機制(修改、查詢)
*   機敏資料加密 https 欄位資料加密(前後端寫bas) guid
*   修改(任何修改)資料時，驗證身分是否相符
*   機敏資料層級限制(for Table)