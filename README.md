# CYOA Game Package

Choose Your Own Adventure Game Package. This was developed for the VPVA workshop series.

## Overview

This Unity-based tool allows you to create interactive narrative experiences by:

* Managing your story content in Google Sheets
* Importing data directly from Google Sheets to Unity via Google Sheets API
* Automatically generating game scenes and templates based on the imported data
* Tracking user choices and interactions for later analysis
* Exporting user data as CSV files

## Prerequisites

1. Google Sheets API Key
2. Google Sheet document set up with the correct format
3. Unity project with this package imported

## File Structure

The main components are organized as follows:

* SheetReaders - Located in `Assets/Scripts/CYOA_Scene` and other modules
  * `StorySheetReader.cs` - Reads story content
  * `JournalSheetReader.cs` - Reads journal entries
  * `CreditSheetReader.cs` - Reads credits information
* UI Populators - Located in corresponding module folders
  * `TitleScreenPopulator.cs` - Populates the title screen
  * `UIPopulator.cs` - Handles main game UI elements
  * Other specialized populators for specific scenes

## Workflow for Importing Data from Google Sheets to Unity

### Step 1: Prepare Your Google Sheet
1. Create a Google Sheet with your content formatted appropriately
2. Share the sheet with "Anyone with the link" access
3. Go to File > Share > Publish to Web and publish the entire document as a web page

### Step 2: Configure the Connection in Unity
1. Obtain your Google Sheets API key
2. Find your Spreadsheet ID from the sheet URL:
   * Example URL: `https://docs.google.com/spreadsheets/d/1SLm9j993IbtSKpzmVoshhebh7FxJcZOp2a4BU5aId8g/edit#gid=0`
   * Spreadsheet ID: `1SLm9j993IbtSKpzmVoshhebh7FxJcZOp2a4BU5aId8g`

3. In your reader script (e.g., `TitleScreenReader.cs`), set the sheet URL with this format:
   ```
   https://sheets.googleapis.com/v4/spreadsheets/<Spreadsheet ID>/values/<Sheet Name>?key=<API Key>
   ```

### Step 3: Run the Game
1. The reader scripts will fetch data from Google Sheets and convert it to scriptable objects
2. The populator scripts will use these scriptable objects to create the game experience
3. User interaction data will be stored and can be exported as CSV

 
