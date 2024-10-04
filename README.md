# DOOM1-2WadLoader
Simple tool for installing wads into DOOM + DOOM II for Windows, Currently supported for v1.0.2265

## Installation

  1) Download the zip file from the releases tab and extract it to a new empty folder
  2) Place as much wads you'd like to install in the folder
  3) Run **DOOM1+2WadLoader.exe**
  4) Head to Steam and launch the game, you'll see the newly installed wads in **Mods -> Play**
  5) Have Fun :)
    
## Launch Arguments (optional)

To initiate a launch command its recommend to use CMD or PowerShell.

  1) launch cmd (or shift + right click inside the folder and select PowerShell)
  2) make sure you're at your folder's directory (you can use `cd` to change the current directory)
  3) Type **DOOM** then press **Tab** (It'll autofill the exe's filename for you)
  4) After the typing the exe's filename give one empty **Space** and type any of the following arguments desired:

To run the repair tool use:
```
C:\"DOOM1+2WadLoader.exe" -r
```
To disable wad titling (uses wad's filename instead):
```
C:\"DOOM1+2WadLoader.exe" -n
```
To redirect the bnetwads folder:
```
C:\"DOOM1+2WadLoader.exe" "path to bnetwads folder"
```
## Repair Tool
The repair tool is an included function within the exe to verify existing bnetwad folders and ensure they're functional by reviewing and comparing it's wad and json data respectively.

Conditions applied:
- Checking whether the folder name matches the stored wad filename
- Ensuring wad and json file existance for each folder
