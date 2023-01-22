
# JeffreyEFerreiras.Utilities.Toolbox

Welcome to the Productivity code base! This toolkit was created to aid in general functionality that is not specific to any particular business domain. It is designed to be performant and can be used in performance critical paths, such as in place of regex operations.

The toolkit includes a variety of useful extension methods for common types such as strings, collections, and more. These extension methods make it easy to perform common operations on these types, without the need to write repetitive code.

## Installation
To install the toolbox, you can add the package from NuGet by running the following command in the Package Manager Console:

Install-Package Productivity

## Usage
Once the package is installed, you can start using the extension methods by including the namespace in your code:

```c# 
using Productivity;
```

You can then call the extension methods on the appropriate types. For example, to check if a string is a valid email address, you can use the following code:
```c#
string email = "example@example.com";
if(email.IsValid())
{
    // do something
}
```

## Contributing

If you would like to contribute to the development of this toolkit, please feel free to open a pull request on the GitHub repository. We welcome any contributions that improve the functionality and performance of the toolkit.