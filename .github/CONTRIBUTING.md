# Contributing to UniSpy

We would love for you to contribute to UniSpy.\
As a contributor, here are the guidelines we would like you to follow:

 - [Problem or Question?](#question)
 - [Issues and Bugs](#issue)
 - [Coding Rules](#rules)
 - [Guidelines for commit messages](#commit)

## <a name="question"></a> You have a problem or question?

Do not open issues for general support questions as we want to keep GitHub issues for bug reports and feature requests.
If you would like to chat about a problem or question, you can reach out via [Discord](https://discord.gg/Tv85Am4).

## <a name="issue"></a> Found a Bug?
If you find a bug in the source code, you can help us by
submitting an issue, or even better, you can submit a Pull Request with a fix.

## <a name="rules"></a> Coding Rules
To ensure consistency throughout the source code, keep these rules in mind:

* All features or bug fixes **must be tested** by one or more specs (Unit testing).

## <a name="commit"></a> Guidelines for commit messages

We have very precise rules over how our git commit messages can be formatted.\
UniSpy is using the [Conventional Commits specification in version 1.0.0](https://www.conventionalcommits.org/en/v1.0.0/).

### Commit message format
Each commit message consists of a **header**. The header has a special
format that includes a **type**, a **scope** and a **subject**.\
If necessary there can also be added a **body** and a **footer**:

```
[type]([scope]): [subject]
[BLANK LINE]
[body]
[BLANK LINE]
[footer]
```

The **header** is mandatory and the **scope** of the header is optional.

The footer should contain a [closing reference to an issue](https://help.github.com/articles/closing-issues-via-commit-messages/) if any.

### Revert
To indicate that the commit reverts a previous commit, it should begin with `revert: `, followed by the header of the reverted commit.\
If available the body should say: `This reverts commit <hash>.`, where the hash is the SHA of the commit being reverted.

### Type
Must be one of the following:

* **feat**: new feature for the user, not a new feature for build script. Such commit will trigger a release bumping a MINOR version.
* **fix**: a bug fix for the user, not a fix to a build script. Such commit will trigger a release bumping a PATCH version.
* **perf**: performance improvements. Such commit will trigger a release bumping a PATCH version.
* **docs**: changes to the documentation.
* **style**: formatting changes, missing semicolons, etc.
* **refactor**: a code change that neither fixes a bug nor adds a feature.
* **test**: adding missing tests, refactoring tests; no production code change.
* **build**: updating build configuration, development tools or other changes irrelevant to the user.
* **ci**: changes to our CI configuration files and scripts.

### Scope
The scope should be the name of the UniSpy server/service/lib that is affected.

List of supported scopes:

| Scope       | Project                       |
| :---------: | :---------------------------: |
| **unispylib** | UniSpyServer.Libraries.UniSpyLib               |
| **cdkey**     | UniSpyServer.Servers.CDKey                     |
| **chat**      | UniSpyServer.Servers.Chat                      |
| **gs**        | UniSpyServer.Servers.GameStatus                |
| **natneg**    | UniSpyServer.Servers.NatNegotiation            |
| **pcm**       | UniSpyServer.Servers.PresenceConnectionManager |
| **psp**       | UniSpyServer.Servers.PresenceSearchPlayer      |
| **qr**        | UniSpyServer.Servers.QueryReport               |
| **sb**        | UniSpyServer.Servers.ServerBrowser             |
| **ws**        | UniSpyServer.Servers.WebServer                 |

Exceptions to the rule are:

* no scope: useful for `refactor`, `style` and `test` changes that are done across all servers/services/libs.

### Subject
The subject contains a concise description of the change:

* use the imperative, present tense: "add" not "added" nor "adds"
* don't capitalize the first letter
* no dot (.) at the end

### Body
Just as in the **subject**, use the imperative, present tense: "add" not "added" nor "adds".\
The body should include the motivation for the change.

### Footer
The footer should reference the GitHub issues that this commit **Closes**.
If included in the **type/scope prefix**, breaking changes MUST be indicated by a **!** immediately before the **:**.\
If **!** is used, **BREAKING CHANGE:** MAY be omitted from the **footer** section, and the commit description SHALL be used to describe the breaking change.

## License
By contributing, you agree that your contributions will be licensed under the [GNU Affero General Public License v3.0](./LICENSE).

