# CYOA Game Package

Choose Your Own Adventure Game Package. This Unity-based interactive storytelling platform was developed for educational workshops and research, featuring dynamic Google Sheets integration for easy content management.
How It Works

Create your story content directly in Google Sheets
Game reads data live from Google Sheets via API
No CSV conversion needed - everything is dynamic
Immediate content updates without rebuilding the game
Comprehensive data collection for research and analytics
Modular scene architecture supporting various story types

Key Features

Looped Scene Flow: Title → Warning → Access → Tutorial → Photo → Animation → Main Story → Title
Google Sheets Integration: Real-time content loading from multiple sheet tabs
Choice-Based Storytelling: Support for linear dialogue and branching choices
Journal System: Automatic reflection content based on player decisions
Access Control: Workshop-based authentication system
Data Collection: Comprehensive player choice and engagement tracking
Developer Tools: Built-in testing and debugging features

Prerequisites

Unity 2021.3+ LTS (or compatible version specified in project)
Google Sheets API Key: Required for sheet integration
Google Sheets Document: Properly formatted with required tabs
Internet Connection: For real-time data loading

Google Sheets Setup
Required Sheet Tabs:

Access: Access codes and workshop IDs for authentication
Title: Title screen branding and background information
StorySheet: Main story content with 22-column structure
JournalSheet: Reflection content and journal entries
CreditSheet: Credits, about, and help information
IntroFrames: Animation sequence definitions
TriggerWarningSheet: Trigger warning content
Downloads: Asset management for dynamic content

Template and Examples:

Template: Google Sheets Template
Live Example: Working CYOA Example

Setup Workflow
1. Google Sheets Configuration

Copy the template or create sheets with required tabs
Share your sheet with "Anyone with the link can view"
Extract Sheet ID from your Google Sheets URL:
https://docs.google.com/spreadsheets/d/YOUR_SHEET_ID_HERE/edit#gid=0

2. Unity Project Setup

Clone this repository
Open in Unity (compatible version specified in project settings)
Update API Configuration:

Navigate to AccessControl scene
Find GoogleSheetReader script
Update sheetKey with your API key
Update default sheetId with your Sheet ID



3. Content Creation

Follow the live example format for your content
Update StorySheet with your dialogue and choices
Add journal entries in JournalSheet tab
Customize branding in Title tab
Set access codes in Access tab

API Integration Details
The game uses Google Sheets API v4 with this URL format:
https://sheets.googleapis.com/v4/spreadsheets/SHEET_ID/values/TAB_NAME?key=API_KEY
Example Implementation:

GoogleSheetReader.cs: Reads access control data
StorySheetReader.cs: Loads main story content
TitleScreenReader.cs: Gets title screen information

Each reader script:

Constructs API URL with Sheet ID and tab name
Makes HTTP request to Google Sheets API
Parses JSON response into ScriptableObjects
Triggers UI updates when data loads

Scene Architecture
Scene Flow:

TitleScreen: Dynamic branding from Google Sheets
TriggerWarning: Content warnings and support resources
AccessControl: Authentication and asset downloading
Tutorial: Interactive gameplay instructions
Photo: Character introduction display
Intro Animation: Cinematic opening sequence
CYOA_Scene: Main interactive storytelling experience

Key Scripts:

Sheet Readers: Load data from different Google Sheets tabs
UI Populators: Manage scene interfaces and interactions
Choice Recorder: Tracks player decisions for research
Dev Tools: Testing and debugging utilities

Data Collection
The system automatically collects:

Player Choices: All decisions with timestamps
Session Data: Player paths through the story
Engagement Metrics: Time spent, journal access, etc.
Research Analytics: Comprehensive data for studies

Development Features
Developer Tools:

Scene Skipping: U+I hotkey for rapid testing
Sheet Switching: Runtime Google Sheets ID changes
Debug Logging: Comprehensive error tracking
Live Updates: Immediate content changes without rebuild

Asset Organization:
Assets/
├── Resources/
│   ├── Backgrounds/     # Scene backgrounds
│   ├── Characters/      # Character sprites
│   ├── Props/          # Interactive objects
│   ├── Sounds/         # Audio effects
│   ├── UI Elements/    # Interface graphics
│   └── AnimationImages/ # Intro sequence frames
├── Scripts/            # Game logic
├── UI Documents/       # Interface layouts
└── Fonts/             # Text fonts (Helveti Hand)
Customization
For Different Workshops:

Create new Google Sheets from template
Update access codes for your participants
Modify story content for your specific needs
Customize branding in Title tab
Update Sheet ID in Unity project

For Different Stories:

Use StorySheet structure for your narrative
Add journal prompts relevant to your content
Include appropriate content warnings
Update credits and about information

Developed by GRID (Games Research and Immersive Design) for educational and research applications.
Master Document: https://docs.google.com/document/d/1d1PVdXazAzQMl-V2prAexhxPvDem7Gm17LLrVfM6qJI/edit?usp=sharing 
