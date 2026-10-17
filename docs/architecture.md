# Architecture and ownership

## Project boundaries

| Layer | Responsibility |
| --- | --- |
| s&box | Underlying engine, runtime, editor, and project environment |
| Lightstream | Reusable C# systems, editor extensions, and authoring tools |
| Game repositories | Game rules, player behavior, maps, and project-specific content |
| przm.DEV | Local tool launching, repository access, and development information |

Lightstream's current direction is an SDK around s&box. A standalone renderer or a port of the old Swift engine is not part of this foundation.

## Code boundaries

Runtime code belongs in `Code/`. Editor-only code belongs in `Editor/` and may use shared runtime types without introducing an editor dependency into runtime code. Supporting authoring workflows belong in `Tools/`.

These directories currently document intended ownership. They are not proof of a configured s&box package or automatic discovery by the editor.

## Integration

Use a real s&box library project for reusable implementation once the initial project is created in the supported editor. Commit its actual project metadata and required source files. Record the host project, installation steps, dependencies, and validation procedure before another game adopts it.

Projekt Alpha remains separate. This cleanup does not add a Lightstream dependency to it or move its game code into this repository.
