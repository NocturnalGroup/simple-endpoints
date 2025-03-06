<img align="right" width="256" height="256" src="Assets/Logo.png">

<div id="user-content-toc">
  <ul align="left" style="list-style: none;">
    <summary>
      <h1>SimpleEndpoints</h1>
    </summary>
  </ul>
</div>

### Adding structure to Minimal APIs

[About](readme.md) · [Docs](docs.md) · [License](license.txt) · Contributing

---

Thank you for your interest in contributing to SimpleEndpoints!

## Code Style

We try to not be too strict on code style.
If we have any problems, we'll let you know in your pull request.
We just ask that you try to keep your code simple and maintainable.

### EditorConfig

That being said, we do require code to follow our EditorConfig.
At the root of the project there is an `.editorconfig` that enforces:

- Indentation characters
- Line endings
- Character encoding

Please make sure you have the EditorConfig plugin installed in your editor:

- [Rider (Included)](https://plugins.jetbrains.com/plugin/7294-editorconfig/)
- [Visual Studio (Included)](https://learn.microsoft.com/en-us/visualstudio/releasenotes/vs2017-relnotes-v15.0#coding-convention-support-through-editorconfig)
- [VS Code](https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig)
- [Other Editors](https://editorconfig.org/#download)

## Commit Guidelines

### Message Format

Commits messages should use the following format:

```
{scope}: {message}
```

The scope parameter should be the area that's being changed.
The message parameter should be a short description of what's changed.

```
build: update projects to use .NET 8
repo: correct a spelling mistake in the readme
parameters: add HttpRequest and HttpResponse to the base parameters
```

### Focused Commits

Where possible, commits should be focused and address a single logical change.
A single commit that makes multiple changes can be harder to review and revert.

For example, instead of combining multiple changes into a single commit like this:

```
users: add last name field to search endpoint and changed its ratelimit
```

It's better to break them down into separate, focused commits:

```
users: add support for querying by last name
users: increase the query endpoint rate limit allowance
```

## Pull Requests

Please make any pull requests to the `main` branch.
Once approved, the contributor that reviewed your pull request will handle merging it in for you!

## Questions or Problems?

If you have any questions or run into issues, please feel free to open an issue.
We're happy to help!

Thanks again for your interest in contributing!
We look forward to your contributions!
