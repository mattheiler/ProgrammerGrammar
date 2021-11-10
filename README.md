# Programmer Grammar

.NET project and item templates using DDD/CQRS, MediatR, AutoMapper, Entity Framework Core, and FluentValidation.

### Projects
- Identity Gateway
- Web Application

### Items
- Command
- Query

## Setup

### Add package source

```
dotnet nuget add source https://nuget.pkg.github.com/mattheiler/index.json
```

### Add package

```
dotnet add PROJECT package ProgrammerGrammar
```

## Usage

```
dotnet new command
dotnet new identitygateway
dotnet new query
dotnet new webapplication
```
