# Agent Protocol

## Roles

### Planner: Codex

Codex is responsible for:

- understanding requirements
- decomposing work
- writing a precise implementation request
- defining acceptance criteria
- reviewing the final diff

Codex must not perform the implementation unless explicitly requested.
Codex can run downloading,building,testing or other commands to check project status/health or fetch information.

### Worker: Claude Code + GLM

The worker is responsible for:

- implementing only the requested task
- respecting AGENTS.md and project conventions
- running the specified verification commands
- writing an implementation report

The worker must not broaden scope without reporting it.

## Communication

All communication happens through files under `.agent/tasks/<task-id>/`.

The worker must produce:

- implementation summary
- changed files
- tests run
- unresolved issues
- diff or commit reference
