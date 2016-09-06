# Clean Code ReSharper Plugin Project

### CleanCode

An experiment in trying to automate some of concepts described in [Uncle Bob's books on Clean Code](http://www.amazon.com/Clean-Code-Handbook-Software-Craftsmanship/dp/0132350882). 

Each check is intended to highlight a symptom that your code is becoming too complex, is not "clean" enough. Increased complexity can make your code harder to read, understand and maintain. The plugin currently checks:

* **Too many dependencies** - warns when a constructor has too many interfaces passed in.
* **Excessive indentation** - warns if a class contains members with too much indentation.
* **Too many chained references** - warns if an expression has too many chained statements, is potentially violating the [Law of Demeter](https://en.wikipedia.org/wiki/Law_of_Demeter). The check will try to ignore fluent APIs - if the type returned is the same as the type invoked, it doesn't count.
* **Method too long** - warns when a method contains too many statements.
* **Class too big** - warns if a class has too many method declarations.
* **Too many method parameters** - warns if a method has too many parameters.
* **Method name not meaningful** - simple check that a method name is longer than a certain threshold.
* **Method flag parameters** - warns if a boolean or enum method parameter is used in an `if` statement with the method. This is an indication that the method has more than one responsibility.
* **Condition complexity** - warns if the condition in an `if` statement contains too many expressions.
* **Hollow type names** - warns if a class has a name with a suffix that is too general, e.g. `Handler`, `Manager`, `Controller`. The list of names is configurable.

The limits used by each analysis are configurable in the options page.

### Upcoming features

All features are [logged here](http://youtrack.codebetter.com/issues/cc)

Please use the previous issue tracker for logging bugs, feature requests, etc.

## Installing

Install from ReSharper &rarr; Extension Manager.

## License

Licensed under MIT (c) 2012 - 2016  Hadi Hariri and Contributors

Note: All references to [Clean Code](http://www.cleancoders.com/), including but not limited to the Clean Code icon are used with permission of Robert C. Martin (a.k.a. UncleBob)
