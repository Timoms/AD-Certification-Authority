![LibADCA](https://raw.githubusercontent.com/Timoms/AD-Certification-Authority/master/ADCA.png)

# AD-Certification-Authority (LibADCA)
Provides tools for signing a key with a predefined template. Early development.

## What is this?

LibADCA provides some basic functions to sign against a Active Directory Certification Authority.  
Microsoft provides basic functions which can be handy. But for a quick and easy way to generate certificates, this can be painful.  

LibADCA tries to make this more simple, in a way programmer can use this to sign using a single button.


## Usage

The entry point of this library is the `PEngine`.  
You can call most of the functions from this class.  
For now, you can only parse the templates. Signing against the CA is already working but not included in this release.

### *Parse all active templates from the CA*
```c#
PEngine p = new PEngine();
List<CertTemplate> templates = p.ParseTemplatesFromAD();
```

This will return a list of templates you can later use to sign a cert against the CA.

# Attention!
This is really realy raw formatting using the windows command line.  
It is indeed really stable but might return unwanted results if not used correctly or with an AD thats not based on 2019's standards.

# Requests

Of course I'd love to add some more features.  
This project is non-profit thus I can't promise working 24/7 on it.  
Nevertheless I'll try my best to improve this library.
