# PlainCC-Tools

This repository is not maintained here in GitHub. Instead the latest version is stored in Microsoft VSTS where it is under CI.

This is a .Net application a Console application that takes a sales files exported from Paypal and converts the them to Magento format by parsing into objects.

Uses Automapper, FileMapper library and contains NUnit Tets.

The project 'Paypal Exporter' contains a Paypal CSV to Magento CSV converter parser written in C#. The application has the following function: to take a sales transaction feed from Paypal which records sales from eBay (and other systems). It reads each line into memory and tries to identify key information from each sales record. For an item in eBay with the description "Plain Zipped Hooded Sweatshirt Boys Girls Childrens Kids Hoodie All Colours [red,Age 4-5 years]" is converted into a strongly typed item. This

Some transactions are multi-transactions i.e. a single order contains multiple transactions. For example a customer can buy one item or can buy multiple items.
