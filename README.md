# Randomize and Add Useless Usings

Randomize and Add Useless Usings is a Visual Studio IDE (not Code) extension that does the opposite of the "Remove and Sort Usings" command in C# code contexts. 

I think it's funny.

# Contributing

## Prerequisites

Make sure to install [this extension pack](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.ExtensibilityEssentials2022) - despite MSFT's best efforts it does a nice job of streamlining the whole extension dev experience. Links to its docs can be found [here](https://devblogs.microsoft.com/visualstudio/writing-extensions-just-got-easier/).

## Stuff to do

Feel free to send in PRs for, you know, stuff. Here's some stuff that would be nice to have:

- Most of the new `using` statements are for namespaces that aren't referenced, causing a compilation error. Any way to resolve that? We're not trying to break user workflows here. A big ol' static list of all the standard library namespaces could work but that seems inelegant to me.
- `global using` lines get scrambled along with all the rest, causing a compilation error since they must appear before all other `using` statements.
- CI (and unit tests???)
- CD to the marketplace, maybe on a tag push

# Support

You may be sufficiently amused by this to want to buy me a coffee. Thanks to technology you can do so here: https://ko-fi.com/lettucemode

