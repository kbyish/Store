1-Add to github 
 
    1- run command  
        git init
    2- run command 
        git add .
    3- run command 
         git commit -m "Initial commit of existing project"
    4- git remote add origin <remote repository URL>
        git remote add origin   https://github.com/kbyish/Store.git

        you can get the url from website like github 
        after you create  a new 
    go to github and create new repository and get url of that 
    Ex: 




2- install EF 
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.EntityFrameworkCore.Design

    
3- EF Commands  
    dotnet tool install --global dotnet-ef

    run the following command to create the db:
        dotnet ef migrations add InitialCreate
        dotnet ef database update

3- DB Connection string
    Data Source=localhost\SQLEXPRESS;
    Initial Catalog=master;
    Integrated Security=True;
    Pooling=False;Connect Timeout=30;
    Encrypt=True;
    Trust Server Certificate=True;
    Application Name=vscode-mssql;
    Connect Retry Count=1;
    Connect Retry Interval=10;
    Command Timeout=30


4- Mapper Framework: AutoMapper
    dotnet add package AutoMapper.Extensions.Microsoft.DependencyInjection
    then ceate Mapping class 
    then register it in Program 


