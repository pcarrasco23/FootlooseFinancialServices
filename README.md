# Footloose Financial Services

Sample online banking system using C#, .NET, SQL Server, Entity Framework, MongoDB, WCF, WebAPI, AngularJS, and ASP.NET MVC

User facing web sites:
 - FootlooseFS.Web.Client: Client facing web site built with AngularJS 2
 - FootlooseFS.Web.AdminUI: Management portal built with ASP.NET MVC 5 application
 
Back-end web services:
 - FootlooseFS.Web.Service: WebAPI RESTful back-end service for client facing site
 - FootlooseFS.EnterpriseService: WCF back-end service for management portal

DLL projects for back-end web services
 - FootlooseFS.Service: interface to service layer for database and business logic
 - FootlooseFS.DataPersistence: data access layer for SQL using Entity Framework 6

 - FootlooseFSDocumentDBETL: Utility program to create the MongoDB database and load it with data from the SQL MDF file
 - \Data\FootlooseFS.mdf - SQL Server data file
 
## How to Build

- To build from Visual Studio, open the solution FootlooseFS.sln file in Visual Studio 2015 or higher and build. 
This solution uses NuGet packages so your installation of Visual Studio must have the NuGet Package manager installed.

## Download and install runtime dependencies

Running the client facing web site requires SQL Server Express LocaleDB 2016 and IIS Express 10.0

- Download and install SqlLocalDB.msi from the download for [SQL Server Express LocalDB 2016] (https://www.microsoft.com/en-us/download/details.aspx?id=42299)
- Download and install [IIS 10.0 Express] (https://www.microsoft.com/en-us/download/details.aspx?id=48264)

In addition to run the Management portal you will need MongoDB 3 for Windows
- Download and install [MongoDB windows] (https://www.mongodb.com/download-center#community)
	- Review the installation instructions [here] (https://docs.mongodb.com/manual/tutorial/install-mongodb-on-windows/)
	- To make runnning MongoDB easier you will want to run it as a Windows service
	- Be sure to review the README file and create a directory for the data file and update the mongodb.cfg file with the location of the data file
	- If you need to change the default MongoDB port you will have to update the MongoDB configuration in the FootlooseFS.EnterpriseService and 
	  FootlooseFSDocumentDBETL projects.
	  
## How to Run Client facing web site

- (Only have to do this once) Open the file FootlooseFS.Web.Service/web.config
	- Change the app setting "DataDirectory" to your specific location of the "FootlooseFinancialServices\Data" folder with the MDF file.
	
- To start the WebAPI service open a command window and enter 
		"C:\Program Files\IIS Express\iisexpress" /path:C:\Users\Peter\Source\Repos\FootlooseFinancialServices\FootlooseFS.Web.Service /port:9095
	- Replace "C:\Users\Peter\Source\Repos" with your specific location of the Visual Studio project
- To start the AngularJS web site open a command window and enter
		"C:\Program Files\IIS Express\iisexpress" /path:C:\Users\Peter\Source\Repos\FootlooseFinancialServices\FootlooseFS.Web.Client /port:9092
	- Replace "C:\Users\Peter\Source\Repos" with your specific location of the Visual Studio project
- Open a web browser and go to http://localhost:9092
- Enter "avenere" for the user name and "avenere1" for the password" and click the Submit button.

The AngularJS web site uses the WebAPI web site for the data service. By default it communicates to the service on port 9095 but that can be modified
in the file C:\Users\Peter\Source\Repos\FootlooseFinancialServices\FootlooseFS.Web.Client\config.js

## How to Run the Management portal

These 3 steps only have to be run once before running the portal
- Open the file FootlooseFS.EnterpriseService/web.config
	- Change the app setting "DataDirectory" to your specific location of the "FootlooseFinancialServices\Data" folder with the MDF file.
- Open the file FootlooseFSDocumentDBETL/app.config
	- Change the app setting "DataDirectory" to your specific location of the "FootlooseFinancialServices\Data" folder with the MDF file.
- From Visual Studio set the startup project to FootlooseFSDocumentDBETL and run the program. 
    - This will create the MongoDB database footloosefs and load it will data from the SQL MDF file
	
- To start the WCF service open a command window and enter 
		"C:\Program Files\IIS Express\iisexpress" /path:C:\Users\Peter\Source\Repos\FootlooseFinancialServices\FootlooseFS.EnterpriseService /port:9096
	- Replace "C:\Users\Peter\Source\Repos" with your specific location of the Visual Studio project
- To start the Management port ASP.NET MVC 5 web site open a command window and enter
		"C:\Program Files\IIS Express\iisexpress" /path:C:\Users\Peter\Source\Repos\FootlooseFinancialServices\FootlooseFS.Web.AdminUI /port:9093
	- Replace "C:\Users\Peter\Source\Repos" with your specific location of the Visual Studio project
- Open a web browser and go to http://localhost:9093
- Enter "admin" for the user name and "admin1" for the password" and click the Submit button.
	
The Management portal web site uses the WCF web service for the data service. By default it communicates to the service on port 9096 but that can be modified
in system.serviceModel client endpoint URL in the file C:\Users\Peter\Source\Repos\FootlooseFinancialServices\FootlooseFS.Web.AdminUI\web.config