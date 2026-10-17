# Preserved Swift history

The standalone Swift prototype has been separated from the s&box direction of `main`. The two previous branch tips diverged, so both snapshots are preserved.

| Branch | Preserved source | Commit |
| --- | --- | --- |
| [swift-standalone](https://github.com/StudioPrism/Lightstream-Engine/tree/swift-standalone) | Latest existing `Dev` tip | `e883075844646eae07e2b4d9eb1c014bf161ad5d` |
| [archive/swift-main](https://github.com/StudioPrism/Lightstream-Engine/tree/archive/swift-main) | Main before the s&box cleanup | `5689ea459290184749903b2d83224e70b10c4958` |

The existing `Dev` branch is retained unchanged for compatibility. Use `swift-standalone` for continued Swift work. The snapshot commits above remain reference points if the branches later advance.

```sh
git fetch origin
git switch --track origin/swift-standalone
```

If the local branch already exists, use `git switch swift-standalone`. Read the README on that branch for its Swift-specific setup. The Swift files were removed from the current main tree, not erased from repository history.
