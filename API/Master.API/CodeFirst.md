## 还原 nuget

```
dotnet restore
```

# Code First

操作目录 Velen.API/

## 生成迁移

```
dotnet ef migrations add xxx --project ../Velen.Infrastructure/Velen.Infrastructure.csproj
```

## 更新数据库

```
dotnet ef database update --project ../Velen.Infrastructure/Velen.Infrastructure.csproj
```

## 删除迁移

```
dotnet ef migrations remove --project ../Velen.Infrastructure/Velen.Infrastructure.csproj
```

## 列出迁移

```
dotnet ef migrations list --project ../Velen.Infrastructure/Velen.Infrastructure.csproj
```

## 恢复迁移

```
dotnet ef database update xxx --project ../Velen.Infrastructure/Velen.Infrastructure.csproj
```