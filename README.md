WebVella ERP 
======
**WebVella ERP** is a free and open-source web software, that targets extreme customization and plugability in service of any business data management needs. It is build upon our experience, best practices and the newest available technologies. Currently it targets ASP.NET Core 1, MVC6 and AngularJS. Our database of choice is PostgreSQL 9.6.x. Targets Linux or Windows as host OS. Currently tested only on Windows.

We will greatly appriciate your support of the project by: 
* giving it a "star" 
* contributing to the source
* Donate via Flattr [![Flattr this git repo](http://api.flattr.com/button/flattr-badge-large.png)](https://flattr.com/submit/auto?user_id=webvella&url=https://github.com/WebVella/WebVella-ERP&title=WebVella-ERP&language=&tags=github&category=software) 

#### Screenshot
![Desktop example](https://cloud.githubusercontent.com/assets/341637/7510849/05e25a66-f4a9-11e4-8d2a-b19113017986.PNG "desktop example")

![Internal page example](https://cloud.githubusercontent.com/assets/341637/7510850/05e35cae-f4a9-11e4-8bfb-81640d82ce72.PNG "internal page example")

## How-to use this code
1. Download or Clone the source ($ git clone https://github.com/WebVella/WebVella-ERP.git)
2. Install or Get a connection string to a PostgreSQL database. You can get the database from [HERE](http://www.postgresql.org/). Currently tested and supported on version 9.6.x, so feedback if there are problems with the newest version.
3. Set the /WebVella.ERP.Web/Config.json with the proper connection string settings. There is example configuration there. The EncriptionKey is optional and if not provided will use a default hardcoded one. For test purposes you can leave it as it is.
4. Set up the site 
5. Browse :)
6. Default account is with email "erp@webvella.com" and password "erp"


## You will get a barebone installation. Before that you can test by doing the following:
1. Create a test entity from "administration -> entities"
2. Create a "general" view for this entity from "administration -> entities -> views" and set it as a default one.
3. Create a "quickcreate" view for this entity from "administration -> entities -> views" and set it as a default one.
4. Create a "general" list for this entity from "administration -> entities -> lists" and set it as a default one
5. Create a sample area to hold the test entity records from "administration -> areas".
6. Subscribe the test entity to the sample area by managing the area. You should be able to get the entity from the dropdown, if you have correctly set the list and view, and didn't forget to mark them as default.
7. Go back to the desktop by clicking on the red button on the top of the screen with the tree icon on it.
8. You should be able to see your new area there. 
9. Click the area. You should be redirected to the test entity records, which are non existent to this moment.
10. Create some records.
11. Do not forget to play with the API too. There are a lot nice methods already present in the APIController.

### Translations
* Translation support is implemented and translation is under way. To check the available translations go to the the [lang folder of the webvella-core plugin](https://github.com/WebVella/WebVella-ERP/tree/master/WebVella.ERP.Web/wwwroot/plugins/webvella-core/lang). To test one of the available ones, change the lang setting of the config.json file, to the corresponding language abbreviation value. For more about how the translation works, check out the awsome [angular-translate project](https://angular-translate.github.io/).
Current translation status:
1. English - 80%
2. Spanish - underway thanks to @Sefirost
3. Russian - underway thanks to @IManakhov
4. Bulgarian - underway

### Third party libraries
* see [LIBRARIES](https://github.com/WebVella/WebVella-ERP/blob/master/LIBRARIES.md) files

## License 
* see [LICENSE](https://github.com/WebVella/WebVella-ERP/blob/master/LICENSE.txt) file

## Version 
* Soon to be in beta

## Contact
#### Developer/Company
* Homepage: [webvella.com](http://webvella.com)
* Twitter: [@webvella](https://twitter.com/webvella "webvella on twitter")


[![Flattr this git repo](http://api.flattr.com/button/flattr-badge-large.png)](https://flattr.com/submit/auto?user_id=webvella&url=https://github.com/WebVella/WebVella-ERP&title=WebVella-ERP&language=&tags=github&category=software) 