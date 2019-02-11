# 7Pace / Azure DevOps Plugin for ManicTime

## Summary

This is a plugin for the [ManicTime time-tracking application](https://www.manictime.com/).
It serves to ease the task of tracking time against Azure Work Items,
and publishing tracked time to the 7Pace Azure DevOps extension.

It achieves this in 2 ways:

- Assigned Work Items are automatically available as Tags in ManicTime.
- Created Tags are used to update 7Pace time sheets on-the-fly.

## Setup

### Installation

> **TL;DR** Copy contents of `\dist` to `%systemdrive%\Users\%username%\AppData\Local\Finkit\ManicTime\Plugins`.

First step is to locate the ManicTime plugin directory. To do so, open ManicTime. Click on the cog at the top right of the window, and click
`Advanced > Open Datebase Folder` in the menu that appears. You will see a folder here called `Plugins`, navigate to `Plugins\Packages`. Here is where you will copy the plugin folder. If `\Packages` does not exist, create it.

The plugin folder is located within the `\dist` folder of this repository.
Close ManicTime (The application can be hidden in the [Notification Area](https://docs.microsoft.com/en-us/windows/desktop/uxguide/winenv-notification), so ensure it has been completely exited)
Copy the folder named `MFitzpatrick.TagSource.Azure7PacePlugin` to the plugin folder opened in the previous step. 

Restart ManicTime. 

### Configuration

> **TL;DR** `Puzzle Icon > Plugin Manager > Settings`, then enter
> configurations.

You should now see a plugin named `Azure 7Pace Tag Plugin` within
ManicTime. (Puzzle icon at top right of window > Plugin Manager)

Click on settings, and enter your Azure Personal Access Token (with read
access to work items), as well as your TimeTracker API secret. The
TimeTracker API secret can be generated within Azure DevOps. Navigate
to `Time > Configuration > API`. Check the box to activate reporting
API access, and press the `Create Token` button to generate the secret.

Below that is the field to configure which organization in Azure DevOps
the work items should be retrieved from.

Finally you can specify the Activity Type ids for billable, and
non-billable tasks in 7Pace. There are at least 2 ways to get a
list of activity types and ids.

1. `GET` request to `https://<organization>.timehub.7pace.com/api/rest/activityTypes?api-version=3.0-preview`,
   using the Time Tracker Api Token for Bearer Authorization.
2. In Azure DevOps, navigate to `Time > Configuration > Activity Types`,
   and inspect the `data-id` attribute of the radio buttons.

## Features

- Currently assigned tasks are available as tags in ManicTime.
  - Id, Project Name, Task Name (as additonal notes)
- Last 7 days of Tags are monitored, Tags further back can be
  considered 'locked', so there is no danger of losing _too_ much
  history should you lose tags locally.
- Time that you manually enter into 7Pace will not be affected
  by this plugin.
- Added Tags (that originate from this plugin) will syncronize with
  7Pace, this includes all creates, edits, deletes of Tags dated
  within the last 7 days.

### Roadmap

- Configurable WIQL query for fetching work items to display as tags.

## Contributing

There are a few important points to note:

- Plugin runtime must be `.net framework 4.5`. I tried upgrading to
  `4.7.2` but it would not appear in plugin listing.
  - As a corollary, any `netstandard` libraries must be `1.1` or below.

The projects `WorkItemService`, `TimeTrackingService` and `HttpExtensions`
all generate package files on build. Located in `<ProjectRoot>\bin\<configuration>`.

These currently are not hosted and are manually included as a NuGet
directory in Visual Studio.

## Tests

There are currently no tests.
