![Logo](Logo.png)

## Navigation
* [Documentation](#documentation)
* [Installation](#installation)
* [Contribution](#contribution)


## Documentation
For documentation, tutorials, version history etc... visit the following link:

### [GoAhead Wiki](https://nikola-grunchevski.gitbook.io/goahead/)


## Installation
All releases since version 2.0.6 can be found in the tab **Releases** of the repository. For older versions, check the following link: [Previous Versions](https://www.dropbox.com/sh/jbi2nvswsgffklm/AACL_hHlBd-ejB1yym3bj17la?dl=0)

For GoAhead 2.0 to work, an external DLL is required: tcl86t.dll. It will suffice to place it in the same folder as your GoAhead.exe. Alternatively, you can place it in your System folder, typically under "C:\Windows\System32". You might also need to add it in "C:\Windows\SysWOW64".

By default, it is included in the project in the folder bin, which is the configured build output.


### Troubleshooting

If the above instructions don't work and you are getting an error that the DLL can not be found or loaded, you can try the following procedure:

1. Download the tool in the link:
[Dependency Walker](http://www.dependencywalker.com/)

2. After downloading it, you can just run depends.exe and then File->Open and open the tcl86t.dll

3. Now you should be able to see all the DLLs that tcl86t depends on and perhaps missing one of them is why your PC can't load it. If a DLL is missing it will be marked with a yellow question symbol. You should expect some false positives - everything labelled as missing that starts with API-MS... and EXT-MS... is not required and can be ignored. So if you see anything else that is missing then that's likely the reason that tcl86t.dll couldn't load.

## Contribution
Please don't push or merge directly to the master branch. Branch out of a version branch and merge into it. Once a version is ready, it will be merged into master and released.
