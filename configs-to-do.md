#### Configurations necessary for using the app:

- change the IP in the connection string from the constructors of every repository to your IP in your network
- (possible) install from NuGet Microsoft.Data.SqlClient

##### To populate database:
- in Sql Server Configuration Manager, go to Sql Server Network Configuration > Protocols for *name of your server* > TCP/IP; on Protocol tab, make sure that Enabled and Listen All are both on Yes; on IP Adresses tab, at the bottom, under IPAll, leave TCP Dynamic Ports blank and put at TCP Port 1235 (you can change it, put change it in the code as well)
- try connecting to the server in Sql Server Management Studio first using the IP and port: for example, 192.168.43.73,1235 (with a comma ','!)
- change in populateDB > Properties > launchSettings.json the working directory to be the directory where your populateDB.csproj is located
- open populateDB.sln and run it (install from NuGet Microsoft.Data.SqlClient if you don't have it)
