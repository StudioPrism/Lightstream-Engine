# Analyzer

Analyzers create diagostrics. The analyzers look at the code for stuff and if it finds
it, it creates a diagnostic.

# Diagnostic

A diagnostic is one of those CS234 things you get in the output in your visual 
studio console. We want them to all have unique numbers so we define them in 
a big static class called Diagnostics, and reference them from there.

# Fixer

The fixers respond to the diagnostics, and use roslyn to convert one peice of
code to a different peice of code.

# Tests

Each Analyzer and Fixer has its own tests. To make things simpler the tests 
are in the class itself.

The tests aren't automatically collected, you need to add them to the test 
project ( in UpgraderTests.cs )

# Test markup

In the test code you're going to see this weird [| and |] markup.

This is telling the tester that we expect a diagnostic between these two tokens.

For example if we have this code:

```
class [|Type1|] { }
```

We're telling the test that we're expecting a diagnostic to trigger for the 
name Type1 for some reason. If it does create a single diagnostic for that span
then the test will pass.

If you do this

```
class Type1 { }
```

and no diagnostics are created then it will pass.

more: https://www.meziantou.net/how-to-test-a-roslyn-analyzer.htm#marker-syntax