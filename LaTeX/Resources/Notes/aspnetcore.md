# ASP.NET Core

Aqui descrevo os passos para criar um projeto ASP.NET Core (Database First).

## Pré-requisitos

Instalar o Visual Studio 2019 com ASP.NET Core Development e Database Development Tools, de seguida instalamos o SQL Server Express e o SQL Server Management Studio.

Abrir o terminal e executar o comando:

+ ```dotnet tool install --global dotnet-ef```

## Pacotes

Instalar pacotes de NuGet, pareço o McDonald's:

+ Microsoft.AspNetCore.Identity.EntityFrameworkCore
+ Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation
+ Microsoft.EntityFrameworkCore.SqlServer
+ Microsoft.EntityFrameworkCore.Tools
+ Microsoft.VisualStudio.Web.CodeGeneration.Design
Atenção:
+ Estou a usar o Visual Studio 2019, logo só tenho a versão do .NET Core 5.0.
Como tal:
+ As versões dos pacotes tem de ser 5.0.XX para funcionar.

## SQL Server

Criar uma base de dados:

+ Criar uma base de dados SQL Server via Queries no SQL Server Management Studio.
+ Todas as tabelas devem ter PKs, devem ser NOT NULL e as FKs têm de fazer ON DELETE CASCADE.

## Scaffolding

Abrir o terminal (CTRL+Ç) e executar o comando:

+ ```dotnet ef dbcontext scaffold "Server=.\SQLExpress;Database=DatabaseName;;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -output-dir Models --context-dir Data --namespace ProjectName.Models --context-namespace ProjectName.Data --context ProjectNameContext -f --no-onconfiguring```

Entrar em todos os ficheiros gerados e eliminar todos os partial, virtual e ? que não são necessários nem sei o que são, em especifico, de seguida, entrar no ficheiro ProjectNameContext.cs e remover a menção do partial class final e a sua override implementation.
