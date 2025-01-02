# onikaplus_server

| Command                                                                                    | Description                            |
| ------------------------------------------------------------------------------------------ | -------------------------------------- |
| dotnet ef database update -c AppDbContext -p .\onikaplus_server\                                    | Update database to latest version      |
| dotnet ef migrations add 'migration name' -c AppDbContext -p .\onikaplus_server\ -o "DB/Migrations" | Create migration with 'migration name' |

All new enum must be added in `ConfigureConventions` method like this

```
protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
{
    base.ConfigureConventions(configurationBuilder);

    configurationBuilder.Properties<EnumType>().HaveConversion<string>();
}
```