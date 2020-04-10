# IOS Code Sign Helper

A simple program that can be used to generate p12 files for codemagic.so that flutter apps can be built for IOS without a mac.
The files depend on openSSL to generate the request certificates and the p12 files. So get them from https://indy.fulgan.com/SSL/  
The openSSL should be placed in a folder named openssl alongside the exe files.   

## How to use the Program:
* Create a project in the "Generate Sign Request" tab
* Generate a Certificate Sign Request File and if required copy it to any Location
* Use the request file to generate a certificate file from Apple through their developer program
* Go to the "Generate p12 File" tab in the program. Choose your project
* Import the cer file obtained from apple
* Generate the p12 file. You can save a copy to another location
* Use the p12 file to build ios app using codemagic (or anywhere else)
  
## Releases:
You can find the built program here
https://github.com/pratheeshrussell/IOS-Code-Sign-Helper/releases
