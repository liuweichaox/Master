使用 Clean Architecture 的原始 SQL 和 DDD 的示例 .NET Core REST API CQRS 实现。
==============================================================

## CI

![](https://github.com/kgrzybek/sample-dotnet-core-cqrs-api/workflows/Build%20Pipeline/badge.svg)

## 给个 Star!

如果你喜欢这个项目，学到了一些东西或者你正在你的应用程序中使用它，请给它一个星。 谢谢！

## Description
使用基本 [CQRS](https://docs.microsoft.com/en-us/azure/architecture/guide/architecture-styles/cqrs) 方法和域驱动设计实现的示例 .NET Core REST API 应用程序。

## Architecture [Clean Architecture](http://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)

![projects_dependencies](https://github.com/kgrzybek/sample-dotnet-core-cqrs-api/raw/master/docs/clean_architecture.jpg)

## CQRS

读取模型 - 在数据库视图对象上执行原始 SQL 脚本 (using [Dapper](https://github.com/StackExchange/Dapper)).

编写模型 - 域驱动设计方法（使用 Entity Framework Core）。

使用 [MediatR](https://github.com/jbogard/MediatR) 库处理命令/查询/域事件。

## Domain

![projects_dependencies](https://github.com/kgrzybek/sample-dotnet-core-cqrs-api/raw/master/docs/domain_model_diagram.png)

## Validation
使用 [FluentValidation](https://github.com/JeremySkinner/FluentValidation) 进行数据验证

使用 [ProblemDetails](https://github.com/khellang/Middleware/tree/master/src/ProblemDetails) 的 HTTP API 标准实现的问题详细信息

## Caching
使用 Cache-Aside 模式和内存缓存。

## Integration
使用 [Quartz.NET](https://github.com/quartznet/quartznet) 实现发件箱模式

## Related blog articles
[使用原始 SQL 和 DDD 的简单 CQRS 实现](http://www.kamilgrzybek.com/design/simple-cqrs-implementation-with-raw-sql-and-ddd/)

[实体框架 2.2 的域模型封装和 PI](http://www.kamilgrzybek.com/design/domain-model-encapsulation-and-pi-with-entity-framework-2-2/)

[REST API 数据验证](http://www.kamilgrzybek.com/design/rest-api-data-validation/)

[领域模型验证](http://www.kamilgrzybek.com/design/domain-model-validation/)

[如何发布和处理域事件](http://www.kamilgrzybek.com/design/how-to-publish-and-handle-domain-events/)

[处理域事件：缺失部分](http://www.kamilgrzybek.com/design/handling-domain-events-missing-part/)

[.NET Core 中的缓存备用模式](http://www.kamilgrzybek.com/design/cache-aside-pattern-in-net-core/)

[发件箱模式](http://www.kamilgrzybek.com/design/the-outbox-pattern/)

## How to run application
1. 创建空数据库。
2. 执行 InitializeDatabase.sql 脚本。
2. 设置连接字符串（在 appsettings.json 中或通过用户机密机制）。
3. 跑！

## 如何运行集成测试
1. 创建空数据库。
2. 执行 InitializeDatabase.sql 脚本。
3. 使用名为“ASPNETCORE_SampleProject_IntegrationTests_ConnectionString”的环境变量设置连接字符串
- 从项目 [src/Tests/SampleProject.IntegrationTests](src/Tests/SampleProject.IntegrationTests) 运行测试
