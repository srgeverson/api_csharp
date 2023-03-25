# API C#
Este sistema servirá para gerenciar usuários.

## 📌 Versão ainda em desenvolvimento 2.0.0

## 💡Pré-requisitos
Antes de começar, você vai precisar ter instalado em sua máquina as seguintes ferramentas:
[Git](https://git-scm.com), [.NET Core](https://dotnet.microsoft.com/en-us/download), [SQL Server](https://www.microsoft.com/pt-br/sql-server/sql-server-downloads), [Visual Studio](https://visualstudio.microsoft.com/) ou [VS Code](https://code.visualstudio.com/) e [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16)

## 🚀 Dependências utilizadas
* **Swashbuckle.AspNetCore 6.4.0**
* **Microsoft.AspNetCore.Mvc.Versioning 5.0.0**
* **Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer 5.0.0**
## 🛠️ Constrção da aplicação

```bash

# Criando o projeto
$ dotnet new webapi -o api_csharp

# Entrando na pasta criada
$ cd api_csharp

# Habilitando protocolo https
$ dotnet dev-certs https --trust

# Criando README.md 
$ echo "# API C#" >> README.md

# Inicializando repositório
$ git init

# Commitando pela primeira vez
$ git add README.md
$ git commit -m "first commit"

# Definindo branch principal
$ git branch -M main

# Enviando para repositório remoto
$ git remote add origin https://github.com/srgeverson/api_csharp.git
$ git push -u origin main

# Biblioteca de versionamento
$ dotnet add package Microsoft.AspNetCore.Mvc.Versioning --version 5.0.0

```

## 🎲 Executando a aplicação

```bash

### Clonando o projeto
$ git clone https://github.com/srgeverson/api_csharp.git

# Entrando na pasta criada
$ cd api_csharp

# Restaurando/Instalando dependências
$ dotnet restore "./api_csharp.csproj"

# Executando aplicação
$ dotnet run --urls=https://localhost:44326

# Acessando swagger
$ https://localhost:44326/swagger/index.html

# Gerando publicação da aplicação
$ dotnet publish "api_csharp.csproj" -c Release -o /app/publish

# Criando a imagem docker
$ docker build -t api_csharp .

# Executando imagem docker
$ docker run -p 8080:80 api_csharp

# Executando docker compose
$ docker-compose up --build

```

## 👨‍💻 Equipe de Desenvolvimento

* **Geverson Souza** - [Geverson Souza](https://www.linkedin.com/in/srgeverson/)

## ✒️ Autor

* **Geverson Souza** - [Geverson Souza](https://www.linkedin.com/in/srgeverson/)