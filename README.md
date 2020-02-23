# PinusProxy  
PinusProxy是为松果时序数据库(PinusDB)提供Restful的服务，一个PinusProxy可以为多个PinusDB提供Restful服务。 本项目仅仅是一个示例，并不包含用户验证。

## 开发环境  
visual studio 2019 + asp.net core 3.1

## 配置文件  
proxy.xml  
服务配置ServerSet下每一个Server项都是一个数据库服务。  
server配置项  
+ name : 服务名，在一个配置文件中，服务名必须唯一。  
+ ip : PinusDB的IP。  
+ port : PinusDB的端口。  
+ username : PinusDB的用户名。  
+ password : PinusDB的密码。  
+ maxconn : 数据库连接池的最大连接数

每个TableSet对应一个Restful可以访问的数据表。  
table配置项  
+ aliasName : Restful对外的表名, 一个配置文件中aliasName必须唯一。  
+ tableName : PinusDB数据库上的表名。  
+ server : PinusDB数据库实例名，server配置项中的 name 项的值。  


## 查询条件  

名字 | 类型 | 是否必须 | 描述  
-|-|-|-  
Field | 字符串 | 是  | 字段名  
Op | 字符串  | 是 | 比较运算  
Value | ? | 否 | 比较值  

Op 取值  
isnull : 指定的字段值是否为空，无Value参数。  
isnotnull : 指定的字段值是否非空，无Value参数。  
eq : 等于，可以用于bigint,datetime, double及string数据类型。  
ne : 不等于， 可以用于bigint,datetime, double及string数据类型。  
lt : 小于， 可以用于bigint，datetime，double数据类型。  
le : 小于等于，可以用于bigint，datetime，double数据类型。  
gt : 大于，可以用于bigint，datetime，double数据类型。  
ge : 大于等于， 可以用于bigint，datetime，double数据类型。  
like : 模式匹配，可以用于string类型，可以使用通配符%。  
in : 值位于指定的数组内，可以用于bigint类型。  
notin : 值不位于指定的数组内，可以用于bigint类型。


## 读取数据
+ 查询表信息  
请求地址: /api/query/tables  
请求方法: POST  
返回实例: 
```json
[{"AliasName":"tab01","TableName":"tab01","ServerName":"server1"}]
```

+ 查询原始数据  
请求地址: /api/query/raw  
请求方法: POST  
请求参数: 

名字 | 类型 | 是否必须 | 描述  
-|-|-|-  
Table | 字符串 | 是 | 表名  
Fields | 字符串列表 | 是 | 查询的列  
Conditions | 查询条件列表 | 否 | 查询条件  
Offset | 数字 | 否 | 跳过条数  
Limit | 数字 | 否 | 查询条数，默认为1000， 不大于10000

请求示例：  
```json
{"Table":"tab01", "Fields":["devid","tstamp","val01","val02"], "Conditions":[{"Field":"devid", "Op":"eq", "Value":10}], "Limit":5}
```

返回示例：  
```json
{ "err":0, "data": [{"devid":10,"tstamp":1582449065073,"val01":false,"val02":3378},{"devid":10,"tstamp":1582449070073,"val01":false,"val02":9733},{"devid":10,"tstamp":1582449075073,"val01":false,"val02":16130},{"devid":10,"tstamp":1582449080073,"val01":true,"val02":22529},{"devid":10,"tstamp":1582449085073,"val01":false,"val02":28876}]}
```

+ 查询快照数据  
与查询原始数据一致，表名后加.snapshot即可  
请求示例：  
```json
{"Table":"tab01.snapshot", "Fields":["devid","tstamp","val01","val02"], "Limit": 3}
```

返回示例：
```json
{ "err":0, "data": [{"devid":1,"tstamp":1582461490073,"val01":true,"val02":-179517},{"devid":2,"tstamp":1582461490073,"val01":true,"val02":-179427},{"devid":3,"tstamp":1582461490073,"val01":true,"val02":-179415}]}
```

+ 根据devid聚合查询  
请求地址: /api/query/group_devid  
请求方法: POST  
请求参数:   

名字 | 类型 | 是否必须 | 描述  
-|-|-|-  
Table | 字符串 | 是 | 表名  
Fields | 聚合字段列表 | 是 | 查询的列 
Conditions | 查询条件列表 | 否 | 查询条件  
Offset | 数字 | 否 | 跳过条数  
Limit | 数字 | 否 | 查询条数，默认为1000，不大于10000  

请求示例：  
```json
{"Table":"tab01", "Fields":[{"Aggregator":"none", "Field":"devid", "Alias":"devid"}, {"Aggregator":"max", "Field":"val02", "Alias":"max_val02"}], "Limit": 3}
```

返回示例：  
```json
{ "err":0, "data": [{"devid":1,"max_val02":199981},{"devid":2,"max_val02":199964},{"devid":3,"max_val02":199965}]}
```

+ 根据 tstamp 聚合查询  
请求地址: /api/query/group_tstamp  
请求方法: POST  
请求参数:  

名字 | 类型 | 是否必须 | 描述  
-|-|-|-  
Table | 字符串 | 是 | 表名  
Fields | 聚合字段列表 | 是 | 查询的列 
Conditions | 查询条件列表 | 否 | 查询条件  
GroupTstamp | 数字 | 是 | 毫秒数
Offset | 数字 | 否 | 跳过条数  
Limit | 数字 | 否 | 查询条数，默认为1000，不大于10000  

以1分钟为聚合，请求示例:
```json
{"Table":"tab01", "Fields":[{"Aggregator":"none", "Field":"tstamp", "Alias":"tstamp"}, {"Aggregator":"max", "Field":"val02", "Alias":"max_val02"}], "Conditions":[{"Field":"tstamp", "Op":"gt", "Value":"2020-2-23 18:0:0"},{"Field":"devid", "Op":"eq", "Value": 123}], "GroupTstamp":60000, "Limit": 3}
```

返回示例：  
```json
{ "err":0, "data": [{"tstamp":1582452000000,"max_val02":null},{"tstamp":1582512000000,"max_val02":null},{"tstamp":1582572000000,"max_val02":null}]}
```

+ 对所有数据进行聚合  
请求地址: /api/query/group_all  
请求方法：POST  
请求参数:  

名字 | 类型 | 是否必须 | 描述  
-|-|-|-  
Table | 字符串 | 是 | 表名  
Fields | 聚合字段列表 | 是 | 查询的列 
Conditions | 查询条件列表 | 否 | 查询条件  

请求示例:
```json
{"Table":"tab01", "Fields":[{"Aggregator":"count", "Field":"devid", "Alias":"count_num"}]}
``` 

返回示例:  
```json
{ "err":0, "data": [{"count_num":248600}]}
```


