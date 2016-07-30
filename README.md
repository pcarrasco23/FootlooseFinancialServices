# Footloose Financial Services

Sample online banking system using C#, .NET, SQL Server, Entity Framework, MongoDB, WCF, WebAPI, AngularJS, and ASP.NET MVC

## How to Build

- To build from Visual Studio, open the solution FootlooseFS.sln file in Visual Studio 2015 or higher and build. 
This solution uses NuGet packages so your installation of Visual Studio must have the NuGet Package manager installed.

## How to Run

The client facing system requires SQL Server Express LocaleDB 2016 and IIS Express 10.0

- Download and install SqlLocalDB.msi from the download for [SQL Server Express LocalDB 2016] (https://www.microsoft.com/en-us/download/details.aspx?id=42299)
- Open the file FootlooseFS.Web.Service/web.config
	- Change the app setting "DataDirectory" to your specific location of the "FootlooseFinancialServices\Data" folder with the MDF file.
	
- Download and install [IIS 10.0 Express] (https://www.microsoft.com/en-us/download/details.aspx?id=48264)
- To start the WebAPI service open a command window and enter 
		"C:\Program Files\IIS Express\iisexpress" /path:C:\Users\Peter\Source\Repos\FootlooseFinancialServices\FootlooseFS.Web.Service /port:9095
- To start the AngularJS web site open a command window and enter
		"C:\Program Files\IIS Express\iisexpress" /path:C:\Users\Peter\Source\Repos\FootlooseFinancialServices\FootlooseFS.Web.Client /port:9092
- Open a web browser and go to http://localhost:9092
- Enter "avenere" for the user name and "avenere1" for the password" and click the Submit button.

The AngularJS web site uses the WebAPI web site for the data service. By default it communicates to the service on port 9095 but that can be modified
in the file C:\Users\Peter\Source\Repos\FootlooseFinancialServices\FootlooseFS.Web.Client\config.js

