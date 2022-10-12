![Nuget](https://img.shields.io/nuget/v/RFVC.IPTV.Utils.Net6)
# RFVC.IPTV.Utils

A collection of Utility Classes about M3u and XMLTV.

## Why?

I needed to parse and trim a large list ( over 50 megas ) and that is the why...

## How can i help

If there is a m3u list that dosen´t work with this code, share an example or sample and we will check it out.

## Where is it used

This is used on my own projects... will share it later

## Change Log

### 1.2.0

- Added CompleteLogoInformation Method 

### 1.1.0

- Added Download Method and Parse
- Extended M3uItem to include original content

### 1.0.0

- Initial Version

## Main Functions

### M3u
| Function | Description |
|--| --|
| GetM3UFileItems | Converts m3u file content into list of objects to use |
|FilterM3uFileByGroup| filters the content of an m3u file by groups |
|CompleteLogoInformation | tries to fill missing logo within the list |

### TvGuide

| Function | Description|
|--|--|
| GetTvGuideItems | Converts xmltv file content into a list of objects to use |
| GetTvGuideItemsForM3uFileItems| Filter the content ona xmltv file and gets only the ones related to a filtered m3u list |





