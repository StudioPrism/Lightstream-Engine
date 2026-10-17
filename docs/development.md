# Development

## Current checkout

`main` contains the s&box-oriented structure and documentation. There is no runnable project descriptor, build command, or automated runtime test suite on this branch yet. Swift Package Manager commands apply only to the preserved Swift branches.

## Establishing the first working integration

1. Use the current s&box editor and its [library project documentation](https://sbox.game/dev/doc/code/libraries/).
2. Create the supported library project in the editor and commit the actual generated project metadata and required source files. Do not invent a descriptor by copying unrelated project settings.
3. Add one small reusable system and connect it to a separate host game for validation.
4. Record the editor version, library installation steps, host project, and a reproducible smoke check in this guide.

Keep game content in its host repository. Never commit credentials, local caches, or generated IDE/build files. Keep source assets and required project metadata under version control.

## Reviewing changes

Describe the user-visible behavior and the host project used for validation. Run relevant s&box checks once executable code exists. Documentation-only changes should be checked for working links, accurate status, and consistent paths.

## Swift work

See [Swift history](swift-history.md) before checking out or continuing the standalone prototype.
