# Clean Code ReSharper Plugin Project

### CleanCode

An experiment in trying to automate some of concepts described in [Uncle Bob's books on Clean Code](http://www.amazon.com/Clean-Code-Handbook-Software-Craftsmanship/dp/0132350882). 

Each check is intended to highlight a symptom that your code is becoming too complex, is not "clean" enough. Increased complexity can make your code harder to read, understand and maintain. The plugin currently checks:

* **Too many dependencies** - warns when a constructor has too many interfaces passed in.
* **Excessive indentation** - warns if a class contains members with too much indentation.
* **Too many chained references** - warns if an expression has too many chained statements, is potentially violating the [Law of Demeter](https://en.wikipedia.org/wiki/Law_of_Demeter).
* **Method too long** - warns when a method contains too many statements.
* **Class too big** - warns if a class has too many method declarations.
* **Too many method parameters** - warns if a method has too many parameters.
* **Method name not meaningful** - simple check that a method name is longer than a certain threshold.
* **Too many flag parameters in method** - warns if too many boolean parameters are used in a method declaration. Multiple boolean values can be easily mixed up.

The limits used by each analysis are configurable in the options page.

### Upcoming features

All features are [logged here](http://youtrack.codebetter.com/issues/cc)

Please use the previous issue tracker for logging bugs, feature requests, etc.

## Installing

Copy the DLL for each Plugin to the ReSharper\bin\Plugins folder

## License

Licensed under MIT (c) 2012 - 2016  Hadi Hariri and Contributors

Note: All references to [Clean Code](http://www.cleancoders.com/), including but not limited to the Clean Code icon are used with permission of Robert C. Martin (a.k.a. UncleBob)
