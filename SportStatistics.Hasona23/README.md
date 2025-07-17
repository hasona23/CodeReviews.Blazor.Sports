The app has the ability to track statistics of user Team.

Features: 
1- Make Matches
2- Manage Team members
3- Manage Matches
4- See player Data using Charts
5- Enable/Disable Players
6- Allow Users to become team player
7- Sign-up/Login using Google

How to run
make sure you are in the directory where .csproj is found 
> ./SportStatistics.hasona23/SportStatisticsApp/
1- Make sure you have Dotnet user-secrets enabled
``` dotnet user-secrets init ```
then let's add the ability to use google login 
``` dotnet user-secrets set Authentication:Google:ClientId {Add Your Client Id} ```
``` dotnet user-secrets set Authentication:Google:ClientSecret {Add Your Client Secret} ```

2- to run project
``` dotnet build ```
``` dotnet run ```

3- becoming a coach enter in login
```coach@example.com```
```Password123!```


Enjoy :smile