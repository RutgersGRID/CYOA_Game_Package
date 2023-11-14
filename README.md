# CYOA Game Package

Choose Your Own Adventure Game Package. This was developed for the VPVA workshop series.

* Convert your script to a spreadsheet 
* Export to CSV
* Load into a directory in Unity
* That file then creates the scene and template for the game
* Stores user data and exports it as a CSV


 The CSV files are placed in the Editor folder
 `Assets/Editor/CSV Imports`


 Prerequisites
 1. Have a Googlesheets API Key
 2. Have a Googlesheet doc ready
 3. A script that takes a json and converts it into a scriptable object

 Workflow for importing data from Googlesheets to Unity: Googlesheets > Googlesheet json > Unity
 1. Have a Googlesheet ready what is shared with anyone who has access to the link.
 2. In the Googlesheet you want to go to File > Share > Publish to Web. You want to publish the entire document as a web page.
 3. Now you need to turn your sheet into a json. To do this you need your Spreadsheet id (Not API Key) and put it in this format "https://sheets.googleapis.com/v4/spreadsheets/<Spreadsheet id>/values/<Sheet Name>?key=<API Key>"
 Your Spreadsheet id should be in the link to the document. https://docs.google.com/spreadsheets/d/<Spreadsheet id>/edit#gid=0
 Example: https://docs.google.com/spreadsheets/d/1cC9iRPYMR9jgyKbeBM-wBO03rG5SPvONt8t-5fNJrTs/edit#gid=0 The id would be "1cC9iRPYMR9jgyKbeBM-wBO03rG5SPvONt8t-5fNJrTs"
 Replace <Spreadsheet id>, <Sheet Name>, and <API Key> with their respective variables in "https://sheets.googleapis.com/v4/spreadsheets/<Spreadsheet id>/values/<Sheet Name>?key=<API Key>".

 For an example of the workflow go to the TitleScreen scene and look at the script TitleScreenPopulatorTwo and GoogleSheetReaderTwo.
 GoogleSheetReaderTwo is the script that reads the Spreadsheet and turns it into scriptable objects. It is also where you will post the link: "https://sheets.googleapis.com/v4/spreadsheets/<Spreadsheet id>/values/<Sheet Name>?key=<API Key>" 
 TitleScreenPopulatorTwo takes the scriptable object and populates UIElements in the scene.

 
